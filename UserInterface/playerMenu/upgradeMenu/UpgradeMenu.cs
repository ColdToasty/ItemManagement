using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Xml.Linq;



/* 
 Still need to connect cookie cost to upgrade buttons
	if not enough cookies, disable buttons
	else decrease cookie amount
 */

public class UpgradeMenu : Control
{
	private string fileSaveName = GameFiles.save_name;

	private Godot.Collections.Dictionary playerData;

	private Label hCost, wsCost, rCost, nCost, cookieCounter;

	private int health, walkSpeed, runSpeed, reachX, reachY, noise, cookies;

	private int[] healthIncreaseValue = new int[] {1, 2, 3, 3};
	private int[] walkSpeedIncreaseValue = new int[] { 20, 30, 50, 70};
	private int[] runSpeedIncreaseValue = new int[] { 10, 20, 40, 60 };

	private int[] reachXIncreaseValue = new int[] { 10, 20, 30, 40};
	private int[] reachYIncreaseValue = new int[] { 10, 15, 20, 25 };
	private int[] noiseDecreaseValue = new int[] {-15, -20, -30, -40 };


	private int[] healthCostAmount = new int[] {3, 5, 7 ,10};
    private int[] speedCostAmount = new int[] { 15, 20, 25, 35 };
    private int[] reachCostAmount = new int[] { 10, 15, 20, 25 };
    private int[] noiseCostAmount = new int[] { 15, 25, 35, 45 };

    private Stack<Tuple<string, int>> playerChoice = new Stack<Tuple<string, int>>();


	private int healthLevel, speedLevel, reachLevel, noiseLevel;


	private TextureButton undoButton, healthButton, speedButton, reachButton, noiseButton;


	Control confirmationMenu;

	HBoxContainer healthProgressBar, speedProgressBar, reachProgressBar, noiseProgressBar;

    public override void _Ready()
	{
		playerData = GameFiles.GetFileContents();

		undoButton = GetNode<TextureButton>("VBoxContainer/backUndoNext/undo");

		hCost = GetNode<Label>("VBoxContainer/health/HBoxContainer/progressBar/cost/Label");
		wsCost = GetNode<Label>("VBoxContainer/speed/HBoxContainer/progressBar/cost/Label");
        rCost = GetNode<Label>("VBoxContainer/reach/HBoxContainer/progressBar/cost/Label");
        nCost = GetNode<Label>("VBoxContainer/noise/HBoxContainer/progressBar/cost/Label");

        cookieCounter = GetNode<Label>("VBoxContainer/title/HBoxContainer/HboxContainer/TextureRect2/Label");


		healthButton = GetNode<TextureButton>("VBoxContainer/health/HBoxContainer/upgrade");
		speedButton = GetNode<TextureButton>("VBoxContainer/speed/HBoxContainer/upgrade");
		reachButton = GetNode<TextureButton>("VBoxContainer/reach/HBoxContainer/upgrade");
		noiseButton = GetNode<TextureButton>("VBoxContainer/noise/HBoxContainer/upgrade");


        healthProgressBar = GetNode<HBoxContainer>("VBoxContainer/health/HBoxContainer/progressBar/HBoxContainer");
        speedProgressBar = GetNode<HBoxContainer>("VBoxContainer/speed/HBoxContainer/progressBar/HBoxContainer");
        reachProgressBar = GetNode<HBoxContainer>("VBoxContainer/reach/HBoxContainer/progressBar/HBoxContainer");
        noiseProgressBar = GetNode<HBoxContainer>("VBoxContainer/noise/HBoxContainer/progressBar/HBoxContainer");


        defaultPlayerChoices();

		setCostBars();

        confirmationMenu = GetNode<Control>("confirmation");
		confirmationMenu.SetDeferred("visible", false);
	}


	private void _on_healthUpgrade_pressed()
	{
		if(healthLevel < 4)
		{
			health += healthIncreaseValue[healthLevel];
			healthLevel++;
            setButton("health");
            Tuple<string, int> choice = new Tuple<string, int>("health", health);
			playerChoice.Push(choice);
            setCostBars();
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
        setButton("speed");
        Tuple<string, int> choice = new Tuple<string, int>("speed", walkSpeed);
		playerChoice.Push(choice);
            setCostBars();
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
            setButton("reach");
            Tuple<string, int> choice = new Tuple<string, int>("reach", reachX);
			playerChoice.Push(choice);
            setCostBars();
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
            setButton("noise");
            Tuple<string, int> choice = new Tuple<string, int>("noise", noise);
			playerChoice.Push(choice);
			setCostBars();

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
			confirmationMenu.SetDeferred("visible", true);
		}
		else
		{
			this.SetDeferred("visible", false);
		}
		setCostBars();
	}



	private void _on_undo_pressed()
	{
		Tuple<string, int> undoUpgrade = playerChoice.Pop();
		
		switch (undoUpgrade.Item1)
		{
			case "health":
				healthLevel--;
				health = undoUpgrade.Item2 - healthIncreaseValue[healthLevel];
				setButton("health");

                if (healthLevel < 4)
				{
				   healthButton.SetDeferred("disabled", false);
				}
				break;

			case "speed":
				speedLevel--;
				walkSpeed = undoUpgrade.Item2 - walkSpeedIncreaseValue[speedLevel];
				runSpeed = undoUpgrade.Item2 - runSpeedIncreaseValue[speedLevel];
                setButton("speed");
                if (speedLevel < 4)
				{
					speedButton.SetDeferred("disabled", false);
				}
				break;

			case "reach":
				reachLevel--;
				reachX = undoUpgrade.Item2 - reachXIncreaseValue[reachLevel];
                //reachY = undoUpgrade.Item2 - reachYIncreaseValue[reachLevel];
                setButton("reach");
                if (reachLevel < 4)
				{
					reachButton.SetDeferred("disabled", false);
				}
				break;

			case "noise":
				noiseLevel--;
				noise = undoUpgrade.Item2 - noiseDecreaseValue[noiseLevel];
                setButton("noise");
                if (noiseLevel < 4)
				{
					noiseButton.SetDeferred("disabled", false);
				}
				break;

		}
		setCostBars();

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


	private void defaultPlayerChoices()
	{
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

		setButton("");

        if (playerChoice.Count > 0) {

            if (healthLevel < 4)
            {
                healthButton.SetDeferred("disabled", false);
            }
			if (speedLevel < 4 )
            {
                speedButton.SetDeferred("disabled", false);
            }
            if (reachLevel < 4)
            {
                reachButton.SetDeferred("disabled", false);
            }
            if (noiseLevel < 4)
            {
                noiseButton.SetDeferred("disabled", false);
            }

        }
        playerChoice.Clear();
    }


	private void setCostBars()
	{
		cookieCounter.Text = cookies.ToString();
		if(healthLevel < 4) {
            hCost.Text = healthCostAmount[healthLevel].ToString();
        }
		else
		{
			hCost.Text = "";

        }

		if(speedLevel < 4) { 
			wsCost.Text = speedCostAmount[speedLevel].ToString(); 
		}
        else
        {
            wsCost.Text = "";

        }

        if (reachLevel < 4) { 
			rCost.Text = reachCostAmount[reachLevel].ToString();
        }
        else
        {
            rCost.Text = "";

        }

        if (noiseLevel < 4) { 
			nCost.Text = noiseCostAmount[noiseLevel].ToString();
		}
        else
        {
            nCost.Text = "";

        }



    }


	//Sets the progress bars
	//can set specific progressBar or all
	private void setButton(string name)
	{
        switch (name)
		{
			case "health":
				Godot.Collections.Array healthBars = healthProgressBar.GetChildren();
                setProgressBarsFrames(healthBars, healthLevel);
				break;

			case "speed":
                Godot.Collections.Array speedBars = speedProgressBar.GetChildren();
                setProgressBarsFrames(speedBars, speedLevel);
                break;

			case "reach":
                Godot.Collections.Array reachBars = reachProgressBar.GetChildren();
                setProgressBarsFrames(reachBars, reachLevel);
                break;

			case "noise":
                Godot.Collections.Array noiseBars = noiseProgressBar.GetChildren();
                setProgressBarsFrames(noiseBars, noiseLevel);
                break;

			default:
                setProgressBarsFrames(healthProgressBar.GetChildren(), healthLevel);
                setProgressBarsFrames(speedProgressBar.GetChildren(), speedLevel);
                setProgressBarsFrames(reachProgressBar.GetChildren(), speedLevel);
                setProgressBarsFrames(noiseProgressBar.GetChildren(), speedLevel);
                break;
		}
	}



	private void setProgressBarsFrames(Godot.Collections.Array bars, int level)
	{
        foreach (Sprite bar in bars)
        {
			int progressLevel = getProgressLevel(bar);
            if (progressLevel <= level)
            {
                bar.Frame = 0;
            }
            else
            {
                bar.Frame = 2;
            }

        }
    }





    private int getProgressLevel(Sprite sprite)
	{
        string pattern = @"\d+"; // This regex pattern matches one or more digits.

        string name = sprite.Name.ToString();
        MatchCollection matches = Regex.Matches(name, pattern);
		if(matches.Count > 0 )
		{
            return matches[0].ToString().ToInt();
        }
		return 0;


	}

	private void _on_no_pressed()
	{
		confirmationMenu.SetDeferred("visible", false);
		
	}


	private void _on_yes_pressed()
	{
        confirmationMenu.SetDeferred("visible", false);
		defaultPlayerChoices();
    }



}

