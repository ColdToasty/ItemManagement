using Godot;
using System;

public class DetectionZone : Area2D
{
	public Player player;


	//If player is seen
	//Will return true
	
	public bool see_player()
	{
		return player != null;
	}

	




}

