using Godot;
using System;

public class levelButton : TextureButton
{
	Label levelName;
	string name;

	[Signal]
	public delegate void levelSelected(string levelNumber);

	public override void _Ready()
	{
		levelName = GetNode<Label>("levelNumber");
		name = levelName.Text;
	}

    private void _on_levelButton_pressed()
    {
		EmitSignal("levelSelected", name);
    }


}

