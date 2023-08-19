using Godot;
using System;

public class npcSort : YSort
{
	Godot.Collections.Array children;
	private int copCount = 0;
	private Area2D spawn, exit;
	private PackedScene copScene;
	private Random rnd = new Random();
	private bool hardMode = false;

	Vector2 man_sees_player;

	[Signal]
	public delegate void endLevel();

	public override void _Ready()
	{

        spawn = GetNode<Area2D>("Spawn");
		//exit = GetNode<Area2D>("Exit");
		copScene = GD.Load<PackedScene>("res://mob/npcs/cop.tscn");
		children = this.GetChildren();
		for (int i = 0; i < children.Count; i++)
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
				//Set the cops mobs target location to where the woman or man mob sees the player
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
			//Set the cops original position / spawn
			copInstance.original_position = copInstance.GlobalPosition;

			//Sets target location to where player was last seen by a man or woman mob
			copInstance.can_move = true;
			copInstance.nav_agent.SetTargetLocation(man_sees_player);


			//Makes the cops move instantly
			copCount++;
		}
	}


    private void _player_exit(object player)
    {
		EmitSignal("endLevel");
    }



}



