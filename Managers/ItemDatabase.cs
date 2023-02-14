using Godot;
using System;
using System.Collections.Generic;

public class ItemDatabase : Node
{
    public List<Resource> items = new List<Resource>();

    public override void _Ready()
    {
        //Open the path name
        Directory directory = new Directory();
        directory.Open("res://ItemData");

        string filename = directory.GetNext();


        //Loop through our items in folder
        while (filename != null)
        {
            if (!directory.CurrentIsDir()){
                //Add the resource to list
                items.Add(GD.Load($"res://ItemData/{filename}"));
            }

            filename = directory.GetNext();
                   
        }
    }


    public Resource get_item(string item_name)
    {

        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].ResourceName == item_name)
            {
                Resource item_resource = items[i];
                return item_resource;
            }
        }
        return null;
    }






//Last bracket
}
