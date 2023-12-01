using Godot;
using Godot.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


public class DialogBox : Node2D
{
    private static string levelDialogPath = "user://saves/dialog/levelDialog";
    private static string ruldolphDialogPath = levelDialogPath + "/ruldolph";

    private static bool dialogFinished = false;


    public static bool DialogFinished
    {
        get { return dialogFinished; }
        set { dialogFinished = value; }
    }

    public override void _Ready()
    {
        
    }


    private Godot.Collections.Dictionary<string, string> getLevelDialog()
    {
        var file = new File();
        Error checkFileContinue = file.Open(levelDialogPath, File.ModeFlags.Read);
        Godot.Collections.Dictionary<string, string> dialog = new Godot.Collections.Dictionary<string, string>();
        if (checkFileContinue == Error.Ok)
        {
            string dialogData = file.GetAsText();
            JSONParseResult saveData = JSON.Parse(dialogData);

            dialog = saveData.Result as Godot.Collections.Dictionary<string, string>;
        }
        return dialog;
        
    }

    

}
