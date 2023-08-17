using Godot;
using Godot.Collections;
using System;
using System.Collections;

public class saveGame : Control
{


	private static string save_directory = "user://saves/";
    private static string continueFileName = "continue.json";
    private static string continueFileLocation = save_directory + continueFileName;
    
	//Not implemented
	private bool pause = false;

	//Temp for filenaming
	private LineEdit name;

    //Navigate to saveFiles
    Directory savedGames = new Directory();

	private int numberOfSavedGames =0;
    
	//Continue keys
	public string continueSaveFileName = "";
    public string continueSaveFileDate = "";
    public string continueSaveFileTime = "";

	//Savefile 
    private playerStats playerStats;

	//Looks at playerStats resource
    private Dictionary<string, float> playerData;

    //Nodes in tree
    private Button continueButton;

    [Signal]
	public delegate void noContinue();

    File file = new File();

    //Need to implement way to get current save and save file
    private void _on_Continue_pressed()
	{
		
		GD.Print("Continue Button pressed");
		GD.Print();

		var file = new File();
		string saveFileDir = save_directory + continueSaveFileName;
        Error error = file.Open(continueFileLocation, File.ModeFlags.Read);

		if(error == Error.Ok)
		{
            if (file.FileExists(saveFileDir + ".json"))
            {
                var continueGameData = file.GetAsText();
                JSONParseResult saveData = JSON.Parse(continueGameData);
                Dictionary result = saveData.Result as Dictionary;
				GD.Print(result["name"]);
            }
            else
            {
                file.Close();
                GD.Print("Missing saveFile");
                return;
            }
        }
        else
        {
            file.Close();
			OS.Alert("Missing savefile");
            return;
        }

        file.Close();



    }


	private void _on_Save_pressed()
	{
		if(name.Text.Length < 1)
		{
			GD.Print("Enter a name");
			return;
		}
		//Creates the key, pair values for player
		playerData = new Dictionary<string, float>
		{
			{"health", playerStats.Health },
			{"walkSpeed", playerStats.Speed},
			{"runSpeed", playerStats.SprintSpeed },
			{"playerItemRadius", playerStats.PlayerItemRadius},
			{"playerItemHeight", playerStats.PlayerItemHeight},
			{"runNoiseRadius", playerStats.runNoiseRadius},
			{"runNoiseHeight", playerStats.runNoiseHeight },
			{"positionX", playerStats.positionX},
			{"positionY", playerStats.positionY}
		};


		//Create a new file
		File file = new File();
		//Check if the directory exist
		Directory directory = new Directory();


        if (!directory.DirExists(save_directory))
		{
			//Creates directoy if it does not exist
			directory.MakeDirRecursive(save_directory);
		}

        //Make new folder with name
        Error trySave = file.Open( save_directory + name.Text + ".json", File.ModeFlags.Write);

		if (trySave == Error.Ok)
		{
			//Save our variables to this file
			file.StoreString(JSON.Print(playerData));
		}

		//Gets the latest save file
        Dictionary<string, string> latestSave = new Dictionary<string, string>
        {
            { "name", "try get filename user gives"},
            { "date", Time.GetDateStringFromSystem()},
            { "time", Time.GetTimeStringFromSystem().Replace(':', '-')}
        };

		latestSave["name"] = name.Text;
        Error continueSave = file.Open(save_directory + continueFileName, File.ModeFlags.Write);
        if (continueSave == Error.Ok)
		{
			//Disable the continue button
			continueButton.SetDeferred("disabled", false);
			//Store the new save into continue
            file.StoreString(JSON.Print(latestSave));

			//Update the continueSave variables
			continueSaveFileName = latestSave["name"];
			continueSaveFileDate = latestSave["date"];
			continueSaveFileTime = latestSave["time"];


        }
		file.Close();
	}



	private void _on_Load_pressed()
	{
		//Bring up new window with saved games
		//Allow option to pick a save
		savedGames.Open(save_directory);
		savedGames.ListDirBegin(true, true);
		
		while (true)
		{
			string filename = savedGames.GetNext();
			GD.Print(filename);
			if (filename == "")
			{
				break;
			}
		
		}


	}
	private void updatePlayerData(Dictionary savedPlayerData)
	{
		//Sets the players health
		playerStats.Health = (savedPlayerData["health"].ToString()).ToInt();

	}

	private void _on_increaseHealth_pressed()
	{
		playerStats.Health++;

	}


	private void countNumberOfSaves()
	{
		numberOfSavedGames = 0;
		savedGames.Open(save_directory);
		savedGames.ListDirBegin(true, true);
		//Count number of saves
		while (true)
		{
			string filename = savedGames.GetNext();
			if (filename == "")
			{
				break;
			}
			else
			{
				numberOfSavedGames++;
			}
		}

	}
	//Allow Differnt loads?
	//Allow QuickSave

	public override void _Ready()
	{

		//Loads playerStats resource
		playerStats = ResourceLoader.Load("res://Player/playerStats/playerStats.tres") as playerStats;

		//Get cookie data
		//Get presents data
		name = GetNode<LineEdit>("name/text");
		continueButton = GetNode<Button>("Continue");


		if (file.FileExists(continueFileLocation))
		{
			//Try open file
			Error error = file.Open(continueFileLocation, File.ModeFlags.Read);
			//If file successfully opens
			if (error == Error.Ok)
			{
				//Sets data as a key : value pair in form of dictionary
				var continueGameData = file.GetAsText();


				JSONParseResult playerContinueData = JSON.Parse(continueGameData);
				Dictionary result = playerContinueData.Result as Dictionary;
				continueSaveFileName = result["name"].ToString();
				continueSaveFileDate = result["date"].ToString();
				continueSaveFileTime = result["time"].ToString();

				//Replace cookieData
				//Replace presentsData
			}
			else
			{
				continueButton.SetDeferred("disabled", true);
			}


		}
		else
		{
			continueButton.SetDeferred("disabled", true);
		}
        //Last ready } bracket
    }






























	// EOF }
}