using Godot;
using System;
using System.Collections.Generic;


public class Bag : Resource
{
    [Export]
    public List<Resource> _items = new List<Resource>();

    [Signal]
    public delegate void inventory_changed(Bag items);

    public List<Resource> Items
    {
        get { return _items; }
        set
        {
            _items = value;
            EmitSignal("inventory_changed", this);
        }

    }

    public Resource get_specific_item(int index)
    {
        return _items[index];
    }

  
    
}
