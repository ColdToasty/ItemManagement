using Godot;
using System;

public class Path2D : Godot.Path2D
{
    PathFollow2D path;
    public override void _Ready()
    {
        path = GetNode<PathFollow2D>("PathFollow2D");
    }

    public override void _PhysicsProcess(float delta)
    {
        //path.Offset = path.Offset + 150 * delta;
    }
}
