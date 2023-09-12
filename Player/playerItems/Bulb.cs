using Godot;
using System;
using System.Threading;

public class Bulb : KinematicBody2D
{
	//Change this to be based on resource
	public float noiseTime = (float)2;
	public float waitTime = (float) 2; 

	private Godot.Timer waitTimer;
	private Godot.Timer noiseTimer;
	private CollisionShape2D noiseEmitArea;

	private float friction = 5;
	private float speed = 200;
	private float acceleration = 1000;
	private Vector2 mousePosition;
	private Vector2 direction;
	private Vector2 velocity = Vector2.Zero;
	private bool haveMousePosition = false;

    private bool waitTimerStarted = false; 
	public override void _Ready()
	{
		//Timers
		waitTimer = GetNode<Godot.Timer>("waitTimer");
		noiseTimer = GetNode<Godot.Timer>("noiseTimer");

		//CollisionShape for mobs to detect
		noiseEmitArea = GetNode<CollisionShape2D>("noiseArea/CollisionShape2D");
		noiseEmitArea.SetDeferred("disabled", true);
		//When instanced item will emit noise after certain time


    }



	private void _on_waitTimer_timeout()
	{
		GD.Print("asdfsd");
		//Enable the 'runNoise' Shape
		//change sprite to emit frame(s)
		noiseEmitArea.SetDeferred("disabled", false);
		noiseTimer.Start(noiseTime);
	}
	
	private void _on_noiseTimer_timeout()
	{
		//Disable the shape
		noiseEmitArea.SetDeferred("disabled", true);
		//DeQueue this
		QueueFree();
	}

    public override void _PhysicsProcess(float delta)
    {

        if (!haveMousePosition)
        {
            direction = this.GlobalPosition.DirectionTo(GetGlobalMousePosition());
            haveMousePosition = true;
        }

        if (speed > 0)
        {

            velocity = velocity.MoveToward(direction * speed, acceleration * delta * 60);
            speed -= friction;
            velocity = MoveAndSlide(velocity);
        }
		else
		{
            velocity = velocity.MoveToward(Vector2.Zero, acceleration * delta * 60);
        }

        if (velocity != Vector2.Zero)
		{
			velocity = MoveAndSlide(velocity);
		}
		else
		{
			if (!waitTimerStarted) {
                waitTimer.Start(waitTime);
				waitTimerStarted = true;
            }

        }

    }

}







