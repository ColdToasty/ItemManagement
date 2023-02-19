using Godot;
using System;

public class Cookies : StaticBody2D
{
	private Sprite cookie_texture;

	private Random rng = new Random();

	private CookieCounter Cookie_counter;

	public override void _Ready()
	{
		cookie_texture = GetNode<Sprite>("Sprite");
		int texture = rng.Next(5);
		cookie_texture.Frame = texture;
		Cookie_counter = GetNode<CookieCounter>("/root/CookieCounter");
	}


	//When player enteres the area
	//Player picks up the cookie
	//Singleton - Cookies += 1
	private void _on_playerReach_area_entered(object area)
	{
		Cookie_counter.Cookie_counter += 1;
		QueueFree();
	}

}



