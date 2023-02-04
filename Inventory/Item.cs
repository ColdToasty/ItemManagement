using Godot;
using System;

public class Item : Node2D
{
    private Texture texture;
    private TextureRect toolTexture;
    public override void _Ready()
    {
        toolTexture = GetNode<TextureRect>("TextureRect");

        if(GD.Randi() % 2 == 0){
            texture = (Texture)GD.Load("res://Item-Images/pickaxe.png");
        }
        else{
            texture = (Texture)GD.Load("res://Item-Images/axe.png");
        }
        
        toolTexture.Texture = texture;
    }

 

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
