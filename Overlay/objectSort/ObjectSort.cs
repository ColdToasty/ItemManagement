using Godot;
using System;
using System.Collections.Generic;
using static Mob;


/*
 Observer class that:
	- spawns cops and allows them to follow the callers player position
	- allows the player to hid in hiding spots
	- Notifies the World class that the player has left the area
 */
public class ObjectSort : YSort
{
	Godot.Collections.Array children;
	private int copCount = 0;
	private Area2D spawn, exit;
	private PackedScene copScene;
	private Random rnd = new Random();
	private Player player;
	private Vector2 man_sees_player;
	private bool canPlayerHide = true;

	private bool canExit = false;
	private bool hasLevelEnded = false;
	public Vector2 closestParentPosition = Vector2.Zero;
	public Vector2 playerPosition = Vector2.Zero;
    public Vector2 gatherPosition = Vector2.Zero;
	[Signal]
	public delegate void end_level();

	public override void _Ready()
	{

        spawn = GetNode<Area2D>("Spawn");
		//exit = GetNode<Area2D>("Exit");
		copScene = GD.Load<PackedScene>("res://mob/npcs/cop.tscn");
		children = this.GetChildren();
		for (int i = 0; i < children.Count; i++)
		{
			if (children[i] is Mob)
			{
                ((Mob)children[i]).Connect("can_player_hide", this, "playerCanHide");
                
				if (children[i] is Man)
                {
                    ((Man)children[i]).Connect("callCops", this, "spawnCops");
                    ((Man)children[i]).Connect("sendPlayerPosition", this, "giveCopsLocation");
                }
            }
			if (children[i] is HidingSpot)
			{
                
                ((HidingSpot)children[i]).Connect("hide_the_player_in_me", this, "hideThePlayerInMe");
                ((HidingSpot)children[i]).Connect("show_the_player", this, "showThePlayer");
            }
			if (children[i] is Player)
			{
				player = (Player)children[i];
			}
			if (children[i] is Child)
			{
                ((Child)children[i]).Connect("get_parents", this, "getParents");
            }
            if (children[i] is Baby)
            {
                ((Baby)children[i]).Connect("gather_parents", this, "gatherParents");
            }

        }

	}
	
	public void gatherParents(Vector2 gatherPosition)
	{
		this.gatherPosition = gatherPosition;

    }
    public void getParents(Vector2 childPosition, Vector2 playerPosition)
	{
		this.playerPosition = playerPosition;
        Stack<double> magnitude = new Stack<double>();
        //Check which parent is closest
        //Check the magnitude of vectors

        for (int i = 0; i < children.Count; i++)
		{
			if (children[i] is Man || children[i] is Cop || children[i] is Old)
			{
				Vector2 adultPosition = ((Mob)children[i]).GlobalPosition;
				//Calculate x
				double xPoint = Math.Pow(adultPosition.x - childPosition.x, 2);
                //Calculate y
				double yPoint = Math.Pow(adultPosition.y - childPosition.y,  2);
				
				//Get Magnitude of vector
				double adultMagnitude = Math.Sqrt(xPoint + yPoint);

				if(magnitude.Count == 0)
				{
					magnitude.Push(adultMagnitude);
					closestParentPosition = adultPosition;
				}
				else
				{
					if(magnitude.Peek() >= adultMagnitude)
					{
						magnitude.Pop();
						magnitude.Push(adultMagnitude);
						closestParentPosition = adultPosition;
					}
				}
			}
		}
		

	}

	public void playerCanHide(bool hidingPossible)
	{
        canPlayerHide = hidingPossible;

    }


	public void hideThePlayerInMe(Vector2 hidingSpotPosition)
	{
		//Change the players position to the hidding spot (ideally its in the center of the object)
		
		if (canPlayerHide)
		{
			GD.Print("invisible");
			player.enableInvisibility();
        }
		player.disableSprite();
        player.GlobalPosition = hidingSpotPosition;

    }

	public void showThePlayer(Vector2 releasePosition)
	{
        player.disableInvisibility();
		player.enableSprite();
		player.GlobalPosition = releasePosition;
		//Disable players movements
        //Release the player 
        //But where and how would it know where to release?
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
		if (!hasLevelEnded)
		{
            EmitSignal("end_level");
			hasLevelEnded = true;
			//Now stop processors of all children
        }
    }



}



