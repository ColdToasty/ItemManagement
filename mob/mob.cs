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
	public CollisionShape2D view_cone_box;
	public viewCone view_cone;

	public float rotation_speed = Mathf.Pi;


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

		float speed = (float)0.2;
		var r = view_cone_box.GlobalRotation;
		var angle_delta = rotation_speed * delta;

		angle = Mathf.LerpAngle(r, angle, speed);
		angle = Mathf.Clamp(angle, r - angle_delta, r + angle_delta);
		view_cone_box.GlobalRotation = angle;
	}

	
	//Need to add a way that rotates viewcone in direction of where player is
	//if player sprints in the detectionZone

	public override void _PhysicsProcess(float delta)
	{
		//Check if player is in detectionZone first
		//If not null then player is sprinting and view cone has to shift
		if (detectionZone.player != null)
		{
			player = detectionZone.player;
		}
		else if (view_cone.player != null)
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

		}










	}

}