#if TOOLS

using Godot;

namespace Editor.Addon;

/// <summary>
/// Aseprite Importer main plugin.
/// </summary>
[Tool]
public partial class AsepriteImporter : EditorPlugin
{
    private AsepriteCommand Command;
    private AsepriteConfig Config;
    private AsepriteImporterPlugin Importer;

    public override void _EnterTree()
    {
        base._EnterTree();

        EditorInterface editorInterface = GetEditorInterface();

        Config = new(editorInterface.GetEditorSettings());
        Config.AddSettings();

        Command = new(Config);

        Importer = new(Command, editorInterface.GetResourceFilesystem());
        AddImportPlugin(Importer);
    }

    public override void _ExitTree()
    {
        base._ExitTree();

        Config.RemoveSettings();
        RemoveImportPlugin(Importer);
    }
}

#endif