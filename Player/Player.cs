using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Policy;

public class Player : KinematicBody2D
{
	public enum STATE
	{
		MOVING,
		ATTACK
	}
	public STATE state = STATE.MOVING;

	public enum ITEMSTATE
	{
		NONE,
		ORNAMENT,
		CANE,
		TINSEL,
		INVISIBILITY
	}

	public ITEMSTATE itemState = ITEMSTATE.NONE;

	public Godot.Vector2 input_vector = Godot.Vector2.Zero;
	public Godot.Vector2 velocity = Godot.Vector2.Zero;

	
	public GridContainer hotbar;
	public GridContainer inventory_slots;

	[Export]
	private playerStats playerStats;
	
	//Animations and movements
	public AnimationNodeStateMachinePlayback animation_playback;
	public AnimationTree animationTree;
	public AnimationPlayer animationPlayer;

	public Godot.Vector2 facing = Godot.Vector2.Zero;
	public Timer timer;
	public RunNoise noise;
	public Area2D slapBoxArea;

	public int SPEED;

	private PlayerVisible playerVisibleArea;
	private CollisionShape2D playerVisibleCollisionShape;

	[Signal]
	public delegate void showGameOverScreen();


	private Sprite playerSprite;
	private bool invisible = false;

	private PackedScene ornamentScene;
	private PackedScene tinselScene;

	public override void _Ready()
	{
		animationTree = GetNode<AnimationTree>("AnimationTree");
		animationTree.Active = true;
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animation_playback = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");

		slapBoxArea = GetNode<Area2D>("Position2D/Area2D");

		timer = GetNode<Timer>("Timer");

		playerVisibleArea = GetNode<PlayerVisible>("PlayerVisible");
		playerVisibleCollisionShape = GetNode<CollisionShape2D>("PlayerVisible/CollisionShape2D");
		playerStats = ResourceLoader.Load("res://Player/playerStats/playerStats.tres") as playerStats;
		playerSprite = GetNode<Sprite>("Sprite");

		SPEED = playerStats.Speed;

		ornamentScene = GD.Load<PackedScene>("res://Player/playerItems/Bulb.tscn");
        tinselScene = GD.Load<PackedScene>("res://Player/playerItems/Tinsel.tscn");
    }


    public void move()
	{
		velocity = MoveAndSlide(velocity);
	}

	//adds the textures to list

	//when a PlaceLocationPlayerDetectionZone enters its area
	private void _on_PlaceLocation_area_entered(object area)
	{

		
	}

	public void enableInvisibility()
	{
		playerVisibleArea.SetDeferred("monitoring", false);
		playerVisibleArea.SetDeferred("monitorable", false);
		playerVisibleCollisionShape.SetDeferred("disabled", true);
		
	}

	public void disableInvisibility()
	{
		playerVisibleArea.SetDeferred("monitoring", true);
		playerVisibleArea.SetDeferred("monitorable", true);
		playerVisibleCollisionShape.SetDeferred("disabled", false);
	}

	public void enableSprite()
	{
		playerSprite.SetDeferred("visible", true);
	}
	public void disableSprite()
	{
		playerSprite.SetDeferred("visible", false);
	}

	public void toggleInvisible()
	{
        invisible = !invisible;
        if (invisible)
		{
			playerSprite.Modulate = new Color(1, 1, 1, (float)0.25);
            playerVisibleCollisionShape.SetDeferred("disabled", true);
        }
		else
		{
			playerSprite.Modulate = new Color(1, 1, 1, 1);
            playerVisibleCollisionShape.SetDeferred("disabled", false);
        }
		
	}


	//When player gets hit by mob with a hitPlayerBox
	private void _on_hitPlayerBox_entered(object area)
	{
		if(area is hitPlayerBox)
		{
			if(((hitPlayerBox)area).GetParent() is Old)
			{
				GD.Print("Im hurt");
				playerStats.Health -= 1;
			}
			else if(( (hitPlayerBox)area).GetParent() is Cop)
			{
				GD.Print("Im knocked out");
				playerStats.Health = 0;
			}
		}
		if(playerStats.Health <= 0)
		{
			//Play disappear animation
			//Emit gameOver signal
			EmitSignal("showGameOverScreen");
		}
	}


	public void move_state()
	{
		input_vector.x = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
		input_vector.y = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");

		input_vector = input_vector.Normalized();
		if (input_vector != Godot.Vector2.Zero)
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
				SPEED = playerStats.SprintSpeed;
				}
			else
				{
				SPEED = playerStats.Speed;
				}
			animation_playback.Travel("Moving");
			velocity = input_vector * SPEED;
			
			}
			
		else
		{
			animation_playback.Travel("Idle");
			velocity = Godot.Vector2.Zero;
			
		}
			
		move();
		
		if (Input.IsActionJustPressed("UseItem"))
			{
			state = STATE.ATTACK;
		}
		
	}


	//Need to add resource to each item
	public void attack_state()
	{
		velocity = Godot.Vector2.Zero;
		Godot.Vector2 mousePosition = GetGlobalMousePosition();
		//Throw in direction of mouse position
		if(itemState == ITEMSTATE.ORNAMENT)
		{
			//instance the ornament
			//Navigate the npcSort tree 
			Bulb ornament = (Bulb)ornamentScene.Instance();
			GetParent().AddChild(ornament);
			ornament.GlobalPosition = this.GlobalPosition;
        }
        else if (itemState == ITEMSTATE.TINSEL)
        {
			Tinsel tinsel = (Tinsel)tinselScene.Instance();
			GetParent().AddChild(tinsel);
            tinsel.GlobalPosition = this.GlobalPosition;
        }

		//Hit someone with cane
        else if(itemState == ITEMSTATE.CANE)
		{
            GD.Print("smack some people");
			//Set blend_position of cane based on input_vector
        }

		//turn invisible (might add timer) 
		else if(itemState == ITEMSTATE.INVISIBILITY)
		{
            GD.Print("you cant see me");
        }

		state = STATE.MOVING;



	}


	private void _on_AnimationPlayer_animation_finished(String anim_name)
	{
		state = STATE.MOVING;
	}

	public override void _PhysicsProcess(float delta)
	{
		if (Input.IsActionJustPressed("Ornament"))
		{
			itemState = ITEMSTATE.ORNAMENT;
            GD.Print("ornament selected");


        }

        if (Input.IsActionJustPressed("Tinsel"))
        {
            itemState = ITEMSTATE.TINSEL;
            GD.Print("tinsel selected");
        }

        if (Input.IsActionJustPressed("Cane"))
        {
            itemState = ITEMSTATE.CANE;
            GD.Print("cane selected");
        }

        if (Input.IsActionJustPressed("Invisibility"))
        {
            itemState = ITEMSTATE.INVISIBILITY;
            GD.Print("invisibility selected");
        }

        if (state == STATE.MOVING)
		{
			move_state();
		}

		if(state == STATE.ATTACK)
		{
			attack_state();
		}
 

    }
}


