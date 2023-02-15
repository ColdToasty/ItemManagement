using Godot;
using System;

public class viewCone : Area2D
{
	public Player player;

	



	public Boolean can_see_player()
    {
		return player != null;
    }

	private void _on_ViewCone_player_shape_entered(RID body_rid, Player body, int body_shape_index, int local_shape_index)
	{
		player = body;

	}




	private void _on_ViewCone_player_shape_exited(RID body_rid, Player body, int body_shape_index, int local_shape_index)
	{
		player = null;
	}

}

