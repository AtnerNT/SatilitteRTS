using System;
using Microsoft.Xna.Framework;

namespace Core;

internal partial class PhysicsObject
{
    public Collider PhysicsCollider { get; private set; } = null;

    public void AttachCollider(Collider colliderToAttach)
    {
        if (this.PhysicsCollider != null) DetachCurrentCollider();
        
        this.PhysicsCollider = colliderToAttach;
        colliderToAttach.Position = Position;
        OnPositionChangeEvent += this.PhysicsCollider.UpdatePosition;
    }

    public void DetachCurrentCollider()
    {
        OnPositionChangeEvent -= this.PhysicsCollider.UpdatePosition;

        this.PhysicsCollider = null;
    }

    private Vector2 Interpolate(Vector2 position, Vector2 veliocity)
    {
        float speed = veliocity.Length();
        if (speed <= 1)
        {
            if (PhysicsCollider.TestCollisionsAtPos(position + veliocity) == false) return position + veliocity;
            else return new Vector2(MathF.Round(position.X, 0), MathF.Round(position.Y, 0));
        }

        float stepSize = Math.Clamp(speed, 0, INTERPOLATION_STEP_SIZE);

        Vector2 stepVector = Vector2.Normalize(veliocity) * stepSize;

        int checkCount = (int)(speed / stepSize) + 1;
        for (int i = 1; i <= checkCount; i++)
        {
            Vector2 testPos = position + stepVector * i;
            if (PhysicsCollider.TestCollisionsAtPos(testPos) == true)
            {
                return Interpolate(position + stepVector * (i - 1), stepVector / 2);
            }
        }

        return position + veliocity;
    }
}
