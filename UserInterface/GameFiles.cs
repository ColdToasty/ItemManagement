using Godot;
using Godot.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameFiles : Control
{

	//All instances of saveGame will have this directory path
	public static string save_directory = "user://saves/";
	public static string continue_file_name = "continue";
	public static string file_extension = ".json";
	public static string continue_file_location = save_directory + continue_file_name + file_extension;
	public static Dictionary current_file_data = new Dictionary();
	private string levelFormat = "res://Levels/level0.tscn";

	private bool pause = false;



	//Navigate to saveFiles
	Directory saved_games = new Directory();

	private int number_of_saved_games =0;
	
	//Continue keys
	private string continue_save_file_name = "";
	private string continue_save_file_date = "";
	private string continue_save_file_time = "";

	//Savefile 
	private playerStats player_stats;

	//Looks at playerStats resource
	private Godot.Collections.Dictionary<string, string> player_data;

	public bool continueFileExist = false;

	[Signal]
	public delegate void noContinue();

	private static File file = new File();

	private string date = Time.GetDateStringFromSystem();
	private string time = Time.GetTimeStringFromSystem().Replace(':', '-');

	//Need to implement way to get current save and save file
	public void ContinueGame()
	{
		GD.Print("Continue Button pressed");
		var file = new File();

		Error checkFileContinue = file.Open(continue_file_location, File.ModeFlags.Read);

		if(checkFileContinue == Error.Ok)
		{
			
			if (file.FileExists(continue_file_location))
			{
				var continueGameData = file.GetAsText();
				JSONParseResult saveData = JSON.Parse(continueGameData);
				Dictionary result = saveData.Result as Dictionary;
                continue_save_file_name = result["name"].ToString();
                continue_save_file_date = result["date"].ToString();
                continue_save_file_time = result["time"].ToString();
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
		try
		{
            this.LoadFile(continue_save_file_name);
        }
		catch(Exception e)
		{
			//Popup some alert
			GD.Print(e.ToString());
		}

	}

	//Loads the game data from directory based on fileName
	public void LoadFile(string fileName)
	{
		//Get the save file to be opened location
		string loadSaveFileLocation = save_directory + fileName + file_extension;
        if (file.FileExists(loadSaveFileLocation))
        {
            Error checkSaveExist = file.Open(loadSaveFileLocation, File.ModeFlags.ReadWrite);

			if (checkSaveExist == Error.Ok)
				{
						string continueGameData = file.GetAsText();
						JSONParseResult saveData = JSON.Parse(continueGameData);
					
						Dictionary result = saveData.Result as Dictionary;
						current_file_data = result;

						file.StoreString(JSON.Print(result));
						file.Close();

				}


			Error updateContinueFile = file.Open(save_directory + continue_file_name + file_extension, File.ModeFlags.Write);


            //Gets the latest save file
            Godot.Collections.Dictionary<string, string> latestSave = new Godot.Collections.Dictionary<string, string>
                {
                    { "name", fileName },
                    { "date", Time.GetDateStringFromSystem() },
                    { "time", Time.GetTimeStringFromSystem().Replace(':', '-') }
                };

            if (updateContinueFile == Error.Ok)
			{
                CreateContinueFile(latestSave);
            }

        }
		file.Close();
		//Reupdate file variables date and time
        GetTree().ChangeScene($"res://Levels/level{current_file_data["latestLevel"]}.tscn");
    }


	public void CreateContinueFile(Godot.Collections.Dictionary<string, string> latestSave)
	{


        //Store the new save into continue
        file.StoreString(JSON.Print(latestSave));

        //Update the continueSave variables
        continue_save_file_name = latestSave["name"];
        continue_save_file_date = latestSave["date"];
        continue_save_file_time = latestSave["time"];
    }

	public void SaveGameData(string saveName)
	{
		//Creates the key, pair values for player
		player_data = new Godot.Collections.Dictionary<string, string>
		{
			//Date and time
			{ "date", date},
			{ "time", time},

			//Level tracking
			{"latestLevel", player_stats.latestLevel.ToString()},
			{"currentLevel", player_stats.currentLevel.ToString()},

			//Player related stats 
			{"health", player_stats.Health.ToString() },
			{"walkSpeed", player_stats.Speed.ToString()},
			{"runSpeed", player_stats.SprintSpeed.ToString() },
			{"playerItemRadius", player_stats.PlayerItemRadius.ToString()},
			{"playerItemHeight", player_stats.PlayerItemHeight.ToString()},
			{"runNoiseRadius", player_stats.runNoiseRadius.ToString()},
			{"runNoiseHeight", player_stats.runNoiseHeight.ToString() },

			//Player item count
			{"ornamentCount", player_stats.ornamentCount.ToString()},
			{"ornamentMaxDistance", player_stats.ornamentMaxDistance.ToString()},
			{"tinselCount", player_stats.tinselCount.ToString()},
			{"caneCount", player_stats.caneCount.ToString()},
			{"invisibilityCount", player_stats.invisibilityCount.ToString()},
			{"invisibilityTime", player_stats.invisibilityTime.ToString() },
			
			//Stats about presents delivered
			{"totalPresentsDelivered", "0"},
			{"currentPresents", "0"},

			//Stats about cookies
			{"totalCookies", "0"},
			{"currentCookies", "0"},
		};


		if (!saved_games.DirExists(save_directory))
		{
			//Creates directoy if it does not exist
			saved_games.MakeDirRecursive(save_directory);
		}

		//Make new folder with name
		Error trySave = file.Open(save_directory + saveName + file_extension, File.ModeFlags.Write);

		if (trySave == Error.Ok)
		{
			//Save our variables to this file
			file.StoreString(JSON.Print(player_data));
		}

		//Gets the latest save file
		Godot.Collections.Dictionary<string, string> latestSave = new Godot.Collections.Dictionary<string, string>
		{
			{ "name", saveName},
			{ "date", Time.GetDateStringFromSystem()},
			{ "time", Time.GetTimeStringFromSystem().Replace(':', '-')}
		};

		latestSave["name"] = saveName;
		Error continueSave = file.Open(continue_file_location, File.ModeFlags.Write);
		if (continueSave == Error.Ok)
		{
			CreateContinueFile(latestSave);
		}

		file.Close();
	
	}


	//return all file names
	public List<string> LoadAllFiles()
	{
		//Bring up new window with saved games
		//Allow option to pick a save
		saved_games.Open(save_directory);
		saved_games.ListDirBegin(true, true);

		List<string> listOfGames = new List<string>();
		while (true)
		{
			string filename = saved_games.GetNext().Split(".")[0];
			if (filename != "continue")
			{
				listOfGames.Add(filename);
			}

			if (filename == "")
			{
				break;
			}

		
		}
		return listOfGames;


	}

	//Method to update save file after every successful game
	private void UpdatePlayerData(Dictionary savedPlayerData)
	{
		//Sets the players health
		player_stats.Health = (savedPlayerData["health"].ToString()).ToInt();

	}

	//Reload the current save
	//But update date, time
	public void ResetLevel()
	{

	}

	private void CountNumberOfSaves()
	{
		number_of_saved_games = 0;
		saved_games.Open(save_directory);
		saved_games.ListDirBegin(true, true);
		//Count number of saves
		while (true)
		{
			string filename = saved_games.GetNext();
			if (filename == "")
			{
				break;
			}
			else
			{
				number_of_saved_games++;
			}
		}

	}





	public override void _Ready()
	{

		//Loads playerStats resource
		player_stats = ResourceLoader.Load("res://Player/playerStats/playerStats.tres") as playerStats;

		if (file.FileExists(continue_file_location))
		{
			continueFileExist = true;
			//Try open file
			Error error = file.Open(continue_file_location, File.ModeFlags.Read);
			//If file successfully opens
			if (error == Error.Ok)
			{
				//Sets data as a key : value pair in form of dictionary
				var continueGameData = file.GetAsText();

				JSONParseResult playerContinueData = JSON.Parse(continueGameData);
				Dictionary result = playerContinueData.Result as Dictionary;
				continue_save_file_name = result["name"].ToString();
				continue_save_file_date = result["date"].ToString();
				continue_save_file_time = result["time"].ToString();
			}
		}
		else
		{
			continueFileExist =false;

		}



		//Last ready } bracket
	}






























	// EOF }
}
