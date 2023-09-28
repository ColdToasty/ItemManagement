using Godot;
using System;
using System.Collections.Generic;

public class itemMenu : Control
{

	int ornamentCost = 3;
	int tinselCost = 7;
	int caneCost = 15;
	int invisibilityCost = 10;


	Label ornamentLabel, ornamentAmount, tinselLabel, tinselAmount, caneLabel, caneAmount, invisibilityLabel, invisibilityAmount, presentsLabel;
	string currentString = "Current: ";

	Stack<string> playerChoice = new Stack<string>();

	TextureButton undoButton, ornamentButton, tinselButton, caneButton, invisibilityButton;

	Godot.Collections.Dictionary playerData = GameFiles.GetFileContents();

	int ornamentCount, tinselCount, caneCount, invisibilityCount, presentsCount;

	bool showItemDescription = false;

	string ornamentDescription = "Roll these at a certain direction and people will investigate";
	string tinselDescription = "These will stop people from moving";
	string caneDescription = "Will knock people out for a certain amount of time";
	string invisibilityDescription = "Turns you invisible so people can't see you";

	string currentDescription = "";

	string saveName = GameFiles.save_name;


	private enum ITEMHOVER
	{
		NONE,
		ORNAMENT,
		TINSEL,
		CANE,
		INVISIBILITY
	}

	private ITEMHOVER hoveringOver = ITEMHOVER.NONE;

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


		presentsLabel = GetNode<Label>("VBoxContainer/title/HBoxContainer/HboxContainer/TextureRect2/Label");

		ornamentLabel = GetNode<Label>("VBoxContainer/ornament/HBoxContainer/cost/Label");
		ornamentAmount = GetNode<Label>("VBoxContainer/ornament/HBoxContainer/amount/Label");

		tinselLabel = GetNode<Label>("VBoxContainer/tinsel/HBoxContainer/cost/Label");
		tinselAmount = GetNode<Label>("VBoxContainer/tinsel/HBoxContainer/amount/Label");

		caneLabel = GetNode<Label>("VBoxContainer/cane/HBoxContainer/cost/Label");
		caneAmount = GetNode<Label>("VBoxContainer/cane/HBoxContainer/amount/Label");

		invisibilityLabel = GetNode<Label>("VBoxContainer/invisibility/HBoxContainer/cost/Label");
		invisibilityAmount = GetNode<Label>("VBoxContainer/invisibility/HBoxContainer/amount/Label");

		//Set the labels to reflect the cost of buying 1 item
		ornamentLabel.Text = ornamentCost.ToString();
		tinselLabel.Text = tinselCost.ToString();
		caneLabel.Text = caneCost.ToString();
		invisibilityLabel.Text = invisibilityCost.ToString();
		
