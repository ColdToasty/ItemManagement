using Godot;
using System;

public class mob : KinematicBody2D
{
	public PackedScene statsScene;
	public mobStats stats;

	public Vector2 player_position;
	public float adjacent;

	public Vector2 my_original_position;


	//Movement variables
	public int speed = 100;
	public int acceleration = 100;
	public Vector2 velocity = Vector2.Zero;


	// View Cone *Not implemented*
	public Position2D view_pivot;
	public float degrees;
	public float max_view_range = 270;
	public Sprite sprite;

	public Vector2 knockback = Vector2.Zero;


	//Current line of sight
	public DetectionZone detectionZone;

	public viewCone view_cone;
	public RayCast2D raycast;


	
	public override void _Ready()
	{
		stats = GetNode<mobStats>("Stats");
		view_pivot = GetNode<Position2D>("Position2D");
		sprite = GetNode<Sprite>("Sprite");
		detectionZone = GetNode<DetectionZone>("DetectionZone");

		view_cone = GetNode<viewCone>("Position2D/ViewCone");
		raycast = GetNode<RayCast2D>("Position2D/ViewCone/RayCast2D");
	}


	//Slap interactions


	//Add in some hit effect that shows character has been hit
	public void playHitEffect() { 
		
	}

	private void _on_hurtbox_area_entered(CollisionShape2D area)
	{
		stats.Health -= 1;
		playHitEffect();

	}

	//Runs when health reaches 0 in mobstats.cs
	private void _on_Stats_no_health()
	{
		QueueFree();
	}



	
	public void vector_towards_player()
    {
		//Player in this function will always be !null
		Player player = detectionZone.player;
		player_position = player.GlobalPosition;

		velocity = GlobalPosition.DirectionTo(player_position);

		velocity = MoveAndSlide(velocity * speed);


	}

	public void shift_view_cone()
    {
		Player player = detectionZone.player;
		raycast.LookAt(player.GlobalPosition);
		raycast.ForceRaycastUpdate();


	}

	public override void _PhysicsProcess(float delta)
	{

		 
		//Player is in detection zone
		if (detectionZone.see_player())
		{
			shift_view_cone();
			vector_towards_player();
		}
		//Player has left detection zone


		//If player leaves detection zone then mob moves towards last seen location
		else if (player_position != null && GlobalPosition != player_position)
        {
			velocity = GlobalPosition.DirectionTo(player_position);

			velocity = MoveAndSlide(velocity * speed);
		}

		//If player wasn't there then mob does original position
		





	}

	//Plans for movement

	/*
	+ Have mob move towards player




	+ Have raycast arrow to detect collision and have it get the first node it hits and make a checkl
		- if its a player, move towards it
		- if its not a player then dont move towards it
	
	+Implement pathfinding 
	 
	 */


}












