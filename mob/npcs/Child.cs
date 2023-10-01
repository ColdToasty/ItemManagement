using Godot;
using System;

public class Child : Mob
{
    //Signal to alert objectSort observer to set navigateToPosition
    [Signal]
    public delegate void get_parents(Vector2 childPosition, Vector2 playerPosition);

    //Signal when child reaches parent
    [Signal]
    public delegate void alert_parent(Vector2 player_position);
    private bool findingParent = false;

    private CollisionShape2D alertAreaCollisionShape;



    public override void _Ready()
    {
        base._Ready();
        alertAreaCollisionShape = GetNode<CollisionShape2D>("alertArea/CollisionShape2D");
        alertAreaCollisionShape.SetDeferred("disabled", true);


    }

    new public void navigateToPosition(float delta)
    {
        //If mob hasnt arrived at target location
        if (!arrived_at_location() && can_move)
        {
            idleTimer.Stop();
            original_location_timer.Stop();

            rotate_cone(delta, nav_agent.GetNextLocation());

            //Make it so that when player is heard stop movement and follow 185
            move();


        }
        //If back to original position
        else if (arrived_at_location() && can_move)
        {
            can_move = false;
            timerStarted = false;

            //Means we need to navigate back to the original position 
            if (nav_agent.GetTargetLocation() != original_position)
            {
                alertAreaCollisionShape.SetDeferred("disabled", true);
                original_location_timer.Start(rnd.Next(4, 7));

            }
            else
            {
                seenPlayer= false;
                investigatingPosition = false;
            }
        }
    }


    public override void _PhysicsProcess(float delta)
    {
        //If the viewcone sees the player
        if (view_cone.can_see_player())
        {
            //Stop the route, if any
            EmitSignal("stop_route", false);
            seenPlayer = true;

            player = view_cone.player;
            idleTimer.Stop();
            original_location_timer.Stop();

            can_move = true;

            //GD.Print("sees player, get adult");
            EmitSignal("can_player_hide", false);

            //if sees player then find closest "adult"
            //send signal 
            EmitSignal("get_parents", this.GlobalPosition, player.GlobalPosition);

            //Set to closest parent
            nav_agent.SetTargetLocation(((ObjectSort)this.GetParent()).closestParentPosition);
            //Rotate the cone towards the player
            rotate_cone(delta, ((ObjectSort)this.GetParent()).closestParentPosition);
            //Activate area
            alertAreaCollisionShape.SetDeferred("disabled", false);

            //play red view cone animation
            investigatingPosition = true;

        }
        //Check detection zone
        //if hears player investigate
        else if (detectionZone.can_hear_player() && !seenPlayer)
        {
            timerStarted = false;
            idleTimer.Stop();
            original_location_timer.Stop();
            EmitSignal("stop_route", false);

            rotate_cone(delta, detectionZone.last_heard);

            nav_agent.SetTargetLocation(detectionZone.last_heard);
            EmitSignal("can_player_hide", true);

            investigatingPosition = true;
            if (!timerStarted)
            {
                timerStarted = true;
                idleTimer.Start(1);
            }
            alertAreaCollisionShape.SetDeferred("disabled", false);
        }
        else
        {
            EmitSignal("can_player_hide", true);
            player = null;
            alertAreaCollisionShape.SetDeferred("disabled", false);

        }


        navigateToPosition(delta);


    }
}
