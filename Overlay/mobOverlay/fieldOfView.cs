using Godot;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

public class fieldOfView : RayCast2D
{
	[Export]
	public float angle;
    public float rayCastLength;
    public float leftAngle, rightAngle;

    public Godot.Vector2 leftLine, rightLine;


    [Export]
	public Color color = new Color(255,0,255,255);
    public override void _Ready()
	{

    }


    public override void _PhysicsProcess(float delta)
    {

    }


    public override void _Draw()
    {



    }

}



