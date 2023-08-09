#if TOOLS

using Godot;
using Godot.Collections;
using System;
using System.Threading;
using System.Collections;

namespace Editor.Addon;

/// <summary>
/// Aseprite Importer that generates useful resource while importing aseprite files.
/// The aseprite resource itself is useless and can be removed
/// if there is no need to reimport.
/// </summary>
[Tool]
public partial class AsepriteImporterPlugin : EditorImportPlugin
{
    private AsepriteCommand Command;
    private EditorFileSystem ResourceFilesystem;

    public AsepriteImporterPlugin(AsepriteCommand command, EditorFileSystem resSystem) =>
        (Command, ResourceFilesystem) = (command, resSystem);

    // override implement
    public override string _GetImporterName()
        => "aseprite_importer.plugin";

    public override string _GetVisibleName()
        => "Aseprite Importer";

    public override string[] _GetRecognizedExtensions()
        => new string[] { "aseprite", "ase" };

    public override string _GetSaveExtension()
        => "res";

    public override string _GetResourceType()
        => "Resource";

    public override int _GetPresetCount() => 1;

    public override string _GetPresetName(int _)
        => "Default";

    public override float _GetPriority() => 1.0f;

    public override int _GetImportOrder() => 1;

    public override bool _GetOptionVisibility(string _, StringName __, Dictionary ___)
        => true;

    public override Array<Dictionary> _GetImportOptions(string _, int __)
        => new()
        {
            new Dictionary()
            {
                {"name", "exclude_layers_pattern" },
                {"default_value", AsepriteConfig.GetExclusionPattern() }
            },
            new Dictionary()
            {
                {"name", "only_visible_layers" },
                {"default_value", false }
            },
            new Dictionary()
            {
                {"name", "tag_only" },
                {"default_value", true }
            },
            new Dictionary()
            {
                {"name", "loop" },
                {"default_value", true }
            },
            new Dictionary()
            {
                {"name", "import_mode" },
                {"default_value", "SpriteFrames" },
                {"property_hint", (int)PropertyHint.Enum },
                {"hint_string", "Texture2D,SpriteFrames,SpriteFramesOffset" }
            },
            new Dictionary()
            {
                {"name", "sheet_type" },
                {"default_value", "Packed" },
                {"property_hint", (int)PropertyHint.Enum },
                {"hint_string", GetSheetTypeHintString() }
            }
        };

    private static string GetSheetTypeHintString()
    {
        string hint = "Packed";
        foreach (int i in new int[] { 2, 4, 8, 16, 32 })
            hint += "," + Convert.ToString(i) + " columns";
        hint += ",Strip";
        return hint;
    }

    private static int GetSheetTypeColumns(string str) 
        => str switch
        {
            "Packed" => 0,
            "2 columns" => 2,
            "4 columns" => 4,
            "8 columns" => 8,
            "16 columns" => 16,
            "32 columns" => 32,
            "Strip" => 128,
            _ => 0
        };

    public override Error _Import(string sourceFile, string savePath,
        Dictionary options, Array<string> _, Array<string> __)
    {
        Error err = GenerateImportFiles(sourceFile, options);

        if (err == Error.Ok)
        {
            // generate useless resource when successful
            ResourceSaver.Save(new Aseprite(), savePath + ".res");
        }

        return err;
    }

