using Godot;
using System;



/*
 Need to make mob be able to interact with door 

 - Should be able to close and open the door
 */
public class DefaultDoor : KinematicBody2D
{
	public Player player;


	public Area2D area;
	public AnimationTree animationTree;
	public AnimationPlayer animationPlayer;
	public AnimationNodeStateMachinePlayback animation_playback;
	public CollisionShape2D collider;


	public override void _Ready()
	{
		area = GetNode<Area2D>("Area2D");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationTree = GetNode<AnimationTree>("AnimationTree");
		animationTree.Active = true;
		animation_playback = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
		collider = GetNode<CollisionShape2D>("CollisionShape2D");
	}


	/*
	 Make door open on both sides
	 have a blend position for both sides 
	 -1 for door to be opened in up direction and 1 for door to be opened in down direction
	 
	 Basic addition maths for checking which way to open the door based on input vector of player

	 For this Door
	 
	 if 0 and -1 then openUp
	 if 0 and 1 then open down

	 if 1 then 0 and close
	 if -1 then 0 and close 
	 */

	//Have player have to be in player reach zone and have to click left mouse button
	//Make Door open in direction of players current facing direction
	public void openDoor()
	{
		int toggle = -1;

		//Change the blend position of the statemachine in animationtree
		//This toggles the direction the door opens

		Vector2 doorPosition =(Vector2)animationTree.Get("parameters/Status/blend_position");
		doorPosition.y = doorPosition.y * toggle;
		animationTree.Set("parameters/Status/blend_position", doorPosition);

	}

	public override void _Input(InputEvent @event)
	{
		//Check if player has clicked mouse and is in the door zone
		if(@event is InputEventMouseButton && player != null)
		{
			//Convert the event into mousebutton event
			InputEventMouseButton e = (InputEventMouseButton)@event;
			//Check if e is the left mouse button pressed
			if (e.Pressed && e.ButtonMask == (int)ButtonList.Left)
			{
				openDoor();
			}
		}
	}



	//Need to check mouse position is over the area2d 
	private void _on_playerReach_area_entered(PlayerReach area)
	{
		player = area.GetParent<Player>();
		
	}

	//Reset area to make sure player cannot open this specific door
	private void _on_playerReach_area_exited(PlayerReach area)
	{
		player = null;
	}






	//If it collides then make it open.
	//Check if mob is sprinting then make it play sound if so

	//If locked then makes players unable to open it - add after implementing basic
}









