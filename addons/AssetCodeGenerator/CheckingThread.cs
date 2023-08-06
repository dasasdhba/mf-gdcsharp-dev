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
            string respath = csStr.Substring(left, right - left);
            bool changed = false;
            if (respath.EndsWith(';')) 
            { 
                respath = respath[..^2];
                changed = true;
            }

            if (!FileAccess.FileExists(respath))
            {
                FileAccess file = FileAccess.Open(cspath, FileAccess.ModeFlags.Write);
                file.StoreString(
                    "// Warning: There is no valid resource file at: " + respath + "\n" +
                    "// Please remove this cs script file.\n"
                    );
                file.Close();
                if (changed) { _Changed = true; }

                GD.PushWarning(Annotation + "Warning: Invalid cs asset class at: " + cspath);
            }
        }
    }
}

# endif