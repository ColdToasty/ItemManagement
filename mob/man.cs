using Godot;
using System;

public class man : mob
{
    int copCount = 0;
    [Signal]
    public delegate void callCops(int count);

    public override void _Ready()
    {
        stats = GetNode<mobStats>("Stats");
        detectionZone = GetNode<DetectionZone>("DetectionZone");
        detectionZone.Connect("give_direction", this, "give_direction");
        view_cone_box = GetNode<CollisionShape2D>("ViewBox"); 
        view_cone = GetNode<viewCone>("ViewBox/ViewCone");
        idleTimer = GetNode<Timer>("idleTimer");
        original_location_timer = GetNode<Timer>("originalLocationTimer");
        nav_agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        original_position = this.GlobalPosition;
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        if(player != null)
        {
            EmitSignal("callCops", copCount);
        }
    }
}
