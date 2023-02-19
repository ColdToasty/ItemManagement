using Godot;
using System;

public class viewCone : Area2D
{
	public Player player;

	public Boolean can_see_player()
	{
		return player != null;
	}



private void _on_Player_area_entered(PlayerVisible area)
{
		player = area.parent;
		GD.Print("playet");
	}


private void _on_Player_area_exited(PlayerVisible area)
{
		player = null;
	}
}