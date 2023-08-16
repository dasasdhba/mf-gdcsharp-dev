# The programming pattern

It's highly recommended to read this book if you never read it:

<gameprogrammingpatterns.com>

With Godot engine you don't need to read the whole book of course, section 2, 5, 6, 7, 12, 13, 14, 15 is recommended.

The followings are also some simple introductions of the patterns used in this project.

## Entity-Component-Spawner pattern

This pattern is invented and named by myself, which may sound like traditional entity-component-system, but they are not exactly the same. A fun fact is that they have the same abbr ECS.

### Entity

In this project, `Entity2D` is an abstract class that will be inherited by all entities. Entity is NOT a node at all. You can use `Entity2D.Init(node)` to add its parts of "components" to the SceneTree, and everything will work fine.

In a word, entity is where we organize the "components" and use them to implement almost everything, just like a node tree editor. Since we can do more abstract designs through this layer, this would also be much more flexible.

You can read the document comments in the `res:\\cs\Entity\Base\Entity2D.cs` for more information.

### Component

There is no general `Component` class, a component can either be a node or not. Some widely used components are placed in namespace `Component` or `GlobalClass`. Generally, components should not know much about each other in order to prevent unnecessary coupling. The necessary connections between different components are organized by either passing arguments by or using observer pattern through entity init.

### Spawner

`Spawner2D` is an abstract class inherited from `Node2D` that will be inherited by all spawners. Spawner is more an editor tool than an entity generator in this project. It will also be helpful when implmenting some "reborn" feature such as lakitu. To implement a concrete spawner, you have to override an abstract method `abstract Entity2D Spawn()`. Here you can just create an new entity and simply modify some its parameters and then return. The method `Entity2D.Init(Node parent)` mentioned above will be called by the method `Respawn()` later.

### No System

There is no "system", the node components in the SceneTree will just run their `_Process(double delta)` and `_PhysicsProcess(double delta)`. Managing systems and querying entities is just not a fun work, and actually, traditional ECS pattern is used more for optimizing but not for quicker and easier development (in my opinion currently). Since Mario Forever is a simple and small game project, it's not really necessary to consider these optimizing patterns.

### Summary

In this pattern, `Entity2D` can init and access to almost everything it owns, so that we can design some inheritance relationship based on it conveniently. Components are organized by entity to avoid coupling. `Spawner2D` helps us "place" our `Entity2D` in the Godot editor and makes the development of some great editor tools possible.

## Common pitfalls

The pitfalls summarized by the Godot official document is not enouch for me, so that's it.

1. Vector2.X/Y is float type by default, so Vector2 \* double is not allowed.

2. Before making change for exported resource in inspector, don't forget create a new resource, or all the change will be ignored.

3. You can't assign a default value to an exported custom resource in C# currently, see: <https://github.com/godotengine/godot/issues/80175>

4. Godot.Variant is a struct, which means even if a `Godot.Variant` `variant` contains a `GodotObject` for example, the statement `variant is GodotObject` will alwasy be `false` and `variant as GodotObject` will be null. But unsafely cast `(GodotObject)variant` works.
