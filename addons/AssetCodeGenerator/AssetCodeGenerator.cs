#if TOOLS

using Godot;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Editor.Addon;

/// <summary>
/// Asset Code Generator that helps generate code.
/// See <c>res:\\doc\generator.md</c> for more information.
/// </summary>
[Tool]
public partial class AssetCodeGenerator : EditorPlugin
{
    private static readonly string Annotation = "[Asset Code Generator] ";

    private static readonly string Comment =
    "// This is an automatically generated cs script by Godot editor tool.";

    // skip filesystem changed by this plugin
    private static bool _Changed = false;
    private static bool _Skip = false;

    private static async void GenerateAsync()
    {
        //GD.Print(Annotation + "Start code generating...");
        Task[] tasks = new Task[]
        {
            WritingAsync(TextureParam),
            WritingAsync(SpriteFramesParam),
            WritingAsync(AudioStreamParam),
            WritingAsync(PackedSceneParam)
        };

        foreach (Task task in tasks)
            await task;


        //GD.Print(Annotation + "Start code checking...");
        await CheckingAsync();

        if (_Changed) 
        {
            GD.Print(Annotation + "Updated.");
            _Changed = false;
            _Skip = true;
        }
    }

    private static string FilePathToPascal(string filePath)
    {
        string file = StringExtensions.GetFile(filePath);
        int extLen = StringExtensions.GetExtension(file).Length;
        string fileCutExt = file[..^(extLen + 1)];
        return StringExtensions.ToPascalCase(fileCutExt);
    }

    private static IEnumerable<string> GetAllFilePath(string dirPath, string[] filter)
    {
        foreach (string dir in DirAccess.GetDirectoriesAt(dirPath))
        {
            foreach (string result in GetAllFilePath(dirPath + "/" + dir, filter))
            {
                yield return result;
            }
        }
        foreach (string file in DirAccess.GetFilesAt(dirPath))
        {
            if (filter.IsEmpty() || Array.Exists(filter, str => str == StringExtensions.GetExtension(file)))
            {
                yield return dirPath + "/" + file;
            }
        }

        yield break;
    }

    public override void _EnterTree()
    {
        base._EnterTree();

        GD.Print(Annotation + "Activated.");

        // connect to godot signal
        GetEditorInterface().GetResourceFilesystem().FilesystemChanged += OnFilesystemChanged;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        // disconnect to godot signal
        GetEditorInterface().GetResourceFilesystem().FilesystemChanged -= OnFilesystemChanged;

        GD.Print(Annotation + "Exited.");
    }

    protected virtual void OnFilesystemChanged()
    {
        if (_Skip)
        {
            _Skip = false;
            return;
        }

        GenerateAsync();
    }
}

#endif