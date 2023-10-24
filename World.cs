using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

public class World : Node2D
{
	[Export]
	private int level;

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

	private gameOverScreen gameOverScreen;
	private int presentsTBD;

	
	private int ornamentCount, tinselCount, caneCount, invisibilityCount;

	BlackFadeOut blackFadeOutAnimation;
	PackedScene blackFadeOutScene;

	public override void _Ready()
	{
		db = GetNode<ItemDatabase>("/root/ItemDatabase");
		//Gets the number of placeLocation nodes
		placeLocationNumber = GetTree().CurrentScene.GetNode<YSort>("PlaceLocations").GetChildCount();
		presentsTBD = placeLocationNumber;

        AddTextures();
		player = GetTree().CurrentScene.GetNode<Player>("objectSort/Player");

		objectSort = GetNode<ObjectSort>("objectSort");
		objectSort.Connect("end_level", this, "endLevel");
		objectSort.Connect("game_over", this, "gameOver");
		cookieCounter = GetNode<CookieCounter>("/root/CookieCounter");
		//Get gameOverScreen
		//add it but disable visibility
		Godot.Collections.Array presents = GetTree().CurrentScene.GetNode<YSort>("PlaceLocations").GetChildren();
		for(int i  = 0;i<  placeLocationNumber; i++)
		{
			((PlaceLocation)presents[i]).Connect("item_placed", this, "itemPlaced");
		}

        GD.Print(level);
        level = Name[Name.Length-1].ToString().ToInt();
		
        blackFadeOutScene = GD.Load<PackedScene>("res://UserInterface/menuAnimations/BlackFadeOut.tscn");
		blackFadeOutAnimation = (BlackFadeOut)blackFadeOutScene.Instance();

		this.AddChild(blackFadeOutAnimation);


        blackFadeOutAnimation.Connect("fade_finished", this, "animationFinished");
        blackFadeOutAnimation.show();
		blackFadeOutAnimation.playFadeIn();

    }

	public void animationFinished(string name)
	{
		blackFadeOutAnimation.QueueFree();
		if (level_ended)
		{
            GetTree().ChangeScene($"res://Levels/BaseLevel/levelSelector.tscn");
        }
    }


    //Shows game over screen

	public void gameOver()
	{

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


	public void endLevel()
	{
		//Prevent cookies being added multiple times
		if(!level_ended && presentsTBD < placeLocationNumber) {
			GD.Print("End Level");
			level_ended = true;


			float stars = 0;
            Godot.Collections.Dictionary<string, string> saveData = CreateSaveDictionary(GameFiles.save_name);
			GameFiles.OnSaveGame(saveData);
			GD.Print("saved");
			
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
			//player.tryToLeaveDialogShow();
			//GD.Print("So you're going to come into someone's house and eat their cookies and dip?");

		}

	}


	private Godot.Collections.Dictionary<string, string> CreateSaveDictionary(string saveName)
	{
        UpdateCountOfItems();
		Godot.Collections.Dictionary updatedSaveData = GameFiles.GetFileContents();
		updatedSaveData["date"] = Time.GetDateStringFromSystem();
        updatedSaveData["time"] = Time.GetTimeStringFromSystem().Replace(':', '-');

		updatedSaveData["totalPresentsDelivered"] = updatedSaveData["totalPresentsDelivered"].ToString().ToInt() + 2;

		GD.Print(updatedSaveData["currentLevel"]);
		if (updatedSaveData["latestLevel"].ToString().ToInt() <= level)
		{
			updatedSaveData["latestLevel"] = level;
			updatedSaveData["currentLevel"] = level + 1;
        }
        GD.Print(updatedSaveData["currentLevel"]);
        updatedSaveData["ornamentCount"] = this.ornamentCount;
        updatedSaveData["tinselCount"] = this.tinselCount;
        updatedSaveData["caneCount"] = this.caneCount;
        updatedSaveData["invisibilityCount"] = this.invisibilityCount;


        //Increments totalPresentsDelivered from how many presents was delivered from level
        int presentsDelivered = placeLocationNumber - presentsTBD;
		updatedSaveData["totalPresentsDelivered"] = updatedSaveData["totalPresentsDelivered"].ToString().ToInt() + presentsDelivered;

		//Updates currentPresents
		updatedSaveData["currentPresents"] = updatedSaveData["currentPresents"].ToString().ToInt() + presentsDelivered;


        //Get cookies collected from level
        int cookiesCollectedForThisLevel = cookieCounter.Level_Cookie_Counter;
        updatedSaveData["totalCookies"] = updatedSaveData["totalCookies"].ToString().ToInt() + cookiesCollectedForThisLevel;

        updatedSaveData["currentCookies"] = updatedSaveData["currentCookies"].ToString().ToInt() + cookiesCollectedForThisLevel;


		
        Godot.Collections.Dictionary<string, string> returnDictionary = new Godot.Collections.Dictionary<string, string>();

		foreach(string key in updatedSaveData.Keys)
		{
			returnDictionary.Add(key, updatedSaveData[key].ToString());
		}

		return returnDictionary;
	}


	//updates ornamentCount, tinselCount, caneCount and invisibilityCount variables
	//From the players itemSelect
	private void UpdateCountOfItems()
	{
		this.ornamentCount = player.currentItemDisplay.OrnamentCount;
		this.tinselCount = player.currentItemDisplay.TinselCount;
		this.caneCount = player.currentItemDisplay.CaneCount;
		this.invisibilityCount = player.currentItemDisplay.InvisibilityCount;
	}



	//Loads the textures 
	public void AddTextures()
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



