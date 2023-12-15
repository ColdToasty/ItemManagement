using Godot;
using System;

public class DialogController : Control
{
    private static string characterDialog = "user://dialog";
    private static string fileType = ".json";
    private static string rudolphDialogPath = characterDialog + "/rudolph/rudolph";


    private Godot.Collections.Dictionary<string, string> rudolphDialog;
    private int dialogIndex = 0;
    private RichTextLabel dialogText;
    private Tween tween;
    private bool dialogFinished = false;

    private string[] dialog = new string[] {
        "Hello there this is test dialog number 1 which will check if it reall works",
        "If that works then this is dialog number 2 which is to continue the dialog to see if the player can interact with it",
        "it works"
    };
    public override void _Ready()
    {
        dialogText = GetNode<RichTextLabel>("DialogBox/RichTextLabel");
        tween = GetNode<Tween>("DialogBox/Tween");
    }

    //Returns the correct dialog level 
    public string characterDialogLevel(string level)
    {
        return rudolphDialogPath + level;
    }


    //Loads the dialog text into RichTextLabel node
    private void loadDialog()
    {
        if(dialogIndex < dialog.Length)
        {
            dialogText.BbcodeText = dialog[dialogIndex];
            dialogText.PercentVisible = 0;
            tween.InterpolateProperty(dialogText, "percent_visible", 0, 1, 3, Tween.TransitionType.Linear, Tween.EaseType.InOut);
            tween.Start();
        }
        else
        {
            this.QueueFree();
        }
        dialogIndex++;
    }


    //Returns the dialog data in a dictionary from files
    private Godot.Collections.Dictionary getDialog(string characterPath)
    {
        Godot.Collections.Dictionary dialog = new Godot.Collections.Dictionary();

        var file = new File();
        Error CheckFileOk = file.Open(characterPath + fileType, File.ModeFlags.Read);
        if (CheckFileOk == Error.Ok)
        {
            if (file.FileExists(characterPath))
            {
                string dialogData = file.GetAsText();
                JSONParseResult saveData = JSON.Parse(dialogData);
                dialog = saveData.Result as Godot.Collections.Dictionary;
            }
            else
            {
                OS.Alert("File doesn't exist");
            }
        }
        else
        {
            OS.Alert("No path");
        }
        return dialog;

    }


    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("UseItem"))
        {
            loadDialog();
        }

    }
}
