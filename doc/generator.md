# Generator document

Here are some godot editor plugins which help generate code or do other repetitive work in this project.

## Asset Code Generator

This is a cs plugin which helps generate cs code packaging some specific resources so that we can easily get access to. Read the base class cs script in `res:\\cs\Asset` for more information.

Currently, this plugin will detect the change of project filesystem and package the resource in:

* `res:\\assets\audio` AudioStream
* `res:\\assets\texture` Texture2D
* `res:\\assets\sprite` SpriteFrames
* `res:\\assets\scene` PackedScene

There may be more resource types maintained by this plugin in the future.

The plugin will convert the resource file name to PascalCase and use it as class name, then generate code in `res:\\cs\Asset\Generated`.

If a resource file has been removed or renamed, the specific generated cs script file will also be removed. Pay attention to potential invalid references if this happens.

### Known issue

* Remove the invalid cs scripts will cause editor error since the scripts have been imported before. (Is there any way to refresh imported resource or just prevent godot to import this non-editor scripts?) This will not make the editor crash or unusable, so it may not matter a lot.

## Spawner-TSCN Generator

This is still a planning item, which may help generate tscn files for concrete spawners so that we can easily use them in level editor.
