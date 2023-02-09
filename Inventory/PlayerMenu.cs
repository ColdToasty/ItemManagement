using Godot;
using System;

public class PlayerMenu : Node2D
{
	//Hotbar variables
	public GridContainer hotbar;
	public Vector2 inventory_hotbar_position = new Vector2(7, 40);


	//This inventory variables
	public GridContainer inventory_slots;

	public Item holding_item = null;
	public Vector2 holding_item_position;
	public Slots previous_slot;
		
		
		//Toggle elements



	public override void _Ready()
	{
		inventory_slots = GetNode<GridContainer>("InventorySpace");
		hotbar = GetNode<GridContainer>("HotBar");

		foreach (Slots slot in inventory_slots.GetChildren())
		{
			slot.Connect("pickUpItem", this, "slot_gui_input");
		}
		foreach (Slots slot in hotbar.GetChildren())
		{
			slot.Connect("pickUpItem", this, "slot_gui_input");
		}



	}


	//Change the inventory_position depending on boolean parameter
	public void change_inventory_position(bool show_inventory)
	{
		//inventory is shown in center of character
		if (show_inventory)
		{

		}
		//Just hotbar is shown
		//Inventory is hidden
		//Hotbar is placed at the bottom
		else
		{

		}
	}


	private void _on_InventoryButton_pressed()
	{
		// Replace with function body.
	}


	private void _on_Map_pressed()
	{
		// Replace with function body.
	}


	public void slot_gui_input(InputEventMouseButton e, Slots slot)
	{
		if (e is InputEventMouseButton)
		{
			if (e.ButtonIndex == (int)ButtonList.Left && e.Pressed)
			{
				//If we are holding on an item
				if (holding_item != null)
				{
					//If the location we want to place our item into is empty
					if (slot.inventoryItem == null)
					{
						slot.PutIntoSlot(holding_item);
						holding_item = null;
					}
					//If an item is already present
					else if (slot.inventoryItem != null)
					{
						Item temp_item = slot.inventoryItem;
						slot.pickFromSlot();
						temp_item.Position = new Vector2(GetGlobalMousePosition().x - 12, GetGlobalMousePosition().y - 12);
						slot.PutIntoSlot(holding_item);
						holding_item = temp_item;
					}
				}
				//If we want to pick up item with nothing in "hand"
				else if (slot.inventoryItem != null)
				{
					holding_item = slot.inventoryItem;

					//Remember the slot
					previous_slot = slot; 

					holding_item_position = slot.inventoryItem.Position;
					slot.pickFromSlot();
					holding_item.GlobalPosition = new Vector2(GetGlobalMousePosition().x - 12, GetGlobalMousePosition().y - 12);
				}


			}


		}

	}


	public override void _Input(InputEvent @event)
	{
		if (holding_item != null)
		{
			holding_item.GlobalPosition = new Vector2(GetGlobalMousePosition().x - 12, GetGlobalMousePosition().y - 12);
		}
		//When Key I/i is pressed 
		if (Input.IsActionJustPressed("Inventory_toggle")){
			//Holding an item and inventory is pressed
			if(holding_item != null) {
				//Use the slot where the item was picked from
				//Add the item back
				previous_slot.PutIntoSlot(holding_item);
				holding_item = null;
			}
			this.Visible = !this.Visible;
        }
	}

}




