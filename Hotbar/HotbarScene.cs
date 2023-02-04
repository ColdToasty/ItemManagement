using Godot;
using System;

public class HotbarScene : Node2D
{
    public TextureRect textureRect;
    public GridContainer hotbar;

    public override void _Ready()
    {
        textureRect = GetNode<TextureRect>("TextureRect");
        hotbar = GetNode<GridContainer>("HotBar");
    }

}
