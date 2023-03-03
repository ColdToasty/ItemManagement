using Godot;
using System;

public class DetectionZone : Area2D
{
	public Player player;

	public bool made_noise = false;

	//If player is seen
	//Will return true

	public Vector2 last_heard;


	public bool can_see_player()
	{
		return player != null;
	}

	public bool can_hear_player()
	{
		return last_heard != null;
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


	public void set_player(Player p)
    {
		if(p.SPEED >= 400)
        {
			last_heard = p.GlobalPosition;
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






