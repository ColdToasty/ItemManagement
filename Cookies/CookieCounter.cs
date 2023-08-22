using Godot;
using System;


/*
 * Level_Cookie_Counter tracks the amount of cookies eaten before the player has left the level
 * this sets the level_cookie_counter variable and should be saved to the temp level save file
 *
 *
 * cookie_counter is a private variable which is hidden 
 * and is only changed when Global_cookie_counter is changed and ideally should only be saved
 * when player has exited the level
 * 
 * 
 * set_global_cookie_counter should only be called when player has exited the level
 */
public class CookieCounter : Node
{
    private int cookie_counter;
    private int level_cookie_counter;

    
    public int Global_Cookie_Counter
    {
        get { return cookie_counter; }
        set
        {
            cookie_counter = value;
        }
    }

    public int Level_Cookie_Counter
    {
        get { return level_cookie_counter; }
        set
        {
            level_cookie_counter = value;
        }
    }

    public override void _Ready()
    {
        //Set cookie_counter based on save
        Global_Cookie_Counter = 0;
        Level_Cookie_Counter = 0;
    }

    public void Save_Temp_Cookies()
    {
        //Save the cookies when a checkpoint is reached
    }

    //Make temp save file for scene.
    //Make actual save when player leaves
    public void Save_global_cookies()
    {
        Global_Cookie_Counter += Level_Cookie_Counter;
        //Save global cookies
    }

}
