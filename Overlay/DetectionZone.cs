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


	public void _on_DetectionZone_body_shape_exited(RID body_rid, Player body, int body_shape_index, int local_shape_index)
	{
		player = null;
	}


	public void _on_DetectionZone_body_shape_entered(RID body_rid, Player body, int body_shape_index, int local_shape_index)
	{
		player = body;
	}

}

