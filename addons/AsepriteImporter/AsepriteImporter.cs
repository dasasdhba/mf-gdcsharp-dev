#if TOOLS

using Godot;

namespace Editor.Addon;

/// <summary>
/// Aseprite Importer main plugin.
/// </summary>
[Tool]
public partial class AsepriteImporter : EditorPlugin
{
    private AsepriteConfig Config;
    private AsepriteImporterPlugin Importer;

    // since importer plugin has to be constructed parameterlessly
    // (weary)
    // we set up the required instance as static
    public static AsepriteCommand Command { get; set; }
    public static EditorFileSystem ResourceFilesystem { get; set; }

    public AsepriteImporter() : base()
    {
        EditorInterface editorInterface = GetEditorInterface();

        Config = new(editorInterface.GetEditorSettings());
        Config.AddSettings();

        Command = new(Config);
        ResourceFilesystem = editorInterface.GetResourceFilesystem();
    }

    public override void _EnterTree()
    {
        base._EnterTree();

        Importer = new();
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