using Godot;
using System;


//Allows interaction between Area2D viewCone
public class PlayerVisible : Area2D
{
    public Player parent;
    public override void _Ready()
    {
        parent = this.GetParent<Player>();
    }

}
