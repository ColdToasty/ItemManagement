using Godot;
using System;
using System.Collections.Generic;

public class PlaceLocation : Node2D
{
	public Sprite item_glow;
	public Sprite item_image;
	public string name;

	private PlaceLocationPlayerDetectionZone zone;
	private CollisionShape2D detectionzone;

	public List<string> glowAmount = new List<string>();

	public AnimationPlayer animationPlayer;

	public Random rnd = new Random();
	public string textureAnimation;
	public bool delivered = false;

	public World level;
    public List<Texture> item_textures = new List<Texture>();

    public override void _Ready()
	{

        item_glow = GetNode<Sprite>("Sprite");
		item_image = GetNode<Sprite>("Sprite2");

		zone = GetNode<PlaceLocationPlayerDetectionZone>("PlaceLocationPlayerDetectionZone");
		detectionzone = zone.GetNode<CollisionShape2D>("CollisionShape2D");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		int count = rnd.Next(1, 2);
		if(count == 1)
        {
			textureAnimation = "shinyGlow1";

		}
        else
        {
			textureAnimation = "shinyGlow2";
		}

        Node root = GetTree().Root;
		string levelName = GetTree().CurrentScene.Name;
        level = root.GetNode<World>(levelName);
		

    }

	public void set_texture()
	{

		detectionzone.Disabled = true;
		delivered = true;
		animationPlayer.Stop();

		//Set the image to the level item_texture
        item_image.Texture = level.item_textures[0];
		//Remove the texture from the list 
        level.item_textures.RemoveAt(0);


    }

    public override void _PhysicsProcess(float delta) 
	{
		if (zone.see_player())
		{
			//Set the texture so that item is placed
			set_texture();
		}
		//set the shinyGlow
        else if(!delivered)
        {
			animationPlayer.Play(textureAnimation);
        }
	}




}






