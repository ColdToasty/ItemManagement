using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

public class levelSelector : Control
{
	//Gets the player data from files
	private Dictionary playerData = GameFiles.GetFileContents();

	Control levelsContainer;
	levelButton currentLevelButton;
	BlackFadeOut fadeOut;
	string currentLevelNumber;
	public override void _Ready()
	{
		fadeOut = GetNode<BlackFadeOut>("BlackFadeOut");
		fadeOut.hide();
		fadeOut.Connect("animation_finished", this, "animationFinished");
		levelsContainer = GetNode<Control>("buttons/levels");


		foreach(levelButton b in levelsContainer.GetChildren())
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
        GetTree().ChangeScene($"res://Levels/PlayableLevels/level{currentLevelNumber}.tscn");
    }

}



