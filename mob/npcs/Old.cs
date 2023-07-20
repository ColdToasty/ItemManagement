using Godot;
using System;

public class Old : Mob
{
    CollisionShape2D collisionShape;
    public override void _Ready()
    {
        base._Ready();
        collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");

    }



    //Make old beat up santa
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        
    }
    //Follows the player and hits them, slowing them while making noise
    //Makes others call for help?
    //If player gets hit 4 times, lose?
}
