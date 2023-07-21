using Godot;
using System;

public class Old : Mob
{
	Area2D hitPlayerArea;
	CollisionShape2D hitPlayerBox;
	Timer hitTimer;
    bool hitTimerStarted = false;
    bool controlPlayerBox = false;

    public override void _Ready()
	{
		base._Ready();
		hitPlayerArea = GetNode<hitPlayerBox>("hitPlayerBox");
		hitPlayerBox = GetNode<CollisionShape2D>("hitPlayerBox/hitPlayer");
        hitTimer = GetNode<Timer>("hitTimer");

    }


    private void _on_hitPlayerBox_area_entered(PlayerVisible area)
    {
        //Only runs if a timer has not started
        if (!hitTimerStarted)
        {
            controlPlayerBox = true;
            hitTimer.Start(2);
            hitPlayerBox.SetDeferred("disabled", true);
            hitTimerStarted = true;
        }


        //Disable the hitPlayerBox then reactivate after timer

    }

    private void _on_hitTimer_timeout()
    {
        hitPlayerBox.SetDeferred("disabled", false);
        //Disables the physics process where it enables the hitPlayerBox
        controlPlayerBox = false;
        //Allows the timer to commence again
        hitTimerStarted = false;
    }


    public override void _PhysicsProcess(float delta)
	{
		base._PhysicsProcess(delta);
        if (!controlPlayerBox)
        {
            if (player != null)
            {
                hitPlayerBox.SetDeferred("disabled", false);
            }
            else
            {
                hitPlayerBox.SetDeferred("disabled", true);
            }
        }


	}
	//Follows the player and hits them, slowing them while making noise
	//Makes others call for help?
	//If player gets hit 4 times, lose?
}






