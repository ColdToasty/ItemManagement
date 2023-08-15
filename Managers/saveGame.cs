using Godot;
using Godot.Collections;
using System;
using System.Collections;

public class saveGame : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.

	private static string save_directory = "user://saves/";
	private static string save_path = save_directory + "saveGame.save";


    private void _on_Save_pressed()
    {
        //Load this from player Resource
        Dictionary<string, int> playerData = new Dictionary<string, int>
        {
            { "health", 5 },
            { "walkSpeed", 5 },
            { "runSpeed", 5 },
            { "reach", 1 },
            { "runNoise", 1 }
        };
        //Create a new file
        var file = new File();

        //Check if the directory exist
        var directory = new Directory();
        if (!directory.DirExists(save_directory))
        {
            //Creates directoy if it exist
            directory.MakeDirRecursive(save_directory);
        }

        //Open the save_path file and save to it 
        file.Open(save_path, File.ModeFlags.Write);

        //Save our variables to this file
        file.StoreVar(playerData);


        file.Close();
    }


    private void _on_Load_pressed()
    {
        var file = new File();
        if (file.FileExists(save_path))
        {
            Error error = file.Open(save_path, File.ModeFlags.Read);
            if (error == Error.Ok)
            {
                //Sets data as a key : value pair in form of dictionary
                var playerDataVariant = file.GetVar() as Dictionary;
                file.Close();
            }
        }
    }

	public override void _Ready()
	{
		//Get player data
		//Get cookie data
		//Get presents data
	}


}



