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
		if (detectionZone.can_see_player())
		{
			state = STATE.CHASE;
		}
	}

	public void rotate_cone(float delta, Vector2 position)
    {
		Vector2 direction = position - view_cone_box.GlobalPosition;
		var angle = direction.Angle();

		float speed = (float)0.1;
		var r = view_cone_box.GlobalRotation;
		var angle_delta = rotation_speed * delta;

		angle = Mathf.LerpAngle(r, angle, speed);
		angle = Mathf.Clamp(angle, r - angle_delta, r + angle_delta);
		view_cone_box.GlobalRotation = angle;
	}

	

	public void move(float delta, Vector2 position)
    {
		direction = this.GlobalPosition.DirectionTo(position);
		velocity = (direction * speed);
		velocity = MoveAndSlide(velocity);
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

		//If person has been seen in either zones and last_player_position is not Vector2.Zero
		if (player != null)
		{
			rotate_cone(delta, player.GlobalPosition);

			//Move mob towards player location
			if (view_cone.can_see_player())
			{
				last_player_position = player.GlobalPosition;
				move(delta, player.GlobalPosition);
			}
			else
			{
				velocity = velocity.MoveToward(Vector2.Zero, delta);
			}

		}
		else if (player == null && last_player_position != Vector2.Zero)
        {
			move(delta, last_player_position);
		}


		if (this.GlobalPosition == last_player_position)
        {
			last_player_position = Vector2.Zero;
        }










	}

}