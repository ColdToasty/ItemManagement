using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;




//The upgradeMenu should only exist when
//A player has loaded the game and is choosing their level OR
//After a successfully leaves a level
public class UpgradeMenu : Control
{
	private string fileSaveName = GameFiles.save_name;

	private Godot.Collections.Dictionary playerData;

	private Label hCost, wsCost, rsCost, rCost, nCost, cookieCounter;


	private int health, walkSpeed, runSpeed, reachX, reachY, noise, cookies;

	private int[] healthIncreaseValue = new int[] {1, 2, 3, 3};
	private int[] walkSpeedIncreaseValue = new int[] { 20, 30, 50, 70};
	private int[] runSpeedIncreaseValue = new int[] { 10, 20, 40, 60 };

	private int[] reachXIncreaseValue = new int[] { 10, 20, 30, 40};
    private int[] reachYIncreaseValue = new int[] { 10, 15, 20, 25 };
    private int[] noiseDecreaseValue = new int[] {-15, -20, -30, -40 };

	private Stack<Tuple<string, int>> playerChoice = new Stack<Tuple<string, int>>();


	private int healthLevel, speedLevel, reachLevel, noiseLevel;


	private TextureButton undoButton, healthButton, speedButton, reachButton, noiseButton;
	public override void _Ready()
	{
		playerData = GameFiles.GetFileContents();

		undoButton = GetNode<TextureButton>("VBoxContainer/backUndoNext/undo");

        hCost = GetNode<Label>("VBoxContainer/health/HBoxContainer/Label");
		wsCost = GetNode<Label>("VBoxContainer/speed/HBoxContainer/Label");
		rsCost = GetNode<Label>("VBoxContainer/speed/HBoxContainer/Label2");
		rCost = GetNode<Label>("VBoxContainer/reach/HBoxContainer/Label");
		nCost = GetNode<Label>("VBoxContainer/noise/HBoxContainer/Label");
		cookieCounter = GetNode<Label>("VBoxContainer/labels/Label");


		healthButton = GetNode<TextureButton>("VBoxContainer/health/HBoxContainer/upgrade");
        speedButton = GetNode<TextureButton>("VBoxContainer/speed/HBoxContainer/upgrade");
        reachButton = GetNode<TextureButton>("VBoxContainer/reach/HBoxContainer/upgrade");
        noiseButton = GetNode<TextureButton>("VBoxContainer/noise/HBoxContainer/upgrade");


        health = playerData["health"].ToString().ToInt();
		walkSpeed = playerData["walkSpeed"].ToString().ToInt();
		runSpeed = playerData["runSpeed"].ToString().ToInt();
		reachX = playerData["reachX"].ToString().ToInt();
        reachY = playerData["reachX"].ToString().ToInt();
        noise = playerData["noiseRadius"].ToString().ToInt();
		cookies = playerData["currentCookies"].ToString().ToInt();

		healthLevel = playerData["healthLevel"].ToString().ToInt();
        speedLevel = playerData["speedLevel"].ToString().ToInt();
        reachLevel = playerData["reachLevel"].ToString().ToInt();
        noiseLevel = playerData["noiseLevel"].ToString().ToInt();
    }


	private void _on_healthUpgrade_pressed()
	{
		if(healthLevel < 4)
		{
            health += healthIncreaseValue[healthLevel];
            healthLevel++;

            Tuple<string, int> choice = new Tuple<string, int>("health", health);
            playerChoice.Push(choice);
        }
        else
        {
            healthButton.SetDeferred("disabled", true);
        }

    }


	private void _on_speedUpgrade_pressed()
	{
		if(speedLevel < 4) { 
        walkSpeed += walkSpeedIncreaseValue[speedLevel];
		runSpeed += runSpeedIncreaseValue[speedLevel];
		speedLevel++;

		Tuple<string, int> choice = new Tuple<string, int>("speed", walkSpeed);
		playerChoice.Push(choice);
        }
        else
        {
            speedButton.SetDeferred("disabled", true);
        }
    }


	private void _on_reachUpgrade_pressed()
	{
		if (reachLevel < 4)
		{
			reachX += reachXIncreaseValue[reachLevel];
			//reachY += reachYIncreaseValue[reachLevel];
			reachLevel++;

			Tuple<string, int> choice = new Tuple<string, int>("reach", reachX);
			playerChoice.Push(choice);
		}
		else
		{
			reachButton.SetDeferred("disabled", true);
		}
    }


	private void _on_noiseUpgrade_pressed()
	{
		if (noiseLevel < 4)
		{
			noise += noiseDecreaseValue[noiseLevel];
			noiseLevel++;
			Tuple<string, int> choice = new Tuple<string, int>("noise", noise);
			playerChoice.Push(choice);
		}
        else
        {
            noiseButton.SetDeferred("disabled", true);
        }
    }


	private void _on_back_pressed()
	{
		if (playerChoice.Count > 0)
		{
			GD.Print("confirmationMenu");
		}
		else
		{
            this.SetDeferred("visible", false);
        }
	}



    private void _on_undo_pressed()
    {
		Tuple<string, int> undoUpgrade = playerChoice.Pop();
		
		switch (undoUpgrade.Item1)
		{
			case "health":
                healthLevel--;
                health = undoUpgrade.Item2 - healthIncreaseValue[healthLevel];
				if(healthLevel < 4)
				{
                   healthButton.SetDeferred("disabled", false);
                }
                break;

			case "speed":
                speedLevel--;
                walkSpeed = undoUpgrade.Item2 - walkSpeedIncreaseValue[speedLevel];
				runSpeed = undoUpgrade.Item2 - runSpeedIncreaseValue[speedLevel];
                if (speedLevel < 4)
                {
                    speedButton.SetDeferred("disabled", false);
                }
                break;

			case "reach":
                reachLevel--;
                reachX = undoUpgrade.Item2 - reachXIncreaseValue[reachLevel];
                //reachY = undoUpgrade.Item2 - reachYIncreaseValue[reachLevel];
                if (reachLevel < 4)
                {
                    reachButton.SetDeferred("disabled", false);
                }
                break;

			case "noise":
                noiseLevel--;
                noise = undoUpgrade.Item2 - noiseDecreaseValue[noiseLevel];
                if (noiseLevel < 4)
                {
                    noiseButton.SetDeferred("disabled", false);
                }
                break;

		}
    }



    private void _on_next_pressed()
	{
		playerData["health"] = health;
		playerData["walkSpeed"] = walkSpeed;
		playerData["runSpeed"] = runSpeed;
		playerData["reachX"] = reachX;
        playerData["reachY"] = reachY;
        playerData["noiseRadius"] = noise;
		playerData["currentCookies"] = cookies;
		
		Godot.Collections.Dictionary<string, string> saveData = new Godot.Collections.Dictionary<string, string>();

		foreach (string key in playerData.Keys)
		{
			saveData.Add(key, playerData[key].ToString());
		}

		GameFiles.OnSaveGame(saveData);
		playerChoice.Clear();
		
	}

	public override void _PhysicsProcess(float delta)
	{
		hCost.Text = health.ToString();
		wsCost.Text = walkSpeed.ToString();
		rsCost.Text = runSpeed.ToString();
		rCost.Text = reachX.ToString() + " " + reachY.ToString();
		nCost.Text = noise.ToString();
		cookieCounter.Text = cookies.ToString();


		if(playerChoice.Count <= 0)
		{
			undoButton.SetDeferred("disabled", true);
		}
		else
		{
            undoButton.SetDeferred("disabled", false);
        }



	}

}






