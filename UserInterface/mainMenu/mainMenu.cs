using Godot;
using System;
using System.Text.RegularExpressions;

public class mainMenu : Control
{
	Button continueButton;

	//new Game Menu
	Button newGameGo;
	LineEdit newGameName;
	Panel newGameMenu;
	RichTextLabel newGameError;
	GameFiles gameFileExplorer;
	private string regex = "^[a-zA-Z]+$";

	//load menu
	Panel loadMenu;
	Button loadMenuLoadButton;
	public override void _Ready()
	{
		continueButton = GetNode<Button>("continue");

		//New Game menu
		newGameMenu = GetNode<Panel>("newGameMenu");
		newGameGo = GetNode<Button>("newGameMenu/go");
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
        loadMenuLoadButton = GetNode<Button>("loadMenu/load");
        loadMenuLoadButton.SetDeferred("disabled", true);
        loadMenu.SetDeferred("visible", false);


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

	private void _on_NewGameMenuGo_pressed()
	{
		string newFileName = newGameName.Text.ToLower();
		gameFileExplorer.SaveGameData(newFileName);
		//call gameFiles load method
		//GetTree().ChangeScene("res://Levels/level1.tscn");
	}

	//Go Back to main menu
	private void _on_NewGameMenuBack_pressed()
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
    private void _on_load_pressed()
	{
        loadMenu.SetDeferred("visible", true);
        //open save dir
        //get get list of all saves
        //display it with its info - name, time, date

        //load button should be disabled when no option is selected
        //When a save is pressed load button should be clickable
    }



    private void _on_loadGameLoad_pressed()
    {
        // Replace with function body.
    }


    private void _on_loadGameBack_pressed()
    {
        loadMenu.SetDeferred("visible", false);
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








