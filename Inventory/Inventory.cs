using Godot;
using System;

public class Inventory : Node2D
{
    //Hotbar variables
    public HotbarScene hotbarscene;
    public GridContainer hotbar;
    public TextureRect hotbarSceneTextureRect;
    public Vector2 inventory_hotbar_position = new Vector2(15, 40);


    //This inventory variables
    public GridContainer inventory_slots;
    private TextureRect inventory_background;

    public Item holding_item = null;



    //Toggle elements
    public bool show_inventory = true;
    public bool show_hotBar = true;


    public override void _Ready()
    {
        inventory_slots = GetNode<GridContainer>("InventorySpace");
        inventory_background = GetNode<TextureRect>("TextureRect");


        hotbarscene = GetNode<HotbarScene>("/root/HotbarScene");

        hotbarSceneTextureRect = hotbarscene.textureRect;

        hotbarscene.RemoveChild(hotbarSceneTextureRect);
        AddChild(hotbarSceneTextureRect);
        hotbarSceneTextureRect.Visible = false;

        hotbar = hotbarscene.hotbar;


        hotbarscene.RemoveChild(hotbar);
        AddChild(hotbar);




        change_hotbar_position(inventory_hotbar_position);


        foreach (Slots slot in inventory_slots.GetChildren())
        {
            slot.Connect("pickUpItem", this, "slot_gui_input");
        }
        foreach (Slots slot in hotbar.GetChildren())
        {
            slot.Connect("pickUpItem", this, "slot_gui_input");
        }



    }



    public void change_hotbar_position(Vector2 position)
    {
        hotbar.RectPosition = position;
        hotbarSceneTextureRect.RectPosition = new Vector2(position.x - 5, position.y - 5);

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
                        Item tempItem = slot.inventoryItem;
                        slot.pickFromSlot();
                        tempItem.Position = new Vector2(GetGlobalMousePosition().x - 12, GetGlobalMousePosition().y - 12);
                        slot.PutIntoSlot(holding_item);
                        holding_item = tempItem;
                    }
                }
                //If we want to pick up item with nothing in "hand"
                else if (slot.inventoryItem != null)
                {
                    holding_item = slot.inventoryItem;
                    slot.pickFromSlot();
                    holding_item.GlobalPosition = new Vector2(GetGlobalMousePosition().x - 12, GetGlobalMousePosition().y - 12);
                }


            }


        }

    }

    //show/Hide inventory
    //Also changes the hotbar textures to show it by itself
    private void toggle_inventory()
    {
        //change show_inventory into opposite
        show_inventory = !show_inventory;
        hide_show_inventory();
    }

    private void hide_show_inventory()
    {
        //Show inventory with hotbar
        if (show_inventory)
        {
            inventory_slots.Visible = true;
            inventory_background.Visible = true;
            hotbarSceneTextureRect.Visible = false;

        }
        //Hide inventory and make hotbar isolated
        else
        {
            inventory_slots.Visible = false;
            inventory_background.Visible = false;
            hotbarSceneTextureRect.Visible = true;

        }
        //Move the inventory depending on show_inventory boolean
        change_inventory_position(show_inventory);
    }



    public override void _Input(InputEvent @event)
    {
        if (holding_item != null)
        {
            holding_item.GlobalPosition = new Vector2(GetGlobalMousePosition().x - 12, GetGlobalMousePosition().y - 12);
        }
        //Means user has pressed a key 
        if (@event is InputEventKey)
        {
            //Go to show inventory method
            if (Input.IsActionJustPressed("Inventory_toggle"))
            {
                toggle_inventory();
            }
        }


    }

}

