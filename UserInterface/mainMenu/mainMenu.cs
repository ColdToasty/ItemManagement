using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class mainMenu : Control
{
	TextureButton continueButton;

	//new Game Menu
	TextureButton newGameGo;
	LineEdit newGameName;
	Panel newGameMenu;
	RichTextLabel newGameError;
	GameFiles gameFileExplorer;
	private string regex = "^[a-zA-Z]+$";

	ItemList listOfSaves;


	//load menu
	Panel loadMenu;
	TextureButton loadMenuLoadButton;

	private static string save_directory = "user://saves/";
	Directory fileDirectory = new Directory();

	private string loadFileName = "";
	public override void _Ready()
	{
		continueButton = GetNode<TextureButton>("VBoxContainer/continue");

		//New Game menu
		newGameMenu = GetNode<Panel>("newGameMenu");
		newGameGo = GetNode<TextureButton>("newGameMenu/HBoxContainer/go");
		newGameName = GetNode<LineEdit>("newGameMenu/newSaveName");
		newGameError = GetNode<RichTextLabel>("newGameMenu/error");
		newGameError.SetDeferred("visible", false);
		newGameMenu.SetDeferred("visible", false);
		newGameGo.SetDeferred("disabled", true);

		gameFileExplorer = GetNode<GameFiles>("GameFiles");

		if (gameFileExplorer.continueFileExist)
		{
			continueButton.SetDeferred("disabled", false);
		}
		else
		{
			continueButton.SetDeferred("disabled", true);
		}

		loadMenu = GetNode<Panel>("loadMenu");
		loadMenuLoadButton = GetNode<TextureButton>("loadMenu/HBoxContainer/load");
		loadMenuLoadButton.SetDeferred("disabled", true);
		loadMenu.SetDeferred("visible", false);

		listOfSaves = GetNode<ItemList>("loadMenu/listOfSaves");

		
	}


	//continue
	private void _on_continue_pressed()
	{
		gameFileExplorer.ContinueGame();
	}

	//Show newGameMenu
	private void _on_new_pressed()
	{
		newGameMenu.SetDeferred("visible", true);
	}

	private void _on_newGameGo_pressed()
	{
		string newFileName = newGameName.Text.ToLower();
		gameFileExplorer.OnNewSaveGameData(newFileName);
		gameFileExplorer.LoadFile(newFileName);
	}

	//Go Back to main menu
	private void _on_newGameback_pressed()
	{
		newGameName.Text = "";
		newGameMenu.SetDeferred("visible", false);
	}



	//Checks the string of the newGame input
	//Can only contain alphabetical characters
	private void _on_newSaveName_text_changed(String new_text)
	{
		if(new_text.Length > 0)
		{
			bool checkName = Regex.IsMatch(new_text, regex);
			if (checkName)
			{
				newGameError.SetDeferred("visible", false);
				newGameGo.SetDeferred("disabled", false);
			}
			else
			{
				
				newGameError.SetDeferred("visible", true);
			}
		}
		else
		{
			newGameError.SetDeferred("visible", false);
			newGameGo.SetDeferred("disabled", true);
		}
	}




	//load
	//pop up a menu with all saves
	//each save should be clickable
	//load button, back arrow button, delete button
	private void _on_mainMenuLoad_pressed()
	{
		loadMenu.SetDeferred("visible", true);


		List<string> savedGames = gameFileExplorer.LoadAllFiles();


		fileDirectory.Open(GameFiles.save_directory);
		File save_file = new File();

		foreach (string gameFile in savedGames)
		{
			Error error = save_file.Open(GameFiles.save_directory + gameFile + GameFiles.file_extension, File.ModeFlags.Read);

			if (error == Error.Ok)
			{
				string file_name = GameFiles.save_directory + gameFile + GameFiles.file_extension;
				if (save_file.FileExists(file_name))
				{
					//Adds the save file name to itemList node
					listOfSaves.AddItem(gameFile);
			
				}
				save_file.Close();

			}
		}

	}

	private void _on_file_selected(int index)
	{
		loadFileName = listOfSaves.GetItemText(index);
		loadMenuLoadButton.SetDeferred("disabled", false);

	}



	private void _on_loadMenuBack_pressed()
	{
		loadMenu.SetDeferred("visible", false);
	}

	private void _on_loadMenuLoad_pressed()
	{
		gameFileExplorer.LoadFile(loadFileName);
	}

	private void _on_loadMenuDelete_pressed()
	{
		// Replace with function body.
	}


	//settings
	private void _on_options_pressed()
	{
		// open options 
	}



	//exit
	private void _on_exit_pressed()
	{
		GetTree().Quit();
	}


}














