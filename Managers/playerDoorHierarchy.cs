using Godot;
using System;

public class playerDoorHierarchy : YSort
{
    Player player;
    int playerIndex;

    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
        
  }



/*
        Give doors an area2D that detects the player or playerBase (this area should be ideally on upper part of door) and emits a signal
        When the player/playerBase enters this area2d swap the y sort between the player and that door (put player behind the door)
        When player/playerBase leaves the area2D, swap the door and player back (player is infront of the door)  
*/
}
