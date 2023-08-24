using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

public class World : Node2D
{

	//What are the constants in every level
	Navigation2D MobNavigation;


	//Items list
	public ItemDatabase db;
	public int placeLocationNumber;
	public List<Texture> item_textures = new List<Texture>();
	private Player player;
	public ObjectSort objectSort;
	public bool pause = false;

	CookieCounter cookieCounter;
	public int cookieScore;

	private bool level_ended = false;

	private gameOverScreen gameOver;
	private int presentsTBD; 
	public override void _Ready()
	{
		db = GetNode<ItemDatabase>("/root/ItemDatabase");
		//Gets the number of placeLocation nodes
		placeLocationNumber = GetTree().CurrentScene.GetNode<YSort>("PlaceLocations").GetChildCount();
        presentsTBD = placeLocationNumber;

        add_textures();
		player = GetTree().CurrentScene.GetNode<Player>("objectSort/Player");
		player.Connect("showGameOverScreen", this, "showGameOverScreen");

        objectSort = GetNode<ObjectSort>("objectSort");
        objectSort.Connect("end_level", this, "endLevel");
		cookieCounter = GetNode<CookieCounter>("/root/CookieCounter");
		//Get gameOverScreen
		//add it but disable visibility
		Godot.Collections.Array presents = GetTree().CurrentScene.GetNode<YSort>("PlaceLocations").GetChildren();
		for(int i  = 0;i<  placeLocationNumber; i++)
		{
			((PlaceLocation)presents[i]).Connect("item_placed", this, "itemPlaced");
		}
    }


	public void itemPlaced()
	{
        presentsTBD--;
		if(presentsTBD == 0)
		{
			//Pop up saying all presents delivered and can leave
			GD.Print("All presents delivered");
		}
	}

	public void showGameOverScreen()
	{
		GD.Print("Game Over");
		//Load an instance of gameOver screen
	}

	public void endLevel()
	{
		//Prevent cookies being added multiple times
		if(!level_ended && presentsTBD < placeLocationNumber) {
            GD.Print("End Level");
            level_ended = true;
			cookieCounter.Save_global_cookies();
			int cookies = cookieCounter.Global_Cookie_Counter;
			int stars = 0;
			//Save the following by sending the values to the saveGame cs file
			//Save cookies
			//Save presents
			//Save time
			//Save presents delivered
			//Save highScore for level
			//saveGame.saveGameData();
            float percentage = (float)(placeLocationNumber-presentsTBD)/ (float)placeLocationNumber * 100;
			if (percentage == 100) {
                //This really is a perfect christmas for these people
                //I hope you enjoyed the cookies because you deserve it
            }
            else if (percentage <100 && percentage > 80)
			{
                //Presents delivered and happy family what else could you ask 
                //You really inspire me to do better 
            }
            else if(percentage < 80 && percentage > 60)
			{
                //Wow great job I can see their joy already rising
				//I hope they enjoy christmas, but do go easy on those cookies or they'll make you sloppy
            }
            else if(percentage < 60 && percentage > 40)
			{
				//I guess this will make them happy
			}
			else if(percentage < 40 && percentage > 20)
			{
				//I mean if you consider this making christmas better then I guess you can do it like this
			}
			else
			{
				//Are you even trying to make christmas enjoyable
			}

        }
		else
		{
			//Popup rudolph critizing player
			GD.Print("So you're going to come into someone's house and eat their cookies and dip?");

		}

	}

	//Loads the textures 
	public void add_textures()
	{
		Random rnd = new Random();
		int item_data_count = db.get_list_length();

		for (int i = 0; i < placeLocationNumber; i++)
		{
			//Randomises the texture the present is assigned
			int rnd_sprite = rnd.Next(item_data_count);
			//Add it to a set list
			item_textures.Add(db.items[rnd_sprite].texture);
		}
	}
	



}
