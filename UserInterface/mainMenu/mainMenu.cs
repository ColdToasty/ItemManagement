using Godot;
using System;

public class mainMenu : Control
{
	Button continueButton, newButton, loadButton, optionsButton, exitButton;

	public override void _Ready()
	{
		continueButton = GetNode<Button>("continue");
        newButton = GetNode<Button>("new");
        loadButton = GetNode<Button>("load");
        optionsButton = GetNode<Button>("options");
        exitButton = GetNode<Button>("exit");
		checkContinueFileExist();
    }
	
	//Checks if the continue file exist and is valid
	private void checkContinueFileExist()
	{
        //Check the continue save dir
        bool exist = false;


		if (exist)
		{
			continueButton.SetDeferred("disabled", false);
		}
		else
		{
            continueButton.SetDeferred("disabled", true);
        }
	}

	//continue
	private void _on_continue_pressed()
	{
		//checks if continue file exist
		//if it exist have button clickable
		//should display name, time and date when hovered over

		//On press load the data from continue.name file
	}

	//new
	private void _on_new_pressed()
	{
		//pop up a new menu where a text field is needed for file name
	}

    //load
    //pop up a menu with all saves
	//each save should be clickable
	//load button, back arrow button, delete button
    private void _on_load_pressed()
	{
		//open save dir
		//get get list of all saves
		//display it with its info - name, time, date

		//load button should be disabled when no option is selected
		//When a save is pressed load button should be clickable
	}

    //settings
    private void _on_options_pressed()
    {
        // open options 
    }



    //exit
    private void _on_exit_pressed()
	{
		//Close instance
	}


}











