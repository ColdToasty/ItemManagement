using Godot;
using System;

public class Slots : Panel
{

    public PackedScene itemScene; 
    public Item inventoryItem = null;
    public Texture itemPresent;
    public Texture noItem;

    public StyleBoxTexture default_style;
    public StyleBoxTexture item_style;

    [Signal]
    public delegate void pickUpItem(InputEvent e, Slots s);

    public override void _Ready()
    {    
        //Load texture for panels
        itemPresent = (Texture)GD.Load("res://Hotbar/ItemPresentOutline.png");
        noItem = (Texture)GD.Load("res://Hotbar/ItemFrameOutline.png");

        default_style = new StyleBoxTexture();
        item_style = new StyleBoxTexture();

        
        default_style.Texture = noItem;
        item_style.Texture = itemPresent;

        if(GD.Randi() % 3 == 0){
            itemScene = ResourceLoader.Load<PackedScene>("res://Inventory/Item.tscn");
            inventoryItem = (Item)itemScene.Instance();
            inventoryItem.Position = new Vector2(2,2);
            AddChild(inventoryItem);
            }
        Set("custom_styles/panel", default_style);
        }



    public void refresh_style(){
        //If item is not added then go default style
        if(inventoryItem == null){
            Set("custom_styles/panel", default_style);
        }
        //If item is added 
        else{
            Set("custom_styles/panel", item_style);
        }
    }

    public void pickFromSlot(){
        RemoveChild(inventoryItem);
        Node2D inventory = (Node2D)FindParent("PlayerMenu");
        inventory.AddChild(inventoryItem);
        inventoryItem = null;
  
    }

    public void PutIntoSlot(Item new_item){
        inventoryItem = new_item;
        inventoryItem.Position = new Vector2(2,2);
        Node2D inventory = (Node2D)FindParent("PlayerMenu");
        inventory.RemoveChild(inventoryItem);
        AddChild(inventoryItem);
       
    }

    public override void _GuiInput(InputEvent @event)
    {
        EmitSignal("pickUpItem", @event, this);
    }
}
