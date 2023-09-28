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

	//Animations and movements
	public AnimationNodeStateMachinePlayback animation_playback;
	public AnimationTree animationTree;
	public AnimationPlayer animationPlayer;

	public Godot.Vector2 facing = Godot.Vector2.Zero;
	public Timer timer;
	public RunNoise noise;
	public Area2D slapBoxArea;

	private PlayerVisible playerVisibleArea;
	private CollisionShape2D playerVisibleCollisionShape;

	private CollisionShape2D runNoiseCollision;
    private CircleShape2D runNoiseShape;

    private CollisionShape2D playerItemReach;
    private CapsuleShape2D playerItemShape;


    [Signal]
	public delegate void game_over();

	private Sprite playerSprite;
	private bool invisible = false;

	private PackedScene ornamentScene;
	private PackedScene tinselScene;

	//Load the data from this dictionary
	private Godot.Collections.Dictionary save_file_data = GameFiles.current_file_data;


	//Player stat variables
	private int health { get; set; }
	public int Health {
		get { return health; }
		set { health = value; }
	}

	private int speed { get; set; }

	public int Speed
	{
		get { return speed; }
		set { speed = value; }
	}
	private int tempWalkSpeed;
	private int tempRunSpeed;

	private int walkSpeed;
	private int runSpeed;

	private bool freeze = false;

	private float itemReachRadius { get; set; }
	public float ItemReachRadius {
		get { return itemReachRadius; }
		set { itemReachRadius = value; }
	}


    private float itemReachHeight { get; set; }

    public float ItemReachHeight { 
        get { return itemReachHeight; }
        set { itemReachHeight = value; }
    }

    private float noiseRadius { get; set; }
	public float NoiseRadius
	{
		get { return noiseRadius; }
		set { noiseRadius = value; }
	}

	public itemSelect currentItemDisplay;


	public WindowDialog tryToLeaveDialog;
	private TextureButton tryToLeaveCloseButton;
	public override void _Ready()
	{
		//Stats
        health = save_file_data["health"].ToString().ToInt();
        walkSpeed = save_file_data["walkSpeed"].ToString().ToInt();
        runSpeed = save_file_data["runSpeed"].ToString().ToInt();
        ItemReachRadius = save_file_data["reachX"].ToString().ToFloat();
        ItemReachHeight = save_file_data["reachY"].ToString().ToFloat();
        NoiseRadius = save_file_data["noiseRadius"].ToString().ToFloat();



        animationTree = GetNode<AnimationTree>("AnimationTree");
		animationTree.Active = true;
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animation_playback = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");

		slapBoxArea = GetNode<Area2D>("Position2D/Area2D");

		timer = GetNode<Timer>("Timer");

		playerVisibleArea = GetNode<PlayerVisible>("PlayerVisible");
		playerVisibleCollisionShape = GetNode<CollisionShape2D>("PlayerVisible/CollisionShape2D");

		runNoiseCollision = GetNode<CollisionShape2D>("RunNoise/CollisionShape2D");
		runNoiseCollision.SetDeferred("disabled", true);
		runNoiseShape = runNoiseCollision.Shape as CircleShape2D;

        playerItemReach = GetNode<CollisionShape2D>("PlayerItem/CollisionShape2D");
        playerItemShape = playerItemReach.Shape as CapsuleShape2D;

        playerSprite = GetNode<Sprite>("Sprite");
		ornamentScene = GD.Load<PackedScene>("res://Player/playerItems/Bulb.tscn");
		tinselScene = GD.Load<PackedScene>("res://Player/playerItems/Tinsel.tscn");



        currentItemDisplay = GetNode<itemSelect>("itemSelect");
		tryToLeaveDialog = GetNode<WindowDialog>("WindowDialog");
		tryToLeaveCloseButton = tryToLeaveDialog.GetCloseButton();

		//Set sizes of noiseShape
		runNoiseShape.Radius = NoiseRadius;

		//Set Size of reach
		playerItemShape.Radius = itemReachRadius;
		playerItemShape.Height = ItemReachHeight;


}

	//Disables movement
	public void DisableSpeed()
	{
		tempWalkSpeed = walkSpeed;
		tempRunSpeed = runSpeed;
		freeze = true;

		walkSpeed = 0;
		runSpeed = 0;
	}

	//Re enables movemment
	public void EnableSpeed()
	{
		walkSpeed = tempWalkSpeed;
		runSpeed = tempRunSpeed;

		freeze = false;
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
				Health -= 1;
				GD.Print(Health);
			}
			else if(( (hitPlayerBox)area).GetParent() is Cop)
			{
				GD.Print("Im knocked out");
				Health = 0;
				EmitSignal("game_over");
			}
		}
		if(Health <= 0)
		{
			//Play disappear animation
			//Emit gameOver signal
			EmitSignal("game_over");
		}
	}

	public void tryToLeaveDialogShow()
	{
		tryToLeaveDialog.Show();
		DisableSpeed();
	}


	public void move_state()
	{
		if (!freeze)
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
					Speed = runSpeed;
                    runNoiseCollision.SetDeferred("disabled", false);
				}
				else
				{
					Speed = walkSpeed;
                    runNoiseCollision.SetDeferred("disabled", true);
				}
				animation_playback.Travel("Moving");
				velocity = input_vector * Speed;
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
		else
		{
			animation_playback.Stop();
		}
		
	}


	//Need to add resource to each item
	public void attack_state()
	{
		velocity = Godot.Vector2.Zero;
		Godot.Vector2 mousePosition = GetGlobalMousePosition();
		//Throw in direction of mouse position

		if (currentItemDisplay.itemAvailable)
		{
			if (itemState == ITEMSTATE.ORNAMENT)
			{
				//instance the ornament
				//Navigate the npcSort tree 
				Bulb ornament = (Bulb)ornamentScene.Instance();
				GetParent().AddChild(ornament);
				ornament.GlobalPosition = this.GlobalPosition;
				currentItemDisplay.OrnamentCount--;

			}
			else if (itemState == ITEMSTATE.TINSEL)
			{
				Tinsel tinsel = (Tinsel)tinselScene.Instance();
				GetParent().AddChild(tinsel);
				tinsel.GlobalPosition = this.GlobalPosition;
				currentItemDisplay.TinselCount--;
			}

			//Hit someone with cane
			else if (itemState == ITEMSTATE.CANE)
			{
				GD.Print("smack some people");
				currentItemDisplay.CaneCount--;
				//Set blend_position of cane based on input_vector
			}

			//turn invisible (might add timer) 
			else if (itemState == ITEMSTATE.INVISIBILITY)
			{
				currentItemDisplay.InvisibilityCount--;
				toggleInvisible();
			}
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

		else  if (Input.IsActionJustPressed("Tinsel"))
		{
			itemState = ITEMSTATE.TINSEL;
			GD.Print("tinsel selected");
		}

		else if (Input.IsActionJustPressed("Cane"))
		{
			itemState = ITEMSTATE.CANE;
			GD.Print("cane selected");
		}

		else  if (Input.IsActionJustPressed("Invisibility"))
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

		if (!freeze)
		{
			Godot.Vector2 tryToLeavePosition = new Godot.Vector2(this.GlobalPosition.x - (tryToLeaveDialog.RectSize.x / 2), this.GlobalPosition.y);
			tryToLeaveDialog.SetGlobalPosition(tryToLeavePosition);
		}

		if (tryToLeaveCloseButton.Pressed)
		{
			EnableSpeed();
		}

	}

}










