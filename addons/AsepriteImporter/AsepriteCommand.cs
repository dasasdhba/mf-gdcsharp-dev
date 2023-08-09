#if TOOLS

using System;
using Godot;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;

namespace Editor.Addon;

/// <summary>
/// Aseprite command line tool.
/// </summary>
[Tool]
public partial class AsepriteCommand
{
    private static readonly string CachedPath = "res://.godot/.aseprite_cache";

    private AsepriteConfig Config;

    public AsepriteCommand(AsepriteConfig config) => Config = config;

    public bool IsAsepriteValid()
    {
        string asepritePath = Config.GetAsepritePath();

#pragma warning disable CS0168 // Variable is declared but never used
        try
        {
            OS.Execute(asepritePath, new string[] { "--version" }, new Godot.Collections.Array(), true);
        }
        catch(Exception _)
        {
            GD.PushWarning("invalid Aseprite exec path, please set it up in " +
                "Editor->Editor Setting->Aseprite->General");
            return false;
        }
#pragma warning restore CS0168 // Variable is declared but never used

        return true;
    }

    public Dictionary<string, string> ExportFile(string fileName, string outputFolder,
        Dictionary<string, Variant> options)
    {
        if (!IsAsepriteValid())
            return new Dictionary<string, string>();

        string exceptionPattern = "";
        if (options.TryGetValue("exception_pattern", out Variant var))
            exceptionPattern = (string)var;

        bool onlyVisibleLayers = false;
        if (options.TryGetValue("only_visible_layers", out var))
            onlyVisibleLayers = (bool)var;

        string outputName = GetFileBasename(fileName);
        string resFileName = outputFolder + "/" + outputName;
        string resData = resFileName + ".json";
        string resSprite = resFileName + ".png";

        string outputDir = ProjectSettings.GlobalizePath(outputFolder);
        string outputFile = outputDir + "/" + outputName;
        string dataFile = outputFile + ".json";
        string spriteSheet = outputFile + ".png";

        string dataFileCommand = IsCached(resData, resData) ? "" : dataFile;
        string spriteSheetCommand = IsCached(resSprite, resSprite) ? "" : spriteSheet;

        Dictionary<string, string> result = new()
        {
            {"data_file", resData },
            {"sprite_sheet", resSprite }
        };
        
        if (dataFileCommand == "" && spriteSheetCommand == "")
            return result;
        
        string sourceName = ProjectSettings.GlobalizePath(fileName);
        Godot.Collections.Array<string> arguments = 
            GetCommandArguments(sourceName, dataFile, spriteSheet);

        if (!onlyVisibleLayers)
            arguments.Insert(0, "--all-layers");

        AddSheetTypeArguments(arguments, options);
        AddIgnoreLayerArguments(sourceName, arguments, exceptionPattern);

        Godot.Collections.Array output = new();
        if (Execute(arguments.ToArray(), output) != 0)
        {
            GD.PushError("Aseprite: failed to export layer spritesheet");
            GD.PushError(output);
            return new Dictionary<string, string>();
        }

        SetCache(result);

        return result;
    }

    private static void SetCache(Dictionary<string, string> cache)
    {
        ConfigFile cfg = new();
        if (Godot.FileAccess.FileExists(CachedPath))
            cfg.Load(CachedPath);

        string data = GetFileHash(cache["data_file"]);
        string spr = GetFileHash(cache["sprite_sheet"]);

        cfg.SetValue("aseprite_importer", cache["data_file"], data);
        cfg.SetValue("aseprite_importer", cache["sprite_sheet"], spr);

        cfg.Save(CachedPath);
    }

    private static bool IsCached(string filepath, string item)
    {
        ConfigFile cfg = new();
        if (Godot.FileAccess.FileExists(CachedPath))
            cfg.Load(CachedPath);
        else
            return false;

        string hash = GetFileHash(filepath);
        return (string)cfg.GetValue("aseprite_importer", item, "Error") == hash;
    }

