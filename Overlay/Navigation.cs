using Godot;
using System;

public class Navigation : Navigation2D
{
    Man manNpc;
    Godot.Collections.Array children;
    int copCount = 0;
    Area2D spawn, exit;
    private PackedScene copScene;
    Random rnd = new Random();
    bool hardMode = false;

    Vector2 man_sees_player;

    public override void _Ready()
    {
        spawn = GetNode<Area2D>("Spawn");
        exit = GetNode<Area2D>("Exit");
        copScene = GD.Load<PackedScene>("res://mob/npcs/cop.tscn");
        children = this.GetChildren();
        for(int i = 0; i< children.Count; i++)
        {
            if (children[i] is Man)
            {
                ((Man)children[i]).Connect("callCops", this, "spawnCops");
                ((Man)children[i]).Connect("sendPlayerPosition", this, "giveCopsLocation");
            }
        }

    }

    public void giveCopsLocation(Vector2 player_position)
    {
        this.man_sees_player = player_position;
        
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i] is Cop)
            {
                ((Cop)(children[i])).nav_agent.SetTargetLocation(this.man_sees_player);
                ((Cop)(children[i])).can_move = true;
            }
        }
    }

    public void spawnCops()
    {
        while (copCount != 2)
        {
            Cop copInstance = (Cop)copScene.Instance();

            this.AddChild(copInstance);
            children = this.GetChildren();
            Vector2 spawnLocationRnd = new Vector2(rnd.Next(-70, 80), rnd.Next(-70, 80));
            
            //Spawns cops near spawn
            copInstance.GlobalPosition = spawn.GlobalPosition + spawnLocationRnd;
            copInstance.spawnPosition = spawn.GlobalPosition + spawnLocationRnd;
            //Set the cops original position 
            copInstance.original_position = copInstance.GlobalPosition;
            //Makes the cops move instantly
            copCount++;
        }
    }
}
