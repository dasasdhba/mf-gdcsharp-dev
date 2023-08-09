#if TOOLS

using Godot;
using System;
using System.Threading;
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

    // writing thread list
    private Thread[] WritingThreads;
    private ThreadStart[] WritingThreadStarts;

    // main thread
    private Thread MainThread;

    public AssetCodeGenerator()
    {
        WritingThreads = new Thread[] { 
            TextureThread,
            SpriteFramesThread,
            AudioStreamThread
        };

        WritingThreadStarts = new ThreadStart[] {
            new ThreadStart(() => WritingThreadMethod(TextureParam)),
            new ThreadStart(() => WritingThreadMethod(SpriteFramesParam)),
            new ThreadStart(() => WritingThreadMethod(AudioStreamParam))
        };
    }

    // skip filesystem changed by this plugin
    private static bool _Changed = false;
    private static bool _Skip = false;

    private void MainThreadMethod()
    {
        //GD.Print(Annotation + "Start code generating...");
        WritingThreadsStart();
        WritingThreadsJoin();

        //GD.Print(Annotation + "Start code checking...");
        if (CheckingThread == null || !CheckingThread.IsAlive) 
        {
            CheckingThread = new(new ThreadStart(CheckingThreadMethod));
            CheckingThread.Start(); 
        }
        CheckingThread.Join();

        if (_Changed) 
        {
            GD.Print(Annotation + "Updated.");
            _Changed = false;
            _Skip = true;

            GetEditorInterface().GetResourceFilesystem().Scan();
        }
    }

    private void WritingThreadsStart()
    {
        for (int i = 0;i < WritingThreads.Length; i++)
        {
            Thread t = WritingThreads[i];
            if (t == null || !t.IsAlive)
            {
                WritingThreads[i] = new(WritingThreadStarts[i]);
                WritingThreads[i].Start();
            }
        }
    }

    private void WritingThreadsJoin()
    {
        foreach (Thread t in WritingThreads)
        {
            t.Join();
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

        GD.PrintRich(Annotation + "Activated.");

        // connect to godot signal
        GetEditorInterface().GetResourceFilesystem().FilesystemChanged += OnFilesystemChanged;
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        // disconnect to godot signal
        GetEditorInterface().GetResourceFilesystem().FilesystemChanged -= OnFilesystemChanged;

        // wait thread to finish
        MainThread?.Join();

        GD.Print(Annotation + "Exited.");
    }

    protected virtual void OnFilesystemChanged()
    {
        if (_Skip)
        {
            _Skip = false;
            return;
        }

        if (MainThread == null || !MainThread.IsAlive)
        {
            MainThread = new(new ThreadStart(MainThreadMethod));
            MainThread.Start();
        }
    }
}

#endif