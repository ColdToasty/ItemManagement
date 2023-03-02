using Godot;
using System;

public class DetectionZone : Area2D
{
	public Player player;

	[Signal]
	public delegate void rotate_to_location(Vector2 last_position);

	//If player is seen
	//Will return true
	
	public bool can_see_player()
	{
		return player != null;
	}



    private void _on_noise_area_exited(RunNoise area)
	{
		player = null;
	}


	private void _on_noise_area_entered(RunNoise area)
	{
		player = area.GetParent<Player>();
	}



}



