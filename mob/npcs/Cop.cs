using Godot;
using System;

public class Cop : Mob
{
    public Vector2 spawnPosition;
    public Vector2 investigatePosition;
    public Navigation parent;

    Area2D hitPlayerArea;
    CollisionShape2D hitPlayerBox;
    public override void _Ready()
    {
        speed = 200;
        base._Ready();
        hitPlayerArea = GetNode<Area2D>("hitPlayerBox");
        hitPlayerBox = GetNode<CollisionShape2D>("hitPlayerBox/hitPlayer");
        hitPlayerBox.SetDeferred("disabled", false);
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    }
}
