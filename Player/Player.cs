using Godot;
using System;

public class Player : KinematicBody2D
{
    public enum STATE
    {
        MOVING
    }

    public int SPEED = 400;
    public STATE state = STATE.MOVING;
    public Vector2 input_vector = Vector2.Zero;
    public Vector2 velocity = Vector2.Zero;

    public GridContainer hotbar;
    public GridContainer inventory_slots;


    public override void _Ready()
    {

    }


    public void move()
    {
        velocity = MoveAndSlide(velocity);
    }

    public override void _PhysicsProcess(float delta)
    {
        input_vector.x = Input.GetActionStrength("Right") - Input.GetActionStrength("Left");
        input_vector.y = Input.GetActionStrength("Down") - Input.GetActionStrength("Up");
        input_vector = input_vector.Normalized();
        if(input_vector != Vector2.Zero)
        {
            //character can move when input vector != 0,0
            if (state == STATE.MOVING)
            {
                velocity = input_vector * SPEED;
                move();
            }

        }
        else
        {
            velocity = Vector2.Zero;
        }
        
    }
}
