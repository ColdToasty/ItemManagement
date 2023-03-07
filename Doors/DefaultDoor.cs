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
	public override void _Ready()
	{
		area = GetNode<Area2D>("Position2D/Area2D");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationTree = GetNode<AnimationTree>("AnimationTree");
		animationTree.Active = true;
		animation_playback = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
	}

	public void unlockDoor(bool unlock)
	{
		locked = unlock;
	}



	//Maybe have zone that gives out a signal that notifies mobs in zone to check it out

	//If player enters the door
	private void _on_Player_area_entered(Player area)
	{
		player = area;
		check_playerY();

	}

	public override void _PhysicsProcess(float delta)
	{
		if(area.GetOverlappingAreas().Count != 0)
		{
			Area2D a  = (Area2D)area.GetOverlappingAreas()[0];
			player = a.GetParent<Player>();
			
			if(player.input_vector.y != 0)
			{
				check_playerY();
			}
		}

	}

	//Need to check which position the door is position at
	//If how it moves

	private void check_playerY()
	{
		var x = 0;
		//Checks the Y axis
		//Going up
		if (player.input_vector.y == -1 && status != -1)
		{
			status += -1;
		}
		//Coming from Down
		else if(player.input_vector.y == 1 && status != 1)
		{
			status += 1;
		}

		//Need to check which blend position our door is in

		//Check the X axis to see if player choses to close door by pusing it
		//Going left
		if(player.input_vector.x == 1)
        {
			
        }
		//Going Right
		else if(player.input_vector.x == -1)
        {

        }
		Vector2 vector = new Vector2(0, status);
		animationTree.Set("parameters/Status/blend_position", vector);
	}



	private void _on_Player_area_exited(object area)
	{
		player = null;
	}



	//Add area 2d enters or collides
	//If it collides then make it open.
	//Check if mob is sprinting then make it play sound if so

	//If locked then makes players unable to open it - add after implementing basic
}








