using Godot;
using System;

public class DetectionZone : Area2D
{
	public Player player;

	public bool made_noise = false;

	//If player is seen
	//Will return true

	public Vector2 last_heard;
	public Timer timer;

	private Random rnd = new Random();


	[Signal]
	public delegate void give_direction(Vector2 last_position);

	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
	}

	public bool can_see_player()
	{
		return player != null;
	}

	public bool can_hear_player()
	{
		return last_heard != Vector2.Zero;
	}

	public override void _PhysicsProcess(float delta)
	{
		if(GetOverlappingAreas().Count != 0)
		{
			RunNoise r = (RunNoise) GetOverlappingAreas()[0];
			Player p = r.GetParent<Player>();
			set_player(p);

		}
	}

	//Reset the last_heard to Vector2.Zero
	private void _on_Timer_timeout()
	{
		EmitSignal("give_direction", last_heard);
		last_heard = Vector2.Zero;
	}


	public void set_player(Player p)
	{
		if(p.SPEED >= 400)
		{
			last_heard = p.GlobalPosition;
			timer.Start(rnd.Next(1));

		}
	}


	private void _on_noise_area_exited(RunNoise area)
	{
		player = null;
	}


	private void _on_noise_area_entered(RunNoise area)
	{
		Player p = area.GetParent<Player>();
		set_player(p);

	}


}







