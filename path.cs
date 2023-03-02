using Godot;
using System;

public class path : Path2D
{
    public PathFollow2D mob_path;
    public mob npc;
    public float offset;
    public bool move_route = true;
    public override void _Ready()
    {
        mob_path = GetNode<PathFollow2D>("PathFollow2D");

        //Allows any npc to be used in the tree
        string name = mob_path.GetChildren()[0].GetType().ToString();
        npc = GetNode<mob>($"PathFollow2D/{name}");
        npc.Connect("stop_route", this, "stop_route");
    }

    public void stop_route(bool move)
    {

        //Set the of npc position where they were last at only if move_route was true
        //This is when npc stops moving in the path and need to save its position for:
        //npc.original position
        if (move == false && move_route)
        {
            //Saves the npc position relative 
            npc.original_position = npc.GlobalPosition;
        }

        move_route = move;
        


    }

    //Need to get mob to roate cone in direction of where they way moving
    public override void _PhysicsProcess(float delta)
    {
 
        //Default movement when npc has not seen player
        if(move_route)
        {
            mob_path.Offset = mob_path.Offset + 100 * delta;
        }

    }

}
