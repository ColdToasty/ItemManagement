using Godot;
using System;
using System.Text.RegularExpressions;

public class levelButton : TextureButton
{
	private Label levelLabel;
	public string levelNumber;

    
	[Signal]
	public delegate void level_selected(string levelNumber);

	public override void _Ready()
	{
        levelLabel = GetNode<Label>("levelNumber");

    }

    private void _on_levelButton_pressed()
    {
		EmitSignal("level_selected", levelNumber);
        changeToCurrentTexture();
    }


    //Sets the label text based on nodeName in levelSelector
    //Return the value
    public int setLabelText(string levelName)
    {
        string pattern = @"\d+"; // This regex pattern matches one or more digits.
        MatchCollection matches = Regex.Matches(levelName, pattern);
        if(matches.Count == 0)
        {
            levelNumber = "1";
        }
        else
        {
            levelNumber = matches[0].ToString();
        }
        
        levelLabel.Text = levelNumber;
        return levelNumber.ToInt();
    }


    //Changes the textureButtons.Textures to blue button textures
    public void changeToCurrentTexture()
    {
        this.SetDeferred("disabled", false);
        Texture currentDefault = GD.Load<levelButtonResource>("res://UserInterface/buttons/levelSelectButton/current/currentDefault.tres").texture;
        Texture currentHover = GD.Load<levelButtonResource>("res://UserInterface/buttons/levelSelectButton/current/currentHover.tres").texture;
        Texture currentPressed = GD.Load<levelButtonResource>("res://UserInterface/buttons/levelSelectButton/current/currentPressed.tres").texture;

        convertTextures(currentDefault, currentHover, currentPressed);


    }

    //Changes the textureButtons.Textures to green button textures
    public void changeToCompletedTexture()
    {
        this.SetDeferred("disabled", false);
        Texture completedDefault = GD.Load<levelButtonResource>("res://UserInterface/buttons/levelSelectButton/completed/completedDefault.tres").texture;
        Texture completedHover = GD.Load<levelButtonResource>("res://UserInterface/buttons/levelSelectButton/completed/completedHover.tres").texture;
        Texture completedPressed = GD.Load<levelButtonResource>("res://UserInterface/buttons/levelSelectButton/completed/completedPressed.tres").texture;
        convertTextures(completedDefault, completedHover, completedPressed);
    }


    public void convertTextures(Texture defaultTexture, Texture hoverTexture, Texture pressedTexture)
    {
        this.TextureNormal = defaultTexture;
        this.TextureHover = hoverTexture;
        this.TexturePressed = pressedTexture;
    }


    //Changes the textureButtons.Textures to red
    //Disables the button
    public void changeToNotReachedTexture()
    {
        this.SetDeferred("disabled", true);
    }

}

