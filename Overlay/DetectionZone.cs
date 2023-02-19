using Godot;
using System;

public class DetectionZone : Area2D
{
	public Player player;


	//If player is seen
	//Will return true
	
	public bool can_see_player()
	{
		return player != null;
	}

    public override void _PhysicsProcess(float delta)
    {
    }

    private void _on_noise_area_exited(RunNoise area)
	{
		player = null;
	}

	//Get the player class
	private void _on_noise_area_entered(RunNoise area)
	{
		player = area.parent;
	}




}



