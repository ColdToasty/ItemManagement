using Godot;
using Godot.Collections;
using System;

public class itemSelect : PanelContainer
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

    private static string playerItemImagesPaths = "res://Player/playerItemImages/";

    public override void _Ready()
    {
        InvisibilityCount = save_file_data["invisibilityCount"].ToString().ToInt();
        CaneCount = save_file_data["caneCount"].ToString().ToInt();
        TinselCount = save_file_data["tinselCount"].ToString().ToInt();
        OrnamentCount = save_file_data["ornamentCount"].ToString().ToInt();

        itemIcon = GetNode<TextureRect>("itemIcon");
        itemCount = GetNode<Label>("itemcount");
    }

    private void updatePanel(string itemName, int itemCount)
    {
        string imageName = playerItemImagesPaths + itemName;
        string imageType = ".png";
        string imagePath = "";

        //Update the ItemIcon.Texture
        //Do it via image pathway
        this.itemCount.Text = itemCount.ToString();
        
        //Change imageTexture to default
        if(itemCount > 0)
        {
            imagePath = imageName + imageType;
        }
        //Change imageTexture to imageNameNone
        else
        {
            imagePath = imageName + "None" + imageType;
        }
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("Ornament"))
        {
            updatePanel("Ornament", ornamentCount);
        }

        else if (Input.IsActionJustPressed("Tinsel"))
        {
            updatePanel("Tinsel", TinselCount);
        }

        else if (Input.IsActionJustPressed("Cane"))
        {
            updatePanel("Cane", CaneCount);
        }

        else if (Input.IsActionJustPressed("Invisibility"))
        {
            updatePanel("Invisibility", InvisibilityCount);
        }
    }
}
