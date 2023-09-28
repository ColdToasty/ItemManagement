using Godot;
using Godot.Collections;
using System;

public class levelSelector : Control
{
    private string fileSaveName = GameFiles.save_name;

    private Dictionary playerData;

    public override void _Ready()
    {
                
        playerData = GameFiles.GetFileContents();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
