using Godot;
using System;

public class viewCone : Area2D
{
	public Player player;

	public Boolean can_see_player()
	{
		return player != null;
	}



	private void _on_ViewCone_area_entered(Player body)
	{
		player = body;
	}


	private void _on_ViewCone_area_exited(Player body)
	{
		player = null;
	}


}