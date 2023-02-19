using Godot;
using System;
using System.Collections.Generic;

public class PlaceLocation : Node2D
{
	public Sprite item_image;
	public string name;

	private PlaceLocationPlayerDetectionZone zone;
	private CollisionShape2D detectionzone;

	private Player player;

	public override void _Ready()
	{
		item_image = GetNode<Sprite>("Sprite");
		zone = GetNode<PlaceLocationPlayerDetectionZone>("PlaceLocationPlayerDetectionZone");
		detectionzone = zone.GetNode<CollisionShape2D>("CollisionShape2D");
	}

	public void set_texture()
	{
		player = zone.player;
		item_image.Texture = player.item_textures[0];
		player.item_textures.RemoveAt(0);
		detectionzone.Disabled = true;

	}


	public override void _PhysicsProcess(float delta) 
	{
		if (zone.see_player())
		{
			//Set the texture so that item is placed
			set_texture();

		}
	}




}






