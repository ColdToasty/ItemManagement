using Godot;
using System;
using System.Drawing.Design;

public class playerStats : Resource
{

    private int latestLevel = 1;
    private int currentLevel = 1;

    public int LatestLevel
    {
        get
        {
            return latestLevel;
        }
        set
        {
            latestLevel = value;
        }
    }

    public int CurrentLevel
    {
        get
        {
            return currentLevel;

        }
        set
        {
            currentLevel = value;
        }
    }

    [Export]
    public int health { get; set; }

    [Export]
    public int speed
    {
        get; set;
    }

    [Export]
    public int sprintSpeed
    {
        get; set;
    }

    [Export]
    public int ornamentCount{ get; set; }

    [Export]
    public int ornamentMaxDistance { get; set; }

    [Export]
    public int tinselCount{ get; set; }


    [Export]
    public int tinselMaxDistance { get; set; }

    [Export]
    public int caneCount{ get; set; }

    [Export]
    public int invisibilityCount{ get; set; }

    [Export]
    public int invisibilityTime { get; set; }

    [Export]
    public float reachX { get; set; }

    [Export]
    public float reachY { get; set; }

    [Export]
    public float playerVisibleRadius { get; set; }

    [Export]
    public float playerVisibleHeight { get; set; }


    [Export]
    public float runNoiseRadius { get; set; }

    public float positionX { get; set; }
    public float positionY { get; set; }

}