    private Error GenerateImportFiles(string sourceFile, Dictionary options)
    {
        System.Collections.Generic.Dictionary<string, Variant> aseOpt = new()
        {
            {"exception_pattern", options["exclude_layers_pattern"] },
            {"only_visible_layers", options["only_visible_layers"] },
            {"column_count",  GetSheetTypeColumns((string)options["sheet_type"]) }
        };

        // texture only pattern
        if ((string)options["import_mode"] == "Texture2D")
        {
            System.Collections.Generic.Dictionary<string, string> outFile =
                Command.ExportFile(sourceFile, 
                StringExtensions.GetBaseDir(sourceFile), aseOpt);

            if (!outFile.ContainsKey("sprite_sheet"))
            {
                GD.PushError("Importing aseprite file failed, please check" +
                    "Aseprite Exec Path in editor settings.");
                return Error.Failed;
            }

            // import image and clear cache
            ResourceFilesystem.UpdateFile(outFile["sprite_sheet"]);
            AppendImportExternalResource(outFile["sprite_sheet"]);
            GD.Load(outFile["sprite_sheet"]).TakeOverPath(outFile["sprite_sheet"]);

            if (AsepriteConfig.GetRemoveJson())
            {
                DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(outFile["data_file"]));
            }
        }
        // spriteframes pattern
        else
        {
            // generate texture and json
            System.Collections.Generic.List<AsepriteFrames.FramesInfo> infos = new();

            foreach (string file in GetRelevantFiles(sourceFile))
            {
                System.Collections.Generic.Dictionary<string, Variant> opt;
                bool loop = (bool)options["loop"];
                bool tagOnly = (bool)options["tag_only"];

                if (file != sourceFile && FileAccess.FileExists(file + ".import"))
                {
                    ConfigFile config = new();
                    config.Load(file + ".import");

                    opt = new()
                    {
                        {"exception_pattern", config.GetValue(
                            "params", "exclude_layers_pattern", "") },
                        {"only_visible_layers", config.GetValue(
                            "params", "only_visible_layers", false) },
                        {"column_count",  GetSheetTypeColumns((string)config.GetValue(
                            "params", "sheet_type", "Packed")) }
                    };

                    loop = (bool)config.GetValue("params", "loop", true);
                    tagOnly = (bool)config.GetValue("params", "tag_only", true);
                }
                else
                {
                    opt = aseOpt;
                }
                
                // generate
                System.Collections.Generic.Dictionary<string, string> outFile =
                    Command.ExportFile(file,
                    StringExtensions.GetBaseDir(file), opt);

                if (!outFile.ContainsKey("sprite_sheet"))
                {
                    GD.PushError("Importing aseprite file failed, please check" +
                        "Aseprite Exec Path in editor settings.");
                    return Error.Failed;
                }

                infos.Add(new AsepriteFrames.FramesInfo(
                    outFile["sprite_sheet"],
                    LoadJson(outFile["data_file"]),
                    GetAnimName(file),
                    loop,
                    tagOnly
                    ));

                // image has to be importred first
                ResourceFilesystem.UpdateFile(outFile["sprite_sheet"]);
                AppendImportExternalResource(outFile["sprite_sheet"]);

                if (AsepriteConfig.GetRemoveJson())
                {
                    DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(outFile["data_file"]));
                }
            }

            SpriteFrames spr = new AsepriteFrames(infos).Create(
                (string)options["import_mode"] == "SpriteFramesOffset");

            string resouceFile = StringExtensions.GetBaseDir(sourceFile) + "/" +
                GetBaseName(sourceFile) + ".tres";
            spr.TakeOverPath(resouceFile);
            ResourceSaver.Save(spr, resouceFile);
        }

        return Error.Ok;
    }

    private static string GetBaseName(string sourceFile)
    {
        string fileName = StringExtensions.GetFile(sourceFile);
        int extLen = StringExtensions.GetExtension(fileName).Length;
        string BaseName =  fileName[..^(extLen + 1)];
        int dotPos = BaseName.LastIndexOf('.');
        if (dotPos == -1)
        {
            return BaseName;
        }
        return BaseName[..dotPos];
    }

    private static string GetAnimName(string sourceFile)
    {
        string fileName = StringExtensions.GetFile(sourceFile);
        int extLen = StringExtensions.GetExtension(fileName).Length;
        string BaseName = fileName[..^(extLen + 1)];
        int dotPos = BaseName.LastIndexOf('.');
        if (dotPos == -1)
        {
            return "Default";
        }
        return BaseName[(dotPos+1)..^0];
    }

    private IEnumerable GetRelevantFiles(string sourceFile) 
    {
        string path = StringExtensions.GetBaseDir(sourceFile);
        string baseName = GetBaseName(sourceFile);

        foreach (string file in DirAccess.GetFilesAt(path))
        {
            if (System.Array.Exists(_GetRecognizedExtensions(),
                str => str == StringExtensions.GetExtension(file))
                && GetBaseName(file) == baseName)
            {
                yield return path + "/" + file;
            }
        }

        yield break;
    }

    private static Dictionary LoadJson(string json)
    {
        FileAccess file = FileAccess.Open(json, FileAccess.ModeFlags.Read);
        Json jsonObj = new();
        jsonObj.Parse(file.GetAsText());
        file.Close();

        return (Dictionary)jsonObj.Data;
    }
}

#endif