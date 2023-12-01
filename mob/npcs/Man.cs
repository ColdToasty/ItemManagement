using Godot;
using System;
using System.Security.AccessControl;

public class Man : Mob
{
	[Signal]
	public delegate void callCops();

    [Signal]
    public delegate void sendPlayerPosition(Vector2 player_position);

    Vector2 player_last_seen;
	Timer callCopTimer;
	bool copTimerStarted = false;


	AnimationTree animationTree;
	AnimationPlayer animationPlayer;
	AnimationNodeStateMachinePlayback animationPlayback;
	Vector2 lookDirection = Vector2.Zero;
	string animation = "";

	private enum LOOKDIRECTION
	{
		LEFT,RIGHT, UP, DOWN
	}

	//Should affect starting viewConePosition too
	[Export]
	LOOKDIRECTION startDirection;

	string startDirectionName;
	public override void _Ready()
	{
		
		base._Ready();
		callCopTimer = GetNode<Timer>("callCopTimer");

		animationTree = GetNode<AnimationTree>("AnimationTree");
		animationTree.Active = true;
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayback = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");

		startDirectionName = startDirection.ToString();
		setSpriteDirection();
    }


	private void setSpriteDirection()
	{
		Vector2 inputVector;
		switch (startDirectionName) {
			case "LEFT":
				inputVector = new Vector2(-1, 0);
                break;

            case "RIGHT":
                inputVector = new Vector2(1, 0);
                break;

            case "UP":
                inputVector = new Vector2(0, -1);
                break;

			default:
                inputVector = new Vector2(0, 1);
				break;
        }


        animationTree.Set("parameters/Idle/blend_position", inputVector);
        animation = "Idle";

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

		lookDirection = nav_agent.GetNextLocation().Normalized();
		GD.Print(lookDirection.ToString());
		//check if player is allowed to move
		//if allowed to move and moving
			//make mob play an animation based on lookDirection.x and lookDirection.y
		if(can_move)
		{

		}
    }
}



