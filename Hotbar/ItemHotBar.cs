using Godot;
using System;

public class ItemHotBar : Panel
{

	private TextureRect current;
	private TextureRect itemkey;
	public int hotbarCount = 10;
	private string[] hotbarItems = new string[10];


	public override void _Ready()
	{
		current = GetNode<TextureRect>("Current");
		itemkey = GetNode<TextureRect>("HBoxContainer/Item3");
	}

	private void scroll_left(){
		if (current.RectPosition.x == 2){
			current.RectPosition = new Vector2(497,2);
		}
		else{
			 current.RectPosition = new Vector2(current.RectPosition.x - 55 ,2);
		}
	}

	private void scroll_right(){
		if(current.RectPosition.x == 497){
			current.RectPosition = new Vector2(2,2);
		}
		else{
			 current.RectPosition = new Vector2(current.RectPosition.x + 55 ,2);
		}

	}
	//Need input key
	private void change_frame_position(int position_value){
		current.RectPosition = new Vector2(2+ 55*position_value, 2);
		GD.Print(hotbarItems.Length);
	}




	public override void _PhysicsProcess(float delta)
	{
		if(Input.IsActionJustReleased("Scroll_Up")){
			scroll_right();
		}

		if(Input.IsActionJustReleased("Scroll_Down")){
			scroll_left();
		}

		if(Input.IsActionJustPressed("1")){
			change_frame_position(0);
		}       
		else if(Input.IsActionJustPressed("2")){
			change_frame_position(1);
		}       
		if(Input.IsActionJustPressed("3")){
			change_frame_position(2);
		}       
		if(Input.IsActionJustPressed("4")){
			change_frame_position(3);
		}       
		if(Input.IsActionJustPressed("5")){
			change_frame_position(4);
		}       
		if(Input.IsActionJustPressed("6")){
			change_frame_position(5);
		}       
		if(Input.IsActionJustPressed("7")){
			change_frame_position(6);
		}       
		if(Input.IsActionJustPressed("8")){
			change_frame_position(7);
		}       
		if(Input.IsActionJustPressed("9")){
			change_frame_position(8);
		}       
		if(Input.IsActionJustPressed("0")){
			change_frame_position(9);
		}       



	}
}


