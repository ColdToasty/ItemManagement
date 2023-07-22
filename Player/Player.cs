using Godot;
using System;
using System.Collections.Generic;
public class Player : KinematicBody2D
{
	public enum STATE
	{
		MOVING,
		ATTACK
	}

	public int SPEED = 100;
	public STATE state = STATE.MOVING;
	public Vector2 input_vector = Vector2.Zero;
	public Vector2 velocity = Vector2.Zero;

	public GridContainer hotbar;
	public GridContainer inventory_slots;

	//Animations and movements
	public AnimationNodeStateMachinePlayback animation_playback;
	public AnimationTree animationTree;
	public AnimationPlayer animationPlayer;

	public Vector2 facing = Vector2.Zero;


	public Timer timer;

	public RunNoise noise;




	public Area2D slapBoxArea;



	
	public override void _Ready()
	{
		animationTree = GetNode<AnimationTree>("AnimationTree");
		animationTree.Active = true;
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animation_playback = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");

		slapBoxArea = GetNode<Area2D>("Position2D/Area2D");

		timer = GetNode<Timer>("Timer");
	}


	public void move()
	{
		velocity = MoveAndSlide(velocity);
	}

	//adds the textures to list
	//Need to make so player is not dependent on how many boxes there are


	//when a PlaceLocationPlayerDetectionZone enters its area
	private void _on_PlaceLocation_area_entered(object area)
	{

		
	}


	//When player gets hit by mob with a hitPlayerBox
    private void _on_hitPlayerBox_entered(object area)
    {
		if(area is hitPlayerBox)
		{
			if(((hitPlayerBox)area).GetParent() is Old)
			{
				GD.Print("Im hurt");
				//Slow player down
				//Decrease health
			}
			else if(( (hitPlayerBox)area).GetParent() is Cop)
			{
				GD.Print("Im knocked out");
			}
		}
    }


    public void move_state()
	{
		input_vector.x = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		input_vector.y = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");

		input_vector = input_vector.Normalized();
		if (input_vector != Vector2.Zero)
		{

			//Set up animation blend_positions
			animationTree.Set("parameters/Idle/blend_position", input_vector);
			animationTree.Set("parameters/Slapping/blend_position", input_vector);
			animationTree.Set("parameters/Moving/blend_position", input_vector);
			facing = input_vector;
			//When input vector signals to move
			//Changes from Idle or starting node and travel to given state machine
			//Then use the input_vector as direction for which way to move
			if (Input.IsActionPressed("Sprint"))
				{
					SPEED = 400;
				}
			else
				{
					SPEED = 200;
				}
			
			animation_playback.Travel("Moving");
			velocity = input_vector * SPEED;
				
			}
			
		else
		{
			animation_playback.Travel("Idle");
			velocity = Vector2.Zero;
		}
			

		move();
		
		if (Input.IsActionJustPressed("Slap"))
			{
				state = STATE.ATTACK;
			}
	}

	public void attack_state()
	{
		velocity = Vector2.Zero;
		animation_playback.Travel("Slapping");

	}

	private void _on_hurtbox_area_shape_entered(Area2D whatEntered)
	{

	}


	private void _on_AnimationPlayer_animation_finished(String anim_name)
	{
		state = STATE.MOVING;
	}

	public override void _PhysicsProcess(float delta)
	{

		

		if(state == STATE.MOVING)
		{
			move_state();
		}

		else if(state == STATE.ATTACK)
		{
			attack_state();
		}
		

	}
}


