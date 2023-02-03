using Godot;
using System;

public class ItemZone : Area2D
{
	public PickupZone PickupZone;


	public Boolean can_see_player(){
			return PickupZone != null;
		}


	private void _on_ItemZone_area_entered(PickupZone area)
	{
		PickupZone = area;
	}


	private void _on_ItemZone_area_exited(PickupZone area)
	{

		PickupZone = null;
	}

}





