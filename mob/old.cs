using Godot;
using System;

public class old : mob
{
    private int copCount = 0;
    [Signal]
    public delegate void callCops();
    public Navigation2D parent;
    public PackedScene copScene;

    public override void _Ready()
    {
        stats = GetNode<mobStats>("Stats");
        detectionZone = GetNode<DetectionZone>("DetectionZone");
        detectionZone.Connect("give_direction", this, "give_direction");
        view_cone_box = GetNode<CollisionShape2D>("oldViewBox");
        view_cone = GetNode<viewCone>("oldViewBox/ViewCone");
        idleTimer = GetNode<Timer>("idleTimer");
        original_location_timer = GetNode<Timer>("originalLocationTimer");
        nav_agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        original_position = this.GlobalPosition;



    }



    //Make old beat up santa
    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    }
}
