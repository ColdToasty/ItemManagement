using Godot;
using System;

public class Bulb : KinematicBody2D
{
	//Change this to be based on resource
	public float noiseTime = (float)2;
	public float waitTime = (float) 2; 

	private Timer waitTimer;
	private Timer noiseTimer;
	private CollisionShape2D noiseEmitArea;
	public override void _Ready()
	{
		//Timers
		waitTimer = GetNode<Timer>("waitTimer");
		noiseTimer = GetNode<Timer>("noiseTimer");

		//CollisionShape for mobs to detect
		noiseEmitArea = GetNode<CollisionShape2D>("noiseArea/CollisionShape2D");
		noiseEmitArea.SetDeferred("disabled", true);
		//When instanced item will emit noise after certain time
		waitTimer.Start(waitTime);
	}




	private void _on_waitTimer_timeout()
	{
		GD.Print("asdfsd");
		//Enable the 'runNoise' Shape
		noiseEmitArea.SetDeferred("disabled", false);
		noiseTimer.Start(noiseTime);
	}
	
	private void _on_noiseTimer_timeout()
	{
		//Disable the shape
		noiseEmitArea.SetDeferred("disabled", true);
		//DeQueue this
	}

}







