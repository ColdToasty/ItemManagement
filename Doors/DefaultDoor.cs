using Godot;
using System;

public class DefaultDoor : KinematicBody2D
{
	public bool locked = false;
	public Player player;

	private int status = 0;
	// Called when the node enters the scene tree for the first time.

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

	public void unlockDoor(bool unlock)
	{
		locked = unlock;
	}



	public void openDoor()
	{
		int toggle = -1;
		Vector2 doorPosition =(Vector2)animationTree.Get("parameters/Status/blend_position");
		doorPosition.y = doorPosition.y * toggle;

		
		animationTree.Set("parameters/Status/blend_position", doorPosition);

	}

	public override void _Input(InputEvent @event)
	{
		if(@event is InputEventMouseButton && player != null)
		{
			InputEventMouseButton e = (InputEventMouseButton)@event;
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


	private void _on_playerReach_area_exited(PlayerReach area)
	{
		player = null;
	}



	//Have player have to be in player reach zone and have to click left mouse button
	//Make Door open in direction of players current facing direction


	//Add area 2d enters or collides
	//If it collides then make it open.
	//Check if mob is sprinting then make it play sound if so

	//If locked then makes players unable to open it - add after implementing basic
}









