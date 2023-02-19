using Godot;
using System;


public class RunNoise : Area2D
{
    public Player parent;
    public bool monitor = false;
    public override void _Ready()
    {
        parent = this.GetParent<Player>();
    }


    public override void _PhysicsProcess(float delta)
    {
        if (monitor)
        {
            SetDeferred("monitorable", true);
            SetDeferred("visible", true);
        }
        else
        {
            SetDeferred("monitorable", false);
            SetDeferred("visible", false);
        }
    }

}
