# Introduction

**This project is still in early development.**

You can read the documents under the folder `res:\\doc` for more information.

## Workflow

This is a Mario Forever project based on `Godot 4 .NET 7`. Current Godot version:
`v4.1.stable.mono.official [970459615]`. Here are some simple introductionso of the workflow in this project.

### Visual Studio 2022

**Visual Studio 2022** is recommended for `C#` coding and debugging.

To debug the project with **VS2022**, you can create a new launch profile in `Debug->Mario Forever GdCSharp Dev Debug Properties`. Set executable path to Godot path (for example, `xxxxx\Godot_v4.1-stable_mono_win64.exe`), and add command line arguments `--path $(SolutionDir) > godot.log`, in which case you can see the Godot console output in a log file `.godot\mono\temp\bin\Debug\godot.log`. If you want to watch the Godot console output in the runtime, you can set the executable to Godot console version (for example, `Godot_v4.1-stable_mono_win64_console.exe`), and add command line arguments `--path $(SolutionDir)` instead of letting Godot output the console as log file.

### Source Generator

See `res:\\doc\generator.md`.

### Aseprite

A custom Aseprite Importer plugin has been implemented in this project (inspired by [Godot Aseprite Wizard](https://github.com/viniciusgerevini/godot-aseprite-wizard)). To set up the plugin, you can either set the path containing `aseprite.exe` to system `PATH`, or just set the full path of `aseprite.exe` to `Editor->Editor Settings->Aseprite->General->Aseprite Exec Path`.

#### Usage

The importer will auto generate `CompressedTexture2D` .png file and `SpriteFrames` .tres file when importing .aseprite or .ase file. (Due to `Import Mode`, only generating the texture file is also possible.) Here are two animation generating patterns:

1. Use `Tag` in Aseprite. You can set up `Tag` in Aseprite, then the importer will use the tag name to create animation. The tag direction `Forward`, `Reverse`, `Ping-pong` is also supported. However, the untagged part will be ignored while the spritesheet is still entire, please avoid this as possible.

2. Use seperate aseprite files. You can create several different aseprite files with the same `Basename` and different `Animation Name` by using a dot `.` separator. For exmaple, putting `mario.Walk.aseprite` and `mario.Jump.aseprite` in the same folder will let the importer generate a single `SpriteFrames` named `mario.tres` with two animations `Walk` and `Jump`. If there is no `Animation Name` found, then the importer will use `Default` as animation name. (Please avoid using `Default` in `Animation Name` or using `.` in `Basename`, as it may cause conflicts.)

These two patterns can also be used together, in which case the tag name will be appended to the animation name to avoid confusing.

If you don't need to reimport anymore, then the aseprite files will become useless. Therefore, feel free to remove the aseprite files and the plugin when exporting.
