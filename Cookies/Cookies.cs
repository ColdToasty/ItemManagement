using Godot;
using System;

public class Cookies : StaticBody2D
{
	private Sprite cookie_texture;

	private Random rng = new Random();

	private CookieCounter level_cookie_counter;

	[Signal]
	public delegate void cookieIncrease();

	public override void _Ready()
	{
		cookie_texture = GetNode<Sprite>("Sprite");
		int texture = rng.Next(5);
		cookie_texture.Frame = texture;
        level_cookie_counter = GetNode<CookieCounter>("/root/CookieCounter");
	}


	//When player enteres the area
	//Player picks up the cookie
	//Singleton - Cookies += 1
	private void _on_playerReach_area_entered(object area)
	{
        level_cookie_counter.Level_Cookie_Counter += 1;
		QueueFree();
	}

}



