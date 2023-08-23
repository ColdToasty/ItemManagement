using Godot;
using System.Collections.Generic;


public class HidingSpot : StaticBody2D
{
	private bool mouse_over = false;
	private Sprite object_skin;
	private bool player_already_hidden = false;
	private bool player_reach = false;
	private Area2D release;

	[Signal]
	public delegate void hide_the_player_in_me(Vector2 position);
	
	[Signal]
	public delegate void show_the_player(Vector2 releasePosition);
	
	public override void _Ready()
	{
		object_skin = GetNode<Sprite>("Sprite");
		release = GetNode<Area2D>("releaseArea");
	}

	private void _on_mouseArea_mouse_entered()
	{
			mouse_over = true;
			//Change skin to one with a hover over
	}


	private void _on_mouseArea_mouse_exited()
	{
			mouse_over = false;
			//Change sprite back to original
	}

	private void hide_player()
	{

		EmitSignal("hide_the_player_in_me", this.GlobalPosition);
		player_already_hidden = true;
		//Change sprite frame
		//Disable the clickable zone
		//Disable player movement
	}

	private void show_player()
	{
		EmitSignal("show_the_player", release.GlobalPosition);
		player_already_hidden = false;
		//Change this sprite back to original
		//Enable the clickable zone
		//Enable player movement
	}


	private void _on_playerDetection_area_entered(PlayerReach playerReach)
	{
		player_reach = true;
		GD.Print("player can click me");
	}

    private void _on_playerDetection_area_exited(object area)
    {
		player_reach = false;
    }


    public override void _Input(InputEvent @event)
	{

		//Check if player has clicked mouse and is in the door zone
		if (@event is InputEventMouseButton)
		{

			//Convert the event into mousebutton event
			InputEventMouseButton e = (InputEventMouseButton)@event;
			//Check if e is the left mouse button pressed
			if (e.Pressed && e.ButtonMask == (int)ButtonList.Left)
			{
					//When player is hovering over hidingSpot and player is not yet hidden
					if(mouse_over && !player_already_hidden && player_reach)
					{
						//call hide_player
						hide_player();
						GD.Print("hide me");
					}
					//If player is hidden then show the player
					else if (player_already_hidden) {
						show_player();
						GD.Print("reveal me");
					}
				
			}
		}
		//If player decides to reveal themselves by moving
		if (@event is InputEventKey && player_already_hidden)
		{
			InputEventKey e = (InputEventKey)@event;

			HashSet<float> keyCheck = new HashSet<float>{
				e.GetActionStrength("Left"),
				e.GetActionStrength("Right"),
				e.GetActionStrength("Up"),
				e.GetActionStrength("Down"),
				e.GetActionStrength("Slap")
			};

			if (keyCheck.Contains((float)1)) {
				show_player();
				GD.Print("revealed me by moving");
			}
		}
	}

}




