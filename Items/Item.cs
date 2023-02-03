using Godot;
using System;




public class Item : KinematicBody2D
{

	private Vector2 move_vector = Vector2.Zero;
	private Vector2 velocity = Vector2.Zero;
	private int speed = 500;
	private int Acceleration = 500;
    private ItemZone itemZone;
	private PickupZone pickupZone;

	[Signal]
	public delegate void ItemPickedUp();


	public override void _Ready(){
		itemZone = GetNode<ItemZone>("ItemZone");
	}

	//Move the item towards the center of the pickupzone
	public override void _PhysicsProcess(float delta){

		pickupZone = itemZone.PickupZone;

		if(pickupZone != null){
			Vector2 direction = GlobalPosition.DirectionTo(pickupZone.GlobalPosition);
			velocity = velocity.MoveToward(direction * speed, Acceleration * delta);
		}
		else{
			velocity = velocity.MoveToward(Vector2.Zero * speed, Acceleration * delta);
		}
		velocity = MoveAndSlide(velocity);	
		}
	

	//When Item touches character sprite
	private void _on_ItemPickedUp_area_entered(object area)
	{
		EmitSignal("ItemPickedUp");
		QueueFree();
	}


	}



