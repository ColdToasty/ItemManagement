using Godot;
using System;



/*
Detects if item is in player vincinity
*/


public class PickupZone : Area2D
{

	public override void _Ready()
	{

	}

    //Looks for item object to enter area
    private void _on_Area2D_area_entered(object area)
    {
        // Replace with function body.
    }

}


