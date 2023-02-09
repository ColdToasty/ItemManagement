using Godot;
using System;

public class mob : KinematicBody2D
{
	public PackedScene statsScene;
	public mobStats stats;

	public override void _Ready()
	{
		stats = GetNode<mobStats>("Stats");

	}


	private void _on_hurtbox_area_entered(CollisionShape2D area)
	{
		stats.Health -= 1;
		playHitEffect();
		GD.Print("hit");
	}

	//Add in some hit effect that shows character has been hit
	public void playHitEffect() { 
	
	}

	//Runs when health reaches 0 in mobstats.cs
	private void _on_Stats_no_health()
	{
		QueueFree();
	}

	//If Player comes into line of sight
	private void _on_ViewCone_body_entered(Player body)
	{		
			GD.Print("I SEE YOU");
        
	}


}











