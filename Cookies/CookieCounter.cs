using Godot;
using System;

public class CookieCounter : Node
{
    private int cookie_counter;

    public int Cookie_counter
    {
        get { return cookie_counter; }
        set
        {
            cookie_counter = value;
        }
    }
    public override void _Ready()
    {
        Cookie_counter = 0;
    }


}
