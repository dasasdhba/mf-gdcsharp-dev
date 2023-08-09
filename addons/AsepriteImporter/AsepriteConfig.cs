#if TOOLS

using Godot;
using Godot.Collections;

namespace Editor.Addon;

/// <summary>
/// Manage Aseprite Importer editor config.
/// </summary>
[Tool]
public partial class AsepriteConfig
{
    private static readonly string SectionKey = "aseprite";

    // editor settings
    private static readonly string EditorAsepritePath = 
        SectionKey +"/general/aseprite_exec_path";

    // project settings
    private static readonly string ProjectExclusionLayer =
        SectionKey + "/general/layer/default_exclude_layers_pattern";
    private static readonly string ProjectRemoveJson =
        SectionKey + "/general/json/remove_json_file";

    private EditorSettings EditorSetting;

    public AsepriteConfig(EditorSettings editorSetting) => EditorSetting = editorSetting;

    public string GetAsepritePath() 
        => (string)GetEditorSetting(EditorAsepritePath, "aseprite.exe");

    public static string GetExclusionPattern() 
        => (string)GetProjectSetting(ProjectExclusionLayer, "");

    public static bool GetRemoveJson()
        => (bool)GetProjectSetting(ProjectRemoveJson, true);
    
    public void AddSettings()
    {
        // editor settings
        AddEditorSetting(EditorAsepritePath, "aseprite.exe");

        // project settings
        AddProjectSetting(ProjectExclusionLayer, "");
        AddProjectSetting(ProjectRemoveJson, true);
    }

    public void RemoveSettings()
    {
        // editor settings
        RemoveEditorSetting(EditorAsepritePath);

        // project settings
        RemoveProjectSetting(ProjectExclusionLayer);
        RemoveProjectSetting(ProjectRemoveJson);
    }

    protected void AddEditorSetting(string key, Variant defaultValue)
    {
        if (EditorSetting.HasSetting(key)) { return; }
        EditorSetting.SetSetting(key, defaultValue);
        EditorSetting.SetInitialValue(key, defaultValue, false);
        EditorSetting.AddPropertyInfo(new Dictionary()
        {
            {"name", key },
            {"type", (int)(defaultValue.VariantType) },
            {"hint", (int)PropertyHint.None }
        });
    }

    protected void RemoveEditorSetting(string key)
    {
        if (EditorSetting.HasSetting(key))
        {
            EditorSetting.Erase(key);
        }
    }

    protected Variant GetEditorSetting(string key, Variant defaultValue)
    {
        if (EditorSetting.HasSetting(key))
            return EditorSetting.GetSetting(key);
        return defaultValue;
    }

    protected static void AddProjectSetting(string key, Variant defaultValue)
    {
        if (ProjectSettings.HasSetting(key)) { return; }

        ProjectSettings.SetSetting(key, defaultValue);
        ProjectSettings.SetInitialValue(key, defaultValue);
        ProjectSettings.AddPropertyInfo(new Dictionary()
        {
            {"name", key },
            {"type", (int)(defaultValue.VariantType) },
            {"hint", (int)PropertyHint.None }
        });
    }

    protected static void RemoveProjectSetting(string key)
    {
        if (ProjectSettings.HasSetting(key)) 
        {
            ProjectSettings.Clear(key);
        }
    }

    protected static Variant GetProjectSetting(string key, Variant defaultValue) 
        => ProjectSettings.GetSetting(key, defaultValue);
}

#endif