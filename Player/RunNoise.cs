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




}
