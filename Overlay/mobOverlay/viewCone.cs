using Godot;
using System;

public class viewCone : Area2D
{
	public Player player;
	public Node2D raycast_nodes;
	public bool seen_player = false;
	public bool investigating = false;


	
	public override void _Ready()
	{
		raycast_nodes = GetNode<Node2D>("CollisionPolygon2D");

	}


	public bool can_see_player()
	{
		return player != null;
	}



	public override void _PhysicsProcess(float delta)
	{

	
		//Check each ray
		foreach (RayCast2D ray in raycast_nodes.GetChildren())
		{

			//If at least 1 ray cast can see the player visible area
			if(ray.GetCollider() is PlayerVisible)
			{
				
				PlayerVisible playerVisible = (PlayerVisible)ray.GetCollider();
				player = (Player)playerVisible.GetParent();
				break;
			}
			player = null;
		}
	}

private void _on_FieldOfView_player_enter()
{
		GD.Print("pklayerender");
}


private void _on_FieldOfView_player_exit()
{
	// Replace with function body.
}


}






