using Godot;
using System;

public class Inventory : Node2D
{
    public GridContainer inventory_slots;

     public Item holding_item = null;
    public override void _Ready()
    {
        inventory_slots = GetNode<GridContainer>("GridContainer");
        foreach(Slots slot in inventory_slots.GetChildren()){
            slot.Connect("pickUpItem", this, "slot_gui_input");
        }
    }
    

    public void slot_gui_input(InputEventMouseButton e, Slots slot){
        if(e is InputEventMouseButton){
            if(e.ButtonIndex == (int)ButtonList.Left && e.Pressed){
                //If we are holding on an item
                if(holding_item != null){
                    //If the location we want to place our item into is empty
                    if(slot.inventoryItem == null){
                        slot.PutIntoSlot(holding_item);
                        holding_item = null;
                    }
                    //If an item is already present
                    else if (slot.inventoryItem != null){
                        Item tempItem = slot.inventoryItem;
                        slot.pickFromSlot();
                        tempItem.Position = new Vector2(GetGlobalMousePosition().x-12,GetGlobalMousePosition().y-12);
                        slot.PutIntoSlot(holding_item);
                        holding_item = tempItem;
                }
                }
                //If we want to pick up item with nothing in "hand"
                else if (slot.inventoryItem != null){
                    holding_item = slot.inventoryItem;
                    slot.pickFromSlot();
                    holding_item.GlobalPosition = new Vector2(GetGlobalMousePosition().x-12,GetGlobalMousePosition().y-12);
                }


            }


        }
    }


    public override void _Input(InputEvent @event)
    {
        if(holding_item != null){
            holding_item.GlobalPosition = new Vector2(GetGlobalMousePosition().x-12,GetGlobalMousePosition().y-12);
        }
    }
}
