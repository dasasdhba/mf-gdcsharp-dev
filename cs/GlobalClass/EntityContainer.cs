using Entity;
using Godot;

namespace GlobalClass;

[GlobalClass]
/// <summary>
/// A container for Entity2D.
/// Using this class to make metadata compatible with Entity2D.
/// </summary>
public partial class EntityContainer : RefCounted
{

    private Entity2D Entity;

    public EntityContainer(Entity2D entity) : base() => Entity = entity;

    public Entity2D GetEntity() => Entity;
}