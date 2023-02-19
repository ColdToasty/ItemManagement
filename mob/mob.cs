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

	public Vector2 last_player_position = Vector2.Zero;


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
		ViewCone = GetNode<viewCone>("ViewCone");
	}


	private void _on_hurtbox_area_entered(CollisionShape2D area)
	{
		stats.Health -= 1;
		playHitEffect();
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
		if (detectionZone.can_see_player() || ViewCone.can_see_player())
		{
			state = STATE.CHASE;
		}
	}

	
	public void rotate(float delta, Vector2 position)
    {
		float r = ViewCone.GlobalRotation;

		//direction = GlobalPosition.DirectionTo(player.GlobalPosition);
		//velocity = velocity.MoveToward(direction * speed, acceleration * delta);

		float angle = (position - this.GlobalPosition).Angle();



		double angle_delta = rotation_speed * delta;

		angle = Mathf.LerpAngle(r, angle, 1);

		angle = Mathf.Clamp((float)angle, (float)(r - angle_delta), (float)(r + angle_delta));
		ViewCone.GlobalRotation = angle;
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
			if (detectionZone.player != null && ViewCone.player == null)
			{
				player = detectionZone.player;
				last_player_position = player.GlobalPosition;
			}
			//If player is not sprinting use the viewCone player
			else if(ViewCone.player != null)
			{
				player = ViewCone.player;
				last_player_position = player.GlobalPosition;
			}
            else
            {
				player = null;
				last_player_position = Vector2.Zero;
			}


			//If person has been seen in either zones and last_player_position is not Vector2.Zero
			if (player != null || last_player_position != Vector2.Zero)
			{
				rotate(delta, player.GlobalPosition);
			}
			else if (player == null && last_player_position != Vector2.Zero)
            {
				rotate(delta, last_player_position);
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











