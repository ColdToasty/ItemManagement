using Godot;
using System;
using System.Drawing.Drawing2D;

public class Tinsel : KinematicBody2D
{
	//Also make it upgradable
	public float tinselHoldTime = (float)1.50;
    private Vector2 directionToMouse;
    private Sprite tinselSprite;


    private float acceleration = 100;
    private Vector2 velocity = Vector2.Zero;
    private float speed = 100;
    private float friction = 2;

    private Vector2 direction
        ;
    private bool haveMousePosition = false;
    public override void _Ready()
    {
        tinselSprite = GetNode<Sprite>("Sprite");

    }

    //Updated every frame
    public override void _PhysicsProcess(float delta)
    {
        if (!haveMousePosition)
        {
            direction = this.GlobalPosition.DirectionTo(GetGlobalMousePosition());
            haveMousePosition = true;
        }

        if (speed > 0) { 

            velocity = velocity.MoveToward(direction * speed, acceleration * delta * 60);
            speed -= friction;
            velocity = MoveAndSlide(velocity);
        }


        //directionToMouse = tinselSprite.GlobalPosition.DirectionTo(GetGlobalMousePosition());
        //update position
        //postion' = position + delta * velocity
        //velocity' = velocity + delta * gravity
        //delta
    }

}
