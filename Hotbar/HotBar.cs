using Godot;
using System;

public class HotBar : GridContainer
{
    public GridContainer hotbar;
    public override void _Ready()
    {
        hotbar = this;
    }


}
