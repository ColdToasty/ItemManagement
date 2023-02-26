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


	//NavigationAgent
	public NavigationAgent2D nav_agent;

	Random rnd_time = new Random();

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

		view_cone_box = GetNode<CollisionShape2D>("ViewBox");
		view_cone = GetNode<viewCone>("ViewBox/ViewCone");

		nav_agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		original_position = this.GlobalPosition;

		timer = GetNode<Timer>("Timer");


	}


	private void _on_hurtbox_area_entered(Slap area)
	{

		stats.Health -= 1;
		playHitEffect();
	}

	//Add in some hit effect that shows character has been hit
	//And some knock back
	public void playHitEffect()
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
		var angle = direction.Angle();

		float speed = (float)1;
		var r = view_cone_box.GlobalRotation;
		var angle_delta = rotation_speed * delta;

		angle = Mathf.LerpAngle(r, angle, speed);
		angle = Mathf.Clamp(angle, r - angle_delta, r + angle_delta);
		view_cone_box.GlobalRotation = angle;
	}

	
	//Move our mob towards a certain location
	public void move(float delta, Vector2 position)
	{
		//Set the position to be moved to
		nav_agent.SetTargetLocation(position);

		//Get the direction to the location

		direction = this.GlobalPosition.DirectionTo(nav_agent.GetNextLocation());

		//Set the speed and which way to go
		velocity = (direction * speed);
		nav_agent.SetVelocity(velocity);

		//Move while it hasn't arrived
		if (!arrived_at_location())
		{
			velocity = MoveAndSlide(velocity);
        }
        else
		{ 
			//Start the timer only when it is inactive 
			if(timer.TimeLeft == 0 && last_player_position != original_position)
            {
				timer.Start(rnd_time.Next(4, 8));
            }
			//Look around while last_position isnt set
			//Means mob is looking around
			if(last_player_position != original_position)
            {
				look_around();
            }
			

		}




	}

	//If mob has arrived at the position its suppose to move towards
	public bool arrived_at_location()
	{
		return nav_agent.IsNavigationFinished();
	}

	//When mob finishes investigating the area
	private void _on_Timer_timeout()
	{
		last_player_position = original_position;
		GD.Print("stopped looking around");
	}

	//Rotates the view Cone around to see it they can spot the player
	private void look_around()
    {
		float current_degree = view_cone_box.GlobalRotation;
    }


	public override void _PhysicsProcess(float delta)
	{
		
		if (detectionZone.can_see_player())
		{
			player = detectionZone.player;
		}
		else if (view_cone.can_see_player())
		{
			player = view_cone.player;
			
		}
		else
		{
			player = null;
		}

		//If person has been seen in either zone
		if (player != null)
		{
			timer.Stop();
			//Rotate the cone towards the player
			rotate_cone(delta, player.GlobalPosition);

			//Move mob towards player location
			if (view_cone.can_see_player())
			{
				last_player_position = player.GlobalPosition;
				move(delta, player.GlobalPosition);
			}

		}
		//If player is not in either zone
		//Mob will move back towards where player was last seen
		else if(last_player_position != Vector2.Zero)
		{
			move(delta, last_player_position);
		}











	}

}

