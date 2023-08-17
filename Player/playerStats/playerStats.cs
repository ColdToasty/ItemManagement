using Godot;
using System;
using System.Drawing.Design;

public class playerStats : Resource
{
    [Export]
    public int Health { get; set; }

    [Export]
    public int Speed
    {
        get;
        set;
    }

    [Export]
    public int SprintSpeed
    {
        get;
        set;
    }

    [Export]
    public float PlayerItemRadius { get; set; }

    [Export]
    public float PlayerItemHeight { get; set; }

    [Export]
    public float playerVisibleRadius { get; set; }

    [Export]
    public float playerVisibleHeight { get; set; }


    [Export]
    public float runNoiseRadius { get; set; }

    [Export]
    public float runNoiseHeight { get; set; }

    public float positionX { get; set; }
    public float positionY { get; set; }

}
