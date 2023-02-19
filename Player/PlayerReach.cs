using Godot;
using System;


//Area2D which allows interaction with object classes:
   // Cookies
   // PlaceLocation
public class PlayerReach : Area2D
{
    public Player parent;
    public override void _Ready()
    {
        parent = this.GetParent<Player>();
    }


}
