using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public class Mob : KinematicBody2D
{

	public PackedScene statsScene;
	public mobStats stats;


	public Vector2 velocity = Vector2.Zero;
	public int speed = 250;
	public int acceleration = 2;

	public bool position_available = false;
	public bool can_move = false;

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

	//Timer to wait before moving
	public Timer idleTimer;
	public bool timerStarted = false;
	//Timer to idle before heading back to original location
	public Timer original_location_timer;

	//
	public int negative_degree = -45;
	public int positive_degree = 45;


	//NavigationAgent
	public NavigationAgent2D nav_agent;

	//signal if mob is in path2D
	[Signal]
	public delegate void stop_route();

    [Signal]
    public delegate void can_player_hide();


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
		idleTimer = GetNode<Timer>("idleTimer");
		original_location_timer = GetNode<Timer>("originalLocationTimer");
		nav_agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
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
		EmitSignal("stop_route", false);
	}


	//Move our mob towards a certain location
	private void move()
	{
		direction = this.GlobalPosition.DirectionTo(nav_agent.GetNextLocation());
		//Set the speed and which way to go
		velocity = (direction * speed);
		velocity = MoveAndSlide(velocity);
		

	}
		//When mob finishes investigating the area


	//If mob has arrived at the position its suppose to move towards
	public bool arrived_at_location()
	{
		return nav_agent.IsNavigationFinished();
	}


	//Rotates the view Cone around to see it they can spot the player
	private void look_around()
	{
	
	}
	//When the player is heard in its detection zone
	private void _on_idleTimer_timeout()
	{
		can_move = true;
	}

	//When mob does not find player and has to go back to its starting position
	private void _on_originalLocationTimer_timeout()
	{
		nav_agent.SetTargetLocation(original_position);
		can_move = true;
	}


	//When player makes sound in the zone
	//Have the viewcone shift towards it even if the player is not in zone anymore

	public override void _PhysicsProcess(float delta)
	{
		//If the viewcone sees the player
		if (view_cone.can_see_player())
		{
			//Stop the route, if any
			EmitSignal("stop_route", false);
			player = view_cone.player;
			idleTimer.Stop();
			original_location_timer.Stop();
			//Rotate the cone towards the player
			rotate_cone(delta, player.GlobalPosition);
			nav_agent.SetTargetLocation(player.GlobalPosition);
			can_move = true;

			EmitSignal("can_player_hide", false);

		}
		else if (detectionZone.can_hear_player())
		{
			timerStarted = false;
			idleTimer.Stop();
			original_location_timer.Stop();
			EmitSignal("stop_route", false);
			rotate_cone(delta, detectionZone.last_heard);
			nav_agent.SetTargetLocation(detectionZone.last_heard);
            EmitSignal("can_player_hide", true);
            if (!timerStarted)
			{
				timerStarted = true;
				idleTimer.Start(1);
			}
		}
		else
		{
            EmitSignal("can_player_hide", true);
            player = null;
		}
		
		//If mob hasnt arrived at target location
		if (!arrived_at_location() && can_move)
		{
				idleTimer.Stop();
				original_location_timer.Stop();

				//GD.Print(nav_agent.GetNextLocation());
				rotate_cone(delta, nav_agent.GetNextLocation());
				
				//Make it so that when player is heard stop movement and follow 185

				move();
				

				
		}
		//If back to original position
		else if (arrived_at_location() && can_move)
		{
			can_move = false;
			timerStarted = false;

			//Means we need to navigate back to the original position 
			if(nav_agent.GetTargetLocation() != original_position)
			{
				original_location_timer.Start(rnd.Next(4, 7));
			}

		}
		//GD.Print(idleTimer.TimeLeft);

	}




}












