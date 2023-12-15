using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class levelSelector : Control
{
	//Gets the player data from files
	private Godot.Collections.Dictionary playerData = GameFiles.GetFileContents() ;

	Control levelsContainer;
	levelButton currentLevelButton;
	BlackFadeOut fadeOut;
	string currentLevelNumber;


	itemMenu itemMenuUI;
	UpgradeMenu upgradeMenu;

	public override void _Ready()
	{
		fadeOut = GetNode<BlackFadeOut>("BlackFadeOut");
		fadeOut.hide();
		fadeOut.Connect("fade_finished", this, "animationFinished");
		levelsContainer = GetNode<Control>("buttons/levels");

		itemMenuUI = GetNode<itemMenu>("itemMenu");
		upgradeMenu = GetNode<UpgradeMenu>("UpgradeMenu");

		itemMenuUI.SetDeferred("visible", false);
		upgradeMenu.SetDeferred("visible", false);
		foreach (levelButton b in levelsContainer.GetChildren())
		{
			b.Connect("level_selected", this, "levelSelected");

			int number = b.setLabelText(b.Name);
			
			//Set currentLevel to blue icon texture
			if(b.levelNumber == playerData["currentLevel"].ToString())
			{
				b.changeToCurrentTexture();
				currentLevelButton = b;
				currentLevelNumber = b.levelNumber;

			}
			else if (b.levelNumber.ToInt() <= playerData["latestLevel"].ToString().ToInt())
			{
				b.changeToCompletedTexture();
			}

			else
			{
				b.changeToNotReachedTexture();
			}

		}

	}

	public void levelSelected(string level)
	{
		foreach (levelButton b in levelsContainer.GetChildren())
		{
			if (b.levelNumber == level )
			{
				b.changeToCurrentTexture();
				currentLevelButton = b;
				currentLevelNumber = b.levelNumber;

			}
			else if (b.levelNumber.ToInt() <= playerData["latestLevel"].ToString().ToInt())
			{
				b.changeToCompletedTexture();
			}
			else
			{
				b.changeToNotReachedTexture();
			}
		}

	}


	private void _on_playLevelButton_pressed()
	{
        fadeOut.show();
		fadeOut.playFadeOut();
	}


	public void animationFinished(string anim_name)
	{
		GD.Print(currentLevelNumber);
		GetTree().ChangeScene($"res://Levels/PlayableLevels/level{currentLevelNumber}.tscn");
	}


	private void _on_showItemMenu_pressed()
	{
		itemMenuUI.SetDeferred("visible", true);
	}


	private void _on_showUpgradeMenu_pressed()
	{
		upgradeMenu.SetDeferred("visible", true);
	}

}




