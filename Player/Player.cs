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

	public int SPEED = 200;
	public STATE state = STATE.MOVING;
	public Vector2 input_vector = Vector2.Zero;
	public Vector2 velocity = Vector2.Zero;

	public GridContainer hotbar;
	public GridContainer inventory_slots;

	//Animations and movements
	public AnimationNodeStateMachinePlayback animation_playback;
	public AnimationTree animationTree;
	public AnimationPlayer animationPlayer;



	public Timer timer;




	public Area2D slapBoxArea;


	//Items list
	public List<ItemInfo> items;
	public ItemDatabase db;
	public int place_location_number;
	public List<Texture> item_textures = new List<Texture>();

	public RunNoise noise;


	
	public override void _Ready()
	{
		animationTree = GetNode<AnimationTree>("AnimationTree");
		animationTree.Active = true;
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animation_playback = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
		slapBoxArea = GetNode<Area2D>("Position2D/Area2D");
		db = GetNode<ItemDatabase>("/root/ItemDatabase");
		timer = GetNode<Timer>("Timer");
		items = db.items;
		
		//Gets the number of placeLocation nodes
		place_location_number = GetTree().CurrentScene.GetNode<YSort>("PlaceLocations").GetChildCount();
		add_textures();
		noise = GetNode<RunNoise>("RunNoise");
	
	}


	public void move()
	{
		velocity = MoveAndSlide(velocity);
	}

	//adds the textures to list
	//Need to make so player is not dependent on how many boxes there are
	public void add_textures()
	{
		Random rnd = new Random();
		int item_data_count = db.get_list_length();

		for (int i = 0; i < place_location_number; i++)
		{
			int rnd_sprite = rnd.Next(item_data_count);
			item_textures.Add(db.items[rnd_sprite].texture);
		}

	}

	//when a PlaceLocationPlayerDetectionZone enters its area
	private void _on_PlaceLocation_area_entered(object area)
	{

		
	}




	public void move_state()
	{
		input_vector.x = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		input_vector.y = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");
		if (Input.IsActionPressed("Sprint") && velocity != Vector2.Zero)
		{
			SPEED = 400;
			noise.monitor = true;
		}
		else
		{
			SPEED = 200;
			noise.monitor = false;
		}
		input_vector = input_vector.Normalized();
		if (input_vector != Vector2.Zero)
		{

			//Set up animation blend_positions
			animationTree.Set("parameters/Idle/blend_position", input_vector);
			animationTree.Set("parameters/Slapping/blend_position", input_vector);
			animationTree.Set("parameters/Moving/blend_position", input_vector);

			//When input vector signals to move
			//Changes from Idle or starting node and travel to given state machine
			//Then use the input_vector as direction for which way to move
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












