using Godot;
using System;

public class Man : Mob
{
	[Signal]
	public delegate void callCops();

    [Signal]
    public delegate void sendPlayerPosition(Vector2 player_position);

    Vector2 player_last_seen;
	Timer callCopTimer;
	bool copTimerStarted = false;
	public override void _Ready()
	{
		base._Ready();
		callCopTimer = GetNode<Timer>("callCopTimer");
	}


    private void _on_callCopTimer_timeout()
    {
        EmitSignal("callCops");
    }


    public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);
		if(player != null)
		{
			player_last_seen = player.GlobalPosition;
			if (!copTimerStarted)
			{
                copTimerStarted = true;
                callCopTimer.Start(2);
			}
			EmitSignal("sendPlayerPosition", player_last_seen);
		}
	}
}



