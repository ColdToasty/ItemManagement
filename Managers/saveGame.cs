using Godot;
using System;

public class saveGame : Resource
{

    private static string SAVE_GAME_PATH = "user://saveGame.tres";
    

    public void Write_save_game()
    {
        ResourceSaver.Save(SAVE_GAME_PATH, this, ResourceSaver.SaverFlags.RelativePaths);
    }


    public static Resource Load_save_game()
    {
        if (ResourceLoader.Exists(SAVE_GAME_PATH))
        {
            return ResourceLoader.Load(SAVE_GAME_PATH);
        }
        return null;
    }


}
