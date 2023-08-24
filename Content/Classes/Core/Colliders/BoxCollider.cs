using Microsoft.Xna.Framework;
using System.Drawing;

namespace Core;

internal class BoxCollider : Collider
{
    public RectangleF CollisionBox => new RectangleF(Position.X, Position.Y, Width, Height);

    public float Width { get; private set; } = 20;
    public float Height {get; private set; } = 20;

    protected override bool TestCircleCollision(CircleCollider circleCollider)
    {
        BoxCollider thisBox = circleCollider.GetAABB();

        if (thisBox.CollisionBox.IntersectsWith(CollisionBox) == false) return false;

        float testx = circleCollider.Position.X;
        float testy = circleCollider.Position.Y;

        if (circleCollider.Position.X < Position.X) testx = Position.X;
        else if (circleCollider.Position.X > Position.X + Width) testx = Position.X + Width;

        if (circleCollider.Position.Y > Position.Y) testy = Position.Y;
        else if (circleCollider.Position.Y < Position.Y + Height) testy = Position.Y + Height;

        float distance = Vector2.Distance(Position, new Vector2(testx, testy));

        return distance < circleCollider.Radius;
    }

    protected override bool TestBoxCollision(BoxCollider boxCollider)
    {
        return CollisionBox.IntersectsWith(boxCollider.CollisionBox);
    }
}
