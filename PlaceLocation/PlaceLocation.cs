using Godot;
using System;

public class PlaceLocation : Node2D
{
	public Sprite item_image;
	
	public override void _Ready()
	{
		item_image = GetNode<Sprite>("Sprite");
	}

	public void _on_PlayerDetectionZone_area_entered(Player player)
	{
		GD.Print("he is heree");
	}
}






