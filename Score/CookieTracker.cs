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
		cookieScore = counter.Cookie_counter;
		timer = GetNode<Timer>("Timer");
		this.Visible = false;
	}


	public void display_score()
	{
		label.Text = $"Cookies: {counter.Cookie_counter}";
		cookieScore = counter.Cookie_counter;
		this.Visible = true;
		timer.Start(2);
	}

	private void _on_Timer_timeout()
	{
		this.Visible = false;
	}



	public override void _PhysicsProcess(float delta)
	{
		if(cookieScore != counter.Cookie_counter)
		{
			display_score();
		}
	}

}


