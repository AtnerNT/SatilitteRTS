using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Core;

internal class CircleCollider : Collider
{
    public float Radius { get; private set; } = 0;

    public BoxCollider GetAABB()
    {
        BoxCollider box = new BoxCollider();
        return box;
    }

    protected override bool TestCircleCollision(CircleCollider circleCollider)
    {
        float distance = Vector2.Distance(circleCollider.Position, Position);
        return distance <= Radius + circleCollider.Radius;
    }

    protected override bool TestBoxCollision(BoxCollider boxCollider)
    {
       System.Drawing.RectangleF thisBox = GetAABB().CollisionBox;

       if(thisBox.IntersectsWith(boxCollider.CollisionBox) == false) return false;

        float testx = Position.X;
        float testy = Position.Y;

        if (Position.X < boxCollider.Position.X) testx = boxCollider.Position.X;
        else if (Position.X > boxCollider.Position.X + boxCollider.Width) testx = boxCollider.Position.X + boxCollider.Width;

        if (Position.Y < boxCollider.Position.Y) testy = boxCollider.Position.Y;
        else if (Position.Y > boxCollider.Position.Y + boxCollider.Height) testy = boxCollider.Position.Y + boxCollider.Height;

        float distance = Vector2.Distance(Position, new Vector2(testx, testy));

        return distance < Radius;
    }
}
