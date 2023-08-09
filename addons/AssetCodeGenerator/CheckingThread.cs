#if TOOLS

using Godot;
using System.Threading;

namespace Editor.Addon;

public partial class AssetCodeGenerator : EditorPlugin
{
    // checking thread
    private Thread CheckingThread;

    private static void CheckingThreadMethod()
    {
        foreach (string cspath in GetAllFilePath("res://cs/Asset/Generated", new string[] {"cs"}))
        {
            string csStr = FileAccess.GetFileAsString(cspath);
            int left = csStr.IndexOf("res://");
            int right = csStr.IndexOf("\n", left + 1);
            string respath = csStr[left..right];
            bool changed = false;
            if (respath.EndsWith(';')) 
            { 
                respath = respath[..^2];
                changed = true;
            }

            if (!FileAccess.FileExists(respath))
            {
                DirAccess.RemoveAbsolute(ProjectSettings.GlobalizePath(cspath));

                if (changed) { _Changed = true; }

                GD.PushWarning(Annotation + "Warning: Invalid cs asset class at: " 
                    + cspath + " Has been removed.");
            }
        }
    }
}

# endif