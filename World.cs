using Godot;
using System;
using System.Collections.Generic;

public class World : Node2D
{

	//What are the constants in every level
	Navigation2D MobNavigation;


	//Items list
	public ItemDatabase db;
	public int place_location_number;
	public List<Texture> item_textures = new List<Texture>();
	private Player player;
	public npcSort npcSort;
	public bool pause = false;

	CookieCounter cookieCounter;
	public int cookieScore;

	private bool level_ended = false;

	private gameOverScreen gameOver;
	public override void _Ready()
	{
		db = GetNode<ItemDatabase>("/root/ItemDatabase");
		//Gets the number of placeLocation nodes
		place_location_number = GetTree().CurrentScene.GetNode<YSort>("PlaceLocations").GetChildCount();
		add_textures();
		player = GetTree().CurrentScene.GetNode<Player>("npcSort/Player");
		player.Connect("showGameOverScreen", this, "showGameOverScreen");

		npcSort = GetNode<npcSort>("npcSort");
		npcSort.Connect("endLevel", this, "endLevel");
		cookieCounter = GetNode<CookieCounter>("/root/CookieCounter");

		//Get gameOverScreen
		//add it but disable visibility
	}




	public void showGameOverScreen()
	{
		GD.Print("Game Over");
		//Load an instance of gameOver screen
	}

	public void endLevel()
	{
		GD.Print("End Level");
		//Prevent cookies being added multiple times
		if(!level_ended) {
			level_ended = true;
			int cookies = cookieCounter.Level_Cookie_Counter;

            //Save the following by sending the values to the saveGame cs file
            //Save cookies
            //Save presents
            //Save time
            //Save presents delivered
            //Save highScore for level
        }

	}

	//Loads the textures 
	public void add_textures()
	{
		Random rnd = new Random();
		int item_data_count = db.get_list_length();

		for (int i = 0; i < place_location_number; i++)
		{
			//Randomises the texture the present is assigned
			int rnd_sprite = rnd.Next(item_data_count);
			//Add it to a set list
			item_textures.Add(db.items[rnd_sprite].texture);
		}

	}
	



}
