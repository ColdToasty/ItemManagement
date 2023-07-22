using Godot;
using System;

public class playerStats : Node2D
{
    //Health
    //PlayerReach
    //Speed

    //PlayerSlap?
    //RunNoise?

    //Have stats written in external file
        //Have three files. An Original file and 2 copies 
            //Two copies being the current and other being a previous 
            //Use current to initalize stats
            //previous will equal current when player has clicked saved stats

        //Allow player to undo an upgrade if not saved
            //When saved overrites the current and sets previous == current

        //Cost to upgrade uses cookies 


    Player player;
    public override void _Ready()
    {
        player = this.GetParent<Player>();
    }

}
