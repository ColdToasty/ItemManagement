using Godot;
using System;
using MonoCustomResourceRegistry;


[RegisteredType(nameof(ItemInfo), "", nameof(Resource))]

public class ItemInfo : Resource
{
    [Export]
    public string name { get; set; }

    [Export]
    public Texture texture { get; set; }
    public void Item_Data()
    {
        this.name = "";
        this.texture = null;
    }

   

}
