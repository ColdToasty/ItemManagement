using Godot;
using System;

public class PlaceLocationPlayerDetectionZone : Area2D
{
	public Player player;


	//If player is seen
	//Will return true

	public bool see_player()
	{
		return player != null;
	}


	private void _on_playerReach_area_entered(PlayerReach area)
	{
		player = area.parent;
	}


	private void _on_playerReach_area_exited(PlayerReach area)
	{
		player = null;
	}
}