		presentsLabel.Text = presentsCount.ToString();



    }

	private void _on_ornamentButton_pressed()
	{
		ornamentCount++;
		presentsCount = presentsCount - ornamentCost;
		playerChoice.Push("ornament");
	}


	private void _on_tinselButton_pressed()
	{ 
		tinselCount++;
		presentsCount = presentsCount - tinselCost;
		playerChoice.Push("tinsel");
	}


	private void _on_caneButton_pressed()
	{
		caneCount++;
		presentsCount = presentsCount - caneCost;
		playerChoice.Push("cane");
	}


	private void _on_invisibilityButton_pressed()
	{
		invisibilityCount++;
		presentsCount = presentsCount - invisibilityCost;
		playerChoice.Push("invisibility");
	}

	private void _on_undo_pressed()
	{
		string undoChoice = playerChoice.Pop();

		switch (undoChoice)
		{
			case "ornament":
				ornamentCount--;
				presentsCount = presentsCount + ornamentCost;
				break;

			case "tinsel":
				tinselCount--;
				presentsCount = presentsCount + tinselCost;
				break;

			case "cane":
				caneCount--;
				presentsCount = presentsCount + caneCost;
				break;

			case "invisibility":
				invisibilityCount--;
				presentsCount = presentsCount + invisibilityCost;
				break;
		}

	}

	//Popup menu asking if player is sure
	//Hide menu
	//set counts and presents to original values
	
	private void _on_back_pressed()
	{
		presentsCount = playerData["currentPresents"].ToString().ToInt();
		ornamentCount = playerData["ornamentCount"].ToString().ToInt();
		tinselCount = playerData["tinselCount"].ToString().ToInt();
		caneCount = playerData["caneCount"].ToString().ToInt();
		invisibilityCount = playerData["invisibilityCount"].ToString().ToInt();
		this.SetDeferred("visible", false);

	}

	//Popup menu asking if player is sure
	//Save counts and presents to save file
	private void _on_next_pressed()
	{
		Godot.Collections.Dictionary<string, string> saveData = new Godot.Collections.Dictionary<string, string>();
		playerData["ornamentCount"] = ornamentCount.ToString();
		playerData["tinselCount"]= tinselCount.ToString();
		playerData["caneCount"] = caneCount.ToString();
		playerData["invisibilityCount"] = invisibilityCount.ToString();
		playerData["currentPresents"]= presentsCount.ToString();

		foreach(string key in playerData.Keys)
		{
			saveData.Add(key, playerData[key].ToString());
		}

		GameFiles.OnSaveGame(saveData);
		this.SetDeferred("visible", false);

	}




	private void _on_ornamentButton_mouse_entered()
	{
		GD.Print("hovering");
		showItemDescription = !showItemDescription;
		currentDescription = ornamentDescription;
        hoveringOver = ITEMHOVER.ORNAMENT;

	}

	private void _on_ornamentButton_mouse_exited()
	{
		GD.Print("left");
		showItemDescription = !showItemDescription;
		currentDescription = "";
        hoveringOver = ITEMHOVER.NONE;
	}



	private void _on_tinselButton_mouse_entered()
	{

		showItemDescription = !showItemDescription;
        currentDescription = tinselDescription;
        hoveringOver = ITEMHOVER.TINSEL;

	}


	private void _on_tinselButton_mouse_exited()
	{

		showItemDescription = !showItemDescription;
        currentDescription = "";
        hoveringOver = ITEMHOVER.NONE;
	}


	private void _on_caneButton_mouse_entered()
	{
		showItemDescription = !showItemDescription;
        currentDescription = caneDescription;
        hoveringOver = ITEMHOVER.CANE;
	}



    private void _on_caneButton_mouse_exited()
    {
        showItemDescription = !showItemDescription;
        currentDescription = "";
        hoveringOver = ITEMHOVER.NONE;
    }


    private void _on_invisibilitiyButton_mouse_entered()
	{
		showItemDescription = !showItemDescription;
        currentDescription = invisibilityDescription;
        hoveringOver = ITEMHOVER.INVISIBILITY;
	}


	private void _on_invisibilityButton_mouse_exited()
	{
		showItemDescription = !showItemDescription;
        currentDescription = "";
        hoveringOver = ITEMHOVER.NONE;
	}




	public override void _Process(float delta)
	{

		ornamentAmount.Text = ornamentCost.ToString();

		tinselAmount.Text = tinselCount.ToString();

		caneAmount.Text = caneCount.ToString();

        invisibilityAmount.Text = invisibilityCount.ToString();


        if (playerChoice.Count > 0)
		{
			undoButton.SetDeferred("disabled", false);
		}
		else
		{
			undoButton.SetDeferred("disabled", true);
		}

		//Update to display current amount user has

		
		presentsLabel.Text = presentsCount.ToString();

		if (ornamentCost > presentsCount || ornamentCount > 998)
		{
			ornamentButton.SetDeferred("disabled", true);
		}
		else
		{
			ornamentButton.SetDeferred("disabled", false);
		}

		if (tinselCost > presentsCount || tinselCount > 998)
		{
			tinselButton.SetDeferred("disabled", true);
		}
		else
		{
			tinselButton.SetDeferred("disabled", false);
		}


		if (caneCost > presentsCount || caneCount > 998)
		{
			caneButton.SetDeferred("disabled", true);
		}
		else
		{
			caneButton.SetDeferred("disabled", false);
		}


		if (invisibilityCount > presentsCount || invisibilityCount > 998)
		{
			invisibilityButton.SetDeferred("disabled", true);
		}
		else
		{
			tinselButton.SetDeferred("disabled", false);
		}

		//Displays a short description about an item
		if (showItemDescription)
		{
            switch (hoveringOver)
            {
                case ITEMHOVER.ORNAMENT:
                    break;

                case ITEMHOVER.TINSEL:
                    break;

                case ITEMHOVER.CANE:
                    break;

                case ITEMHOVER.INVISIBILITY:
                    break;

                default:
                    break;
            }
        }



	}


}













