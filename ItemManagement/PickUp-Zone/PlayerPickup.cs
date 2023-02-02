using Godot;
using System;


/*

*/



public class PlayerPickup : Area2D
{

//When item enters Add to inventory
//

private void _on_PlayerPickup_area_entered(object area)
{
	GD.Print("yes");
}

}



