using Godot;
using System;

public class CookieTracker : Control
{
	public Label label;
	public CookieCounter counter;
	public int cookieScore;
	public Timer timer;

	public override void _Ready()
	{
		label = GetNode<Label>("HBoxContainer/Label");
		counter = GetNode<CookieCounter>("/root/CookieCounter");
		cookieScore = counter.Level_Cookie_Counter;
		timer = GetNode<Timer>("showCookiesTimer");
		this.Visible = false;
	}


	public void display_score()
	{
		label.Text = $"Cookies: {counter.Level_Cookie_Counter}";
		cookieScore = counter.Level_Cookie_Counter;
		this.Visible = true;
		timer.Start(2);
	}
	
	private void _on_Timer_timeout()
	{
		this.Visible = false;
	}



	public override void _PhysicsProcess(float delta)
	{
		//Displays the score only when changed
		if(cookieScore != counter.Level_Cookie_Counter)
		{
			display_score();
		}
	}

}


