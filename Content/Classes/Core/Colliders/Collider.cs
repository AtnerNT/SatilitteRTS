using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Core;

internal abstract class Collider
{
    public Vector2 Position { get; set; }
    public static QuadTree QuadTree;

    private List<Collider> collidersToTest;

    public Collider()
    {
        QuadTree.Insert(this);
    }

    public void UpdatePosition(Vector2 position)
    {
        Position = position;
    }

    public static BoxCollider EnsureBox(Collider collider)
    {
        BoxCollider colliderBox = collider as BoxCollider;
        if (colliderBox == null) colliderBox = (collider as CircleCollider).GetAABB();

        return colliderBox;
    }

    public virtual bool TestCollisionsAtPos(Vector2 position)
    {
        Vector2 oldPos = Position;
        Position = position;

        bool returnVal = IsColliding();
        Position = oldPos;

        return returnVal;
    }

    public virtual bool IsColliding()
    {
        collidersToTest = QuadTree.Retrieve(null, this);

        foreach (Collider collider in collidersToTest)
        {
            if (collider == this) continue;
            if (collider as CircleCollider != null)
            {
                if (TestCircleCollision(collider as CircleCollider) == true) return true;
            }
            else if (collider as BoxCollider != null)
            {
                if (TestBoxCollision(collider as BoxCollider) == true) return true;
            }
        }

        return false;
    }

    protected virtual bool TestCircleCollision(CircleCollider circleCollider)
    {
        return false;
    }

    protected virtual bool TestBoxCollision(BoxCollider boxCollider)
    {
        return false;
    }
}
