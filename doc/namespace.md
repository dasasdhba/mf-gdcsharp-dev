# Namespace document

The `namespace` list of this project.

* `Utils`: Collection of helpful tool functions.
* `GlobalClass`: Collection of custom Godot editor classes.

The global Game namespace.

* `Game`: Global game systems and constants.
* `Game.Const`: Global game constants.

The Component namepace.

* `Component`: Collection of the base class of components mainly used by Entity.
* `Component.BT`: Collection of behavior tree components.

The Entity namespace.

* `Entity`: Collection of the base Entity.
* `Entity.Player`: Player entity and some static player data.
* `Entity.Fluid`: Water and lava entity.

The Spawner namespace has similar struct with Entity namespace.

* `Spawner`: Colletion of the base Spawner.
* `Spawner.Player`: Collection of the player Spawner.
* ...

The Asset namespace managed automatically by Godot editor tool.

* `Asset`: Collection of asset holders.
* `Asset.Texture`: Automatically generated Texture2DHolder by Godot editor tool.
* `Asset.Sprite`: Automatically generated SpriteFramesHolder by Godot editor tool.
* `Asset.Audio`: Automatically generated AudioStreamHolder by Godot editor tool.
* `Asset.Scene`: Automatically generated PackedScene components by Godot editor tool.

The Editor Tool.

* `Editor.Addon`: C# based Godot editor addons in this project.
