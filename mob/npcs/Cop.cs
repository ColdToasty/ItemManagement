using Godot;
using System;

public class Cop : Mob
{
    public Vector2 spawnPosition;
    public Vector2 investigatePosition;
    public Navigation parent;
    public override void _Ready()
    {
        speed = 200;
        base._Ready();
    }

    public override void _PhysicsProcess(float delta)
    {
        
        base._PhysicsProcess(delta);
    }
}
