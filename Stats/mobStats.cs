using Godot;
using System;

public class mobStats : Node
{
    private int max_health;

    [Export]
    public int Max_Health
    {
        get { return max_health; }
        set { max_health = value; }
    }

    private int health;

    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            if (health <= 0)
            {
                EmitSignal("no_health");
            }
        }
    }


    [Signal]
    public delegate void no_health();
    public override void _Ready()
    {
        Health = Max_Health;
    }



}
