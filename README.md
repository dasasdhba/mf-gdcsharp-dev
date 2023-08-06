# Introduction

**This project is still in early development.**

This is a Mario Forever project based on Godot `C#`(v4.1.stable.mono.official [970459615]). **Visual Studio 2022** is recommended.

To debug the project with **VS2022**, you can create a new launch profile in `Debug->Mario Forever GdCSharp Dev Debug Properties`. Set executable path to Godot path (for example, `xxxxx\Godot_v4.1-stable_mono_win64.exe`), and add command line arguments `--path $(SolutionDir) > godot.log`, in which case you can see the Godot console output in a log file `.godot\mono\temp\bin\Debug\godot.log`. If you want to watch the Godot console output in the runtime, you can set the executable to Godot console version (for example, `Godot_v4.1-stable_mono_win64_console.exe`), and add command line arguments `--path $(SolutionDir)` instead of letting Godot output the console as log file.

You can read the documents under the folder `res:\\doc` for more information.
