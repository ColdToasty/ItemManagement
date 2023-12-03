using Godot;
using Godot.Collections;
using System;

public class itemSelect : CanvasLayer
{
    private Dictionary save_file_data = GameFiles.current_file_data;

    private int invisibilityCount { get; set; }
    public int InvisibilityCount
    {
        get { return invisibilityCount; }
        set { invisibilityCount = value; }
    }

    private int caneCount { get; set; }
    public int CaneCount
    {
        get { return caneCount; }
        set { caneCount = value; }
    }

    private int tinselCount { get; set; }
    public int TinselCount
    {
        get { return tinselCount; }
        set { tinselCount = value; }
    }

    private int ornamentCount { get; set; }
    public int OrnamentCount
    {
        get { return ornamentCount; }
        set { ornamentCount = value; }
    }

    private TextureRect itemIcon;
    private Label itemCount;

    private static string playerItemImagesPaths = "res://Player/playerItemImages/ItemIcon/";

    public bool itemAvailable = true;
    public enum ITEMSTATE
    {
        NONE,
        ORNAMENT,
        CANE,
        TINSEL,
        INVISIBILITY
    }

    private ITEMSTATE pickedItem = ITEMSTATE.NONE;

    public override void _Ready()
    {
        InvisibilityCount = save_file_data["invisibilityCount"].ToString().ToInt();
        CaneCount = save_file_data["caneCount"].ToString().ToInt();
        TinselCount = save_file_data["tinselCount"].ToString().ToInt();
        OrnamentCount = save_file_data["ornamentCount"].ToString().ToInt();

        itemIcon = GetNode<TextureRect>("itemIcon");
        itemCount = GetNode<Label>("itemCount");

    }

    private void updatePanel(string itemName, int itemCount)
    {
        string imageName = playerItemImagesPaths + itemName;
        string imageType = ".png";
        string imagePath = "";

        this.itemCount.Text = itemCount.ToString();
        
        //Change imageTexture to default
        if(itemCount > 0)
        {
            imagePath = imageName + imageType;
            itemAvailable = true;
            itemIcon.Modulate = new Color(1, 1, 1, (float).8);
        }
        else
        {
            imagePath = imageName + imageType;
            itemAvailable = false;
            itemIcon.Modulate = new Color(1, 1, 1, (float)0.5);
        }

        Texture texture = GD.Load<Texture>(imagePath);
        itemIcon.Texture = texture;
    }

    public override void _Process(float delta)
    {
        //Update to based on state
        if (Input.IsActionJustPressed("Ornament") )
        {
            pickedItem = ITEMSTATE.ORNAMENT;
            updatePanel("Ornament", OrnamentCount);
        }

        else if (Input.IsActionJustPressed("Tinsel"))
        {
            pickedItem = ITEMSTATE.TINSEL;
            updatePanel("Tinsel", TinselCount);
        }

        else if (Input.IsActionJustPressed("Cane"))
        {
            pickedItem = ITEMSTATE.CANE;
            updatePanel("Cane", CaneCount);
        }

        else if (Input.IsActionJustPressed("Invisibility"))
        {
            pickedItem = ITEMSTATE.INVISIBILITY;
            updatePanel("Invisibility", InvisibilityCount);
        }



    }
}
