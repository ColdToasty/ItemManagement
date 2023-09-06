using Godot;
using System;
using System.Data;

public class Baby : StaticBody2D
{

    private DetectionZone detectionzone;
    private Area2D alertArea;
    private CollisionShape2D alertAreaCollisionShape;
    private Area2D parentGather;
    [Signal]
    public delegate void gather_parents(Vector2 gatherPosition);
    
    
    public override void _Ready()
    {
        detectionzone = GetNode<DetectionZone>("DetectionZone");
        alertArea = GetNode<Area2D>("alertArea");
        alertAreaCollisionShape = GetNode<CollisionShape2D>("alertArea/CollisionShape2D");
        alertArea.Visible = false;
        alertAreaCollisionShape.SetDeferred("disabled", true);

        parentGather = GetNode<Area2D>("parentGather");
    }


    public override void _PhysicsProcess(float delta)
    {
        if (detectionzone.can_hear_player())
        {
            GD.Print("i hear player");
            alertArea.Visible = true;
            alertAreaCollisionShape.SetDeferred("disabled", false);
            EmitSignal("gather_parents", parentGather.GlobalPosition);
        }
        else
        {
            alertArea.Visible = false;
            alertAreaCollisionShape.SetDeferred("disabled", true);
        }
    }
}
