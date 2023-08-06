using Component;
using Godot;

namespace GlobalClass;

/// <summary>
/// Overlap2D handles overlapping test.
/// Sync shape with specific CollisionObject2D.
/// </summary>
[GlobalClass]
public partial class OverlappingSync2D : Overlapping2D
{

    private OverlapCollisionSync2D OverlapObject = new();
    protected override OverlapManager2D GetOverlapManager() => OverlapObject;

    [ExportCategory("OverlappingSync2D")]

    [Export]
    public CollisionObject2D SyncCollisionObject
    {
        get => _SyncCollisionObject;
        set
        {
            if (_SyncCollisionObject != value)
            {
                if (ExcludeSyncObject && _SyncCollisionObject != null) 
                { 
                    OverlapObject.RemoveException(_SyncCollisionObject); 
                }

                _SyncCollisionObject = value;
                OverlapObject.SyncObject = value;
            }
        }
    }
    private CollisionObject2D _SyncCollisionObject;

    [Export]
    public bool ExcludeSyncObject
    {
        get => _ExcludeSyncObject;
        set
        {
            if (_ExcludeSyncObject != value)
            {
                if (SyncCollisionObject != null)
                {
                    if (_ExcludeSyncObject) { OverlapObject.RemoveException(SyncCollisionObject); }
                    else { OverlapObject.AddException(SyncCollisionObject); }
                }

                _ExcludeSyncObject = value;
            }
        }
    }
    private bool _ExcludeSyncObject = true;

    public OverlappingSync2D() : base()
    {
        OverlapObject.SyncObject = SyncCollisionObject;

        if (ExcludeSyncObject && SyncCollisionObject != null)
        {
            OverlapObject.AddException(SyncCollisionObject);
        }
    }

}