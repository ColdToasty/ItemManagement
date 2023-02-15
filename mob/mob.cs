using Godot;
using System;

public class mob : KinematicBody2D
{
	public PackedScene statsScene;
	public mobStats stats;
	public viewCone mobViewCone;
	public DetectionZone detectionZone;


	public Vector2 velocity = Vector2.Zero;
	public int acceleration = 100;
	public int speed = 400;

	public Vector2 originalPosition;
	public Vector2 direction;

	public Player player;

	public Sprite sprite;


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
		if (detectionZone.see_player())
		{
			state = STATE.CHASE;
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		if (state == STATE.CHASE)
		{
			player = detectionZone.player;
			if (player != null)
			{
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
			direction = GlobalPosition.DirectionTo(originalPosition);
			velocity = velocity.MoveToward(direction * speed, (acceleration / 4) * delta);
			seek_player();
		}

		velocity = MoveAndSlide(velocity);




	}

}











