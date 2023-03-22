using Godot;
using System;

public class mob : KinematicBody2D
{

	public PackedScene statsScene;
	public mobStats stats;


	public Vector2 velocity = Vector2.Zero;
	public int speed = 250;
	public int acceleration = 2;

	public Vector2 last_player_position = Vector2.Zero;
	public Vector2 original_position;
	public Vector2 direction;
	public Vector2 prevDirection = Vector2.Zero;

	public Player player;
	public PlayerVisible player_visible_shape;


	//Zones which detect player
	public DetectionZone detectionZone;
	public CollisionShape2D view_cone_box;
	public viewCone view_cone;

	public float rotation_speed = Mathf.Pi;

	//CollisionDetection
	public RayCast2D raycast_nodes;

	public Timer timer;

	public int negative_degree = -45;
	public int positive_degree = 45;


	//NavigationAgent
	public NavigationAgent2D nav_agent;

    //signal if mob is in path2D
    [Signal]
	public delegate void stop_route();

	Random rnd = new Random();




	public enum STATE
	{
		CHASE,
		IDLE
	}
	public STATE state = STATE.IDLE;

	public override void _Ready()
	{
		stats = GetNode<mobStats>("Stats");
		detectionZone = GetNode<DetectionZone>("DetectionZone");
		detectionZone.Connect("give_direction", this, "give_direction");
		view_cone_box = GetNode<CollisionShape2D>("ViewBox");
		view_cone = GetNode<viewCone>("ViewBox/ViewCone");

		nav_agent = GetNode<NavigationAgent2D>("NavigationAgent2D");

		timer = GetNode<Timer>("Timer");
		original_position = this.GlobalPosition;

	}


	private void _on_hurtbox_area_entered(Slap area)
	{

		stats.Health -= 1;
		playHitEffect();
	}

	//Add in some hit effect that shows character has been hit
	//And some knock back
	private void playHitEffect()
	{

	}

	//Runs when health reaches 0 in mobstats.cs
	private void _on_Stats_no_health()
	{
		QueueFree();
	}



	public void rotate_cone(float delta, Vector2 position)
	{
		Vector2 direction = position - view_cone_box.GlobalPosition;

		prevDirection = direction;
		var angle = direction.Angle();

		float speed = (float)1;
		var r = view_cone_box.GlobalRotation;
		var angle_delta = rotation_speed * delta;

		angle = Mathf.LerpAngle(r, angle, speed);
		angle = Mathf.Clamp(angle, r - angle_delta, r + angle_delta);
		view_cone_box.GlobalRotation = angle;
		

	}


	private void give_direction(Vector2 last_heard)
    {
		last_player_position = last_heard;
		EmitSignal("stop_route", false);
		//Stop the timer 
		if(timer.TimeLeft != 0)
        {
			timer.Stop();
        }
	}


	//Move our mob towards a certain location
	private void move(float delta, Vector2 position)
	{
		//Set the position to be moved to
		nav_agent.SetTargetLocation(position);

		//Get the direction to the location

		direction = this.GlobalPosition.DirectionTo(nav_agent.GetNextLocation());

		//Set the speed and which way to go
		velocity = (direction * speed);
		nav_agent.SetVelocity(velocity);

		//While mob hasn't arrived at target location
		if (!arrived_at_location())
		{
			velocity = MoveAndSlide(velocity);
			GD.Print("moving towards target");
		}
		//If npc has arrived, but last_player_position is a position that is not (0,0) or the original position
		//This means that they are at a location where the player was last seen
		else if (last_player_position != Vector2.Zero && last_player_position != original_position && arrived_at_location())
		{
			//Start the timer only when it is inactive 
			if (timer.TimeLeft == 0)
			{
				timer.Start(rnd.Next(4, 7));
				GD.Print(timer, " Timer start");
			}

			//Look around while last_position isnt set
			//Means mob is looking around
		}

		else if(arrived_at_location() && last_player_position == original_position)
        {
			last_player_position = Vector2.Zero;
			EmitSignal("stop_route", true);
			GD.Print("arrived at original");

        }

	}
		//When mob finishes investigating the area
	private void _on_Timer_timeout()
		{
			last_player_position = original_position;
			GD.Print("stopped looking around");
		}




	//If mob has arrived at the position its suppose to move towards
	public bool arrived_at_location()
	{
		return nav_agent.IsNavigationFinished();
	}


	//Rotates the view Cone around to see it they can spot the player
	private void look_around()
    {
	
	}


	//When player makes sound in the zone
	//Have the viewcone shift towards it even if the player is not in zone anymore

	public override void _PhysicsProcess(float delta)
	{

		if (view_cone.can_see_player())
		{
			player = view_cone.player;
		}
		else
		{
			player = null;
		}

		if (detectionZone.can_hear_player())
		{
			rotate_cone(delta, detectionZone.last_heard);
		}

		//If person has been seen in either zone
		if (player != null)
		{
			//Rotate the cone towards the player
			rotate_cone(delta, player.GlobalPosition);

			//Move mob towards player location
			if (view_cone.can_see_player())
			{
				EmitSignal("stop_route", false);
				last_player_position = player.GlobalPosition;
				move(delta, player.GlobalPosition);
			}
		}

		//If player is null
		//Mob will move back towards where player was last seen
		//last_player_position not vector2.zero means move to location
		if (!detectionZone.can_hear_player())
		{
			if(last_player_position != Vector2.Zero && last_player_position != original_position)
			{
				move(delta, last_player_position);
				rotate_cone(delta, last_player_position);
			}
			if(last_player_position == original_position)
			{
				move(delta, original_position);
				rotate_cone(delta, original_position);
			}
		}
	}


}