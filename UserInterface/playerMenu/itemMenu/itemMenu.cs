using Godot;
using System;
using System.Collections.Generic;

public class itemMenu : Control
{

	int ornamentCost = 3;
	int tinselCost = 5;
	int caneCost = 8;
	int invisibilityCost = 10;


	Label ornamentLabel, ornamentCurrentLabel, tinselLabel, tinselCurrentLabel, caneLabel, caneCurrentLabel, invisibilityLabel, invisibilityCurrentLabel;
	string currentString = "Current: ";

	Stack<string> playerChoice = new Stack<string>();

	TextureButton undoButton, ornamentButton, tinselButton, caneButton, invisibilityButton;

	Godot.Collections.Dictionary playerData = GameFiles.GetFileContents();

	int ornamentCount, tinselCount, caneCount, invisibilityCount, presentsCount;

	bool showItemDescription = false;

	string ornamentDescription = "Roll these bulbs to distract people";
	string tinselDescription = "Throw these to tangle";
	string caneDescription = "Let them taste the Candy Cane!";
	string invisibilityDescription = "Sneaky Santa";

	private enum ITEMHOVER
	{
		NONE,
		ORNAMENT,
		TINSEL,
		CANE,
		INVISIBILITY
	}

	private ITEMHOVER hoveringOer = ITEMHOVER.NONE;

    public override void _Ready()
	{
		//Open save file and get amount of items
		ornamentCount = playerData["ornamentCount"].ToString().ToInt();
		tinselCount= playerData["tinselCount"].ToString().ToInt();
		caneCount = playerData["caneCount"].ToString().ToInt();
		invisibilityCount = playerData["invisibilityCount"].ToString().ToInt();
		presentsCount = playerData["currentPresents"].ToString().ToInt();


		undoButton = GetNode<TextureButton>("VBoxContainer/backUndoNext/undo");
		ornamentButton = GetNode<TextureButton>("VBoxContainer/ornament/HBoxContainer/TextureButton");
		tinselButton = GetNode<TextureButton>("VBoxContainer/tinsel/HBoxContainer/TextureButton");
		caneButton = GetNode<TextureButton>("VBoxContainer/cane/HBoxContainer/TextureButton");
		invisibilityButton = GetNode<TextureButton>("VBoxContainer/invisibility/HBoxContainer/TextureButton");

		ornamentLabel = GetNode<Label>("VBoxContainer/ornament/HBoxContainer/cost/Label");
		ornamentCurrentLabel = GetNode<Label>("VBoxContainer/ornament/HBoxContainer/currentAmount/Label");

		tinselLabel = GetNode<Label>("VBoxContainer/tinsel/HBoxContainer/cost/Label");
		tinselCurrentLabel = GetNode<Label>("VBoxContainer/tinsel/HBoxContainer/currentAmount/Label");

		caneLabel = GetNode<Label>("VBoxContainer/cane/HBoxContainer/cost/Label");
		caneCurrentLabel = GetNode<Label>("VBoxContainer/cane/HBoxContainer/currentAmount/Label");

		invisibilityLabel = GetNode<Label>("VBoxContainer/invisibility/HBoxContainer/cost/Label");
		invisibilityCurrentLabel = GetNode<Label>("VBoxContainer/invisibility/HBoxContainer/currentAmount/Label");

		//Set the labels to reflect the cost of buying 1 item
		ornamentLabel.Text = ornamentCost.ToString();
		tinselLabel.Text = tinselCost.ToString();
		caneLabel.Text = caneCost.ToString();
		invisibilityLabel.Text = invisibilityCost.ToString();



	}

	private void _on_ornamentButton_pressed()
	{
		ornamentCount++;
		playerChoice.Push("ornament");
	}


	private void _on_tinselButton_pressed()
	{ 
		tinselCount++;
		playerChoice.Push("tinsel");
	}


	private void _on_caneButton_pressed()
	{
		caneCount++;
		playerChoice.Push("cane");
	}


	private void _on_invisibilityButton_pressed()
	{
		invisibilityCount++;
		playerChoice.Push("invisibility");
	}

	private void _on_undo_pressed()
	{
		string undoChoice = playerChoice.Pop();

		switch (undoChoice)
		{
			case "ornament":
				ornamentCount--;
				break;

			case "tinsel":
				tinselCount--;
				break;

			case "cane":
				caneCount--;
				break;

			case "invisibility":
				invisibilityCount--;
				break;
		}

	}

	//Popup menu asking if player is sure
	//Hide menu
	//set counts and presents to original values
	
	private void _on_back_pressed()
	{

	}

	//Popup menu asking if player is sure
	//Save counts and presents to save file
	private void _on_next_pressed()
	{
	  
	}




	private void _on_ornamentButton_mouse_entered()
	{
		GD.Print("hovering");
		showItemDescription = !showItemDescription;

    }

	private void _on_ornamentButton_mouse_exited()
	{
		GD.Print("left");
        showItemDescription = !showItemDescription;

    }



	private void _on_tinselButton_mouse_entered()
	{

        showItemDescription = !showItemDescription;

    }


	private void _on_tinselButton_mouse_exited()
	{

        showItemDescription = !showItemDescription;

    }


	private void _on_caneButton_mouse_entered()
	{
        showItemDescription = !showItemDescription;
    }


	private void _on_invisibilitiyButton_mouse_entered()
	{
        showItemDescription = !showItemDescription;
    }


	private void _on_invisibilityButton_mouse_exited()
	{
        showItemDescription = !showItemDescription;
    }




	public override void _Process(float delta)
	{
		if(playerChoice.Count > 0)
		{
			undoButton.SetDeferred("disabled", false);
		}
		else
		{
			undoButton.SetDeferred("disabled", true);
		}

		//Update to display current amount user has
		ornamentCurrentLabel.Text = currentString + ornamentCount.ToString();
		tinselCurrentLabel.Text = currentString + tinselCount.ToString();
		caneCurrentLabel.Text = currentString + caneCount.ToString();
		invisibilityCurrentLabel.Text = currentString + invisibilityCount.ToString();


		if(ornamentCost > presentsCount)
		{
			ornamentButton.SetDeferred("disabled", true);
		}
		else
		{
			ornamentButton.SetDeferred("disabled", false);
		}

		if (tinselCost > presentsCount)
		{
			tinselButton.SetDeferred("disabled", true);
		}
		else
		{
			tinselButton.SetDeferred("disabled", false);
		}


		if (caneCost > presentsCount)
		{
			caneButton.SetDeferred("disabled", true);
		}
		else
		{
			caneButton.SetDeferred("disabled", false);
		}


		if (invisibilityCount > presentsCount)
		{
			invisibilityButton.SetDeferred("disabled", true);
		}
		else
		{
			tinselButton.SetDeferred("disabled", false);
		}
	}


}