    private static string GetFileHash(string filepath)
    {
        if (!Godot.FileAccess.FileExists(filepath))
            return "";

        string globalPath = ProjectSettings.GlobalizePath(filepath);
        FileStream stream = new(globalPath, FileMode.Open);

        SHA256 Sha256 = SHA256.Create();
        byte[] by = Sha256.ComputeHash(stream);
        Sha256.Clear();

        stream.Close();

        return BitConverter.ToString(by).Replace("-", "").ToLower();
    }

    private int Execute(string[] arguments, Godot.Collections.Array output)
        => OS.Execute(Config.GetAsepritePath(), arguments, output, true, true);

    private static Godot.Collections.Array<string> GetCommandArguments(
    string sourceName, string dataPath, string spritesheetPath)
    {
        if (spritesheetPath == "")
        {
            return new Godot.Collections.Array<string>()
            {
                "-b",
                "--list-tags",
                "--data",
                dataPath,
                "--format",
                "json-array",
                sourceName
            };
        }

        if (dataPath == "")
        {
            return new Godot.Collections.Array<string>()
            {
                "-b",
                "--sheet",
                spritesheetPath,
                sourceName
            };
        }

        return new Godot.Collections.Array<string>()
        {
            "-b",
            "--list-tags",
            "--data",
            dataPath,
            "--format",
            "json-array",
            "--sheet",
            spritesheetPath,
            sourceName
        };
    }

    private static string GetFileBasename(string filepath)
    {
        string fileName = StringExtensions.GetFile(filepath);
        int extLen = StringExtensions.GetExtension(fileName).Length;
        return fileName[..^(extLen + 1)];
    }

    private static void AddSheetTypeArguments(
        Godot.Collections.Array<string> arguments, Dictionary<string, Variant> options)
    {
        int count = 0;
        if (options.TryGetValue("column_count", out Variant var))
            count = (int)var;

        if (count > 0)
        {
            arguments.Add("--merge-duplicates");
            arguments.Add("--sheet-columns");
            arguments.Add(Convert.ToString(count));
        }
        else
        {
            arguments.Add("--sheet-pack");
        }
    }

    private void AddIgnoreLayerArguments(string sourceName,
        Godot.Collections.Array<string> arguments, string exceptionPattern)
    {
        Godot.Collections.Array<string> layers = 
            GetExceptionLayers(sourceName, exceptionPattern);

        if (layers.Count == 0)
            return;

        foreach (string str in layers)
        {
            arguments.Insert(0, str);
            arguments.Insert(0, "--ignore-layer");
        }

    }

    private Godot.Collections.Array<string> GetLayers(
        string sourceName, bool onlyVisible = false)
    {
        Godot.Collections.Array output = new();
        Godot.Collections.Array<string> arguments = new()
        {
            "-b",
            "--list-layers",
            sourceName
        };

        if (!onlyVisible)
            arguments.Insert(0, "--all-layers");

        if (Execute(arguments.ToArray(), output) != 0)
        {
            GD.PushError("Aseprite: failed listing layers.");
            GD.PushError(output);
            return new Godot.Collections.Array<string>();
        }

        if (output.Count == 0)
            return new Godot.Collections.Array<string>();

        Godot.Collections.Array<string> result = new();
        foreach (string str in output[0].ToString().Split('\n'))
            result.Add(StringExtensions.StripEdges(str));

        return result;
    }

    private Godot.Collections.Array<string> GetExceptionLayers(
        string sourceName, string exceptionPattern)
    {
        Godot.Collections.Array<string> result = new();

        Godot.Collections.Array<string> layers = GetLayers(sourceName);
        RegEx regex = CompileRegex(exceptionPattern);

        if (regex == null)
            return result;

        foreach (string str in layers)
        {
            if (regex.Search(str) != null)
            {
                result.Add(str);
            }
        }

        return result;
    }

    private static RegEx CompileRegex(string pattern)
    {
        if (pattern == "")
            return null;

        RegEx rgx = new();
        if (rgx.Compile(pattern) == Error.Ok)
            return rgx;

        GD.PushError("Exception regex error.");
        return null;
    }
}

#endif