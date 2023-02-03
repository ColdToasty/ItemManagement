using Godot;
using System;

public class Player : KinematicBody2D
{
	private enum STATE {
		IDLE,
		MOVE
	}
	//Our animation variables
	private AnimationPlayer animation_player;
	private AnimationTree animation_tree;
	private AnimationNodeStateMachinePlayback animationState; 
	public PickupZone pickUpDetectionZone;

	public PlayerPickup playerPickup;


	private STATE state = STATE.MOVE;

	//Our Movement speed variables
	private int SPEED = 400;
	private Vector2 velocity = Vector2.Zero;


	//Our input vectors
	private Vector2 input_vector = Vector2.Zero;


	public override void _Ready()
	{
		animation_player = GetNode<AnimationPlayer>("AnimationPlayer");
		animation_tree = GetNode<AnimationTree>("AnimationTree");
		animation_tree.Active = true;
		animationState = (AnimationNodeStateMachinePlayback)animation_tree.Get("parameters/playback");

		pickUpDetectionZone = GetNode<PickupZone>("PickupDetectionZone");
		playerPickup = GetNode<PlayerPickup>("PlayerPickup");
	}


	private void move_state(){
		//Get the direction of where our user wants us to move

		//If +1 them Right key is pressed, if -1 then Left key is pressed
		input_vector.x = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		input_vector.y = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");
		input_vector = input_vector.Normalized();

		if(input_vector != Vector2.Zero){
			//Sets up bend positions
			animation_tree.Set("parameters/Idle/blend_position", input_vector);
			animation_tree.Set("parameters/Run/blend_position", input_vector);
			
			//Sets animation to run since we are moving
			animationState.Travel("Run");
			velocity = input_vector * SPEED;
		}
		else{
			animationState.Travel("Idle");
			velocity = Vector2.Zero;
		}
		move();
	}

	private void move(){
		velocity = MoveAndSlide(velocity);
	}






	public override void _PhysicsProcess(float delta)
	{
		//Plays the running animation based on were character is running
		if(state == STATE.MOVE){
			move_state();
		}

	}
}



