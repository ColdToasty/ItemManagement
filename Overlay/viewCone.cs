using Godot;
using System;

public class viewCone : Area2D
{
	public Player player;

	public bool see_player() { return player != null; }


	private void _on_ViewCone_body_shape_exited(RID body_rid, Player body, int body_shape_index, int local_shape_index)
	{
		player = null;
	}


	private void _on_ViewCone_body_shape_entered(RID body_rid, Player body, int body_shape_index, int local_shape_index)
	{
		player = body;
	}

}

