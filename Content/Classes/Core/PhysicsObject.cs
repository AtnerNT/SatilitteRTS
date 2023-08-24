using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;

namespace Core;

public interface IDrawMe
{
    public void Initialise()
    {
        GameManager.Drawables.Add(this);
    }
}
internal partial class PhysicsObject : IDrawMe
{
    private const float BIG_G = 1f;
    private const int INTERPOLATION_STEP_SIZE = 10;
    public int Mass { get; private set; }
    public Vector2 Position { get; private set; }
    public Vector2 Veliocity { get; private set; }
    public int Rotation { get; private set; }

    public bool hasGravity { get; private set; } = false;
    public int gravityRadius { get; private set; } = 0;

    public bool affectedByGravity { get; private set; } = false;

    private int lastTime = GameManager.ProgramTime;

    public delegate void OnPositionChange(Vector2 position);
    public OnPositionChange OnPositionChangeEvent;

    public static readonly List<PhysicsObject> PhysicsObjects = new List<PhysicsObject>();
    public static readonly List<PhysicsObject> Attractables = new List<PhysicsObject>();
    public static readonly List<PhysicsObject> Attractors = new List<PhysicsObject>();

    public PhysicsObject(int mass, Vector2 initialPosition, Vector2 initialVeliocity, int initialRotation = 0, bool _hasGravity = false, bool _affectedByGravity = true, int _gravityRadius = 0)
    {
        Mass = mass;
        Position = initialPosition;
        Veliocity = initialVeliocity;
        Rotation = initialRotation;
        gravityRadius = gravityRadius;

        lastTime = GameManager.ProgramTime;

        UpdateIfGravityAffected(_affectedByGravity);
        UpdateIfHasGravity(_hasGravity);
        PhysicsObjects.Add(this);

        ((IDrawMe)this).Initialise();
    }

    public void Destroy()
    {
        PhysicsObjects.Remove(this);
        if(Attractables.Contains(this)) Attractables.Remove(this);
        if (Attractors.Contains(this)) Attractors.Remove(this);
    }

    public void UpdateIfGravityAffected(bool newValue)
    {
        if (affectedByGravity == newValue) return;

        affectedByGravity = newValue;
        if(affectedByGravity == true) Attractables.Add(this);
        else Attractables.Remove(this);
    }

    public void UpdateIfHasGravity(bool newValue)
    {
        if (hasGravity == newValue) return;

        hasGravity = newValue;
        if (hasGravity == true) Attractors.Add(this);
        else Attractors.Remove(this);
    }

    public void Pull()
    {
        if (this.hasGravity == false) return;

        foreach(PhysicsObject physicsObject in Attractables)
        {
            if (physicsObject == this) continue;
            if (physicsObject.affectedByGravity == false) continue;

            Vector2 forceToAdd = this.Position - physicsObject.Position;
            float distance = Vector2.Distance(physicsObject.Position, this.Position);

            forceToAdd /= distance;
            forceToAdd *= (this.Mass * physicsObject.Mass) / (distance * distance);
            forceToAdd *= BIG_G;
            physicsObject.AddForce(forceToAdd);
        }

    }

    public void AddForce(Vector2 force)
    {
        Veliocity += force / Mass;
    }

    public virtual void PhysicsUpdate()
    {
        int timeDifference = GameManager.ProgramTime - lastTime;
        lastTime = GameManager.ProgramTime;

        Vector2 veliocityThisFrame = Veliocity * timeDifference;

        if (PhysicsCollider != null) Position = Interpolate(Position, veliocityThisFrame);     
        else Position += veliocityThisFrame;
 
        OnPositionChangeEvent?.Invoke(Position);
    }

}
