using Godot;
using System;

public class mob : KinematicBody2D
{

	public PackedScene statsScene;
	public mobStats stats;


	public Vector2 velocity = Vector2.Zero;
	public int acceleration = 100;
	public int speed = 300;

	public Vector2 originalPosition;
	public Vector2 direction;

	public Player player;
	public Sprite sprite;


	public DetectionZone detectionZone;
	public viewCone ViewCone;
	public Position2D pivotCone;
	public double rotation_speed = Mathf.Pi;
	public RayCast2D raycast;



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
		originalPosition = this.GlobalPosition;
		sprite = GetNode<Sprite>("Sprite");
		ViewCone = GetNode<viewCone>("RayCast2D/ViewCone");
		raycast = GetNode<RayCast2D>("RayCast2D");
	}


	private void _on_hurtbox_area_entered(CollisionShape2D area)
	{
		stats.Health -= 1;
		playHitEffect();
		GD.Print("hit");
	}

	//Add in some hit effect that shows character has been hit
	public void playHitEffect()
	{

	}

	//Runs when health reaches 0 in mobstats.cs
	private void _on_Stats_no_health()
	{
		QueueFree();
	}

	public void seek_player()
	{
		if (ViewCone.can_see_player())
		{
			state = STATE.CHASE;
		}
	}

	//Need to add a way that rotates viewcone in direction of where player is
	//if player sprints in the detectionZone

	public override void _PhysicsProcess(float delta)
	{
		seek_player();

		if (state == STATE.CHASE)
		{
			//Check if player is in detectionZone first
			//If not null then player is sprinting and view cone has to shift
			if (detectionZone.player != null && ViewCone.player == null && detectionZone.player.SPEED > 200)
			{
				player = detectionZone.player;
				GD.Print("Sprint");

			}
			//If player is not sprinting use the viewCone player
            else if(ViewCone.player != null)
            {
				player = ViewCone.player;
			}

			//Check the detectionZone first
			

			if (player != null)
			{
				raycast.LookAt(player.GlobalPosition);
				raycast.ForceRaycastUpdate();
				direction = GlobalPosition.DirectionTo(player.GlobalPosition);
				velocity = velocity.MoveToward(direction * speed, acceleration * delta);
			}
			else
			{
				state = STATE.IDLE;
			}
			
		}
		if (state == STATE.IDLE)
        {
			velocity = velocity.MoveToward(Vector2.Zero * speed, acceleration * delta);
		}

		velocity = MoveAndSlide(velocity);




	}

}











