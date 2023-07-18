using Godot;
using System;
using System.Collections.Generic;

public class ItemDatabase : Node
{
    public List<ItemInfo> items = new List<ItemInfo>();

    

    public override void _Ready()
    {
        //Open the path name
        Directory directory = new Directory();
        directory.Open("res://ItemData/");
        directory.ListDirBegin();

        while (true)
        {
            string filename = directory.GetNext();
            if (filename == "")
            {
                break;
            }
            else if (!filename.BeginsWith("."))
            {
                //Adds resource to list
                items.Add((ItemInfo)GD.Load<Resource>($"res://ItemData/{filename}"));
                
            }
        }

    }


    public void assignTexture()
    {

    }
    //Returns the whole list of item objects from ItemData
    public int get_list_length()
    {
        return items.Count;
    }


    








//Last bracket
}
