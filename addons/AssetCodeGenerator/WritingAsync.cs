#if TOOLS

using Godot;
using System.Threading.Tasks;

namespace Editor.Addon;

public partial class AssetCodeGenerator : EditorPlugin
{
    // texture thread
    private static readonly WritingThreadParam TextureParam = new(
        "Asset.Texture",
        "Texture2DHolder",
        "TexturePath",
        "res://cs/Asset/Generated/Texture",
        "res://assets/texture",
        new string[] { "tres", "bmp", "dds", "exr", "hdr", "jpg", "jpeg", "png", "tga", "svg", "svgz", "webp" }
        );

    // sprite frames thread
    private static readonly WritingThreadParam SpriteFramesParam = new(
        "Asset.Sprite",
        "SpriteFramesHolder",
        "SpriteFramesPath",
        "res://cs/Asset/Generated/Sprite",
        "res://assets/sprite",
        new string[] { "tres" }
        );

    // audio stream thread
    private static readonly WritingThreadParam AudioStreamParam = new(
        "Asset.Audio",
        "AudioStreamHolder",
        "AudioStreamPath",
        "res://cs/Asset/Generated/Audio",
        "res://assets/audio",
        new string[] { "wav", "mp3", "ogg" }
        );

    // packed sceen thread
    private static readonly WritingThreadParam PackedSceneParam = new(
        "Asset.Scene",
        "PackedSceneHolder",
        "PackedScenePath",
        "res://cs/Asset/Generated/PackedScene",
        "res://assets/scene",
        new string[] { "tscn" }
        );

    // writing thread template
    private struct WritingThreadParam
    {
        public string NameSpace;
        public string Classname;
        public string Property;
        public string CsPath;
        public string ResPath;
        public string[] Filter;

        public WritingThreadParam(string n, string cn, string p, string cs, string r, string[] f)
        {
            NameSpace = n;
            Classname = cn;
            Property = p;
            CsPath = cs;
            ResPath = r;
            Filter = f;
        }
    }

    private static async Task WritingAsync(WritingThreadParam param)
    {
        await Task.Run(() =>
        {
            foreach (string respath in GetAllFilePath(param.ResPath, param.Filter))
            {
                string classname = FilePathToPascal(respath);
                string cspath = param.CsPath + "/" + classname + ".cs";

                if (FileAccess.FileExists(cspath))
                {
                    string fileStr = FileAccess.GetFileAsString(cspath);
                    if (fileStr.Contains(param.Property + " = \"" + respath + "\";")) { continue; }
                }

                FileAccess file = FileAccess.Open(cspath, FileAccess.ModeFlags.Write);
                file.StoreString(
                    Comment + "\n" +
                    "\n" +
                    "namespace " + param.NameSpace + ";\n" +
                    "\n" +
                    "public partial class " + classname + " : " + param.Classname + "\n" +
                    "{\n" +
                    "    public " + classname + "() => " + param.Property + " = \"" + respath + "\";\n" +
                    "}\n"
                    );
                file.Close();
                _Changed = true;

                GD.PrintRich("[color=green]" + Annotation + param.Classname + " [b]" + classname + "[/b] updated.[/color]");

            }
        });
        
    }
}

#endif