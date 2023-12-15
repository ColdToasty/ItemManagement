using Godot;
using System;

public class BlackFadeOut : Control
{
	
	private AnimationPlayer animationPlayer;
	public TextureRect blackLayer;

	[Signal]
	public delegate void fade_finished(string name);

	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		blackLayer = GetNode<TextureRect>("TextureRect");
		
	}

	public void hide()
	{
		this.SetDeferred("visible", false);
	}
	public void show()
	{
		this.SetDeferred("visible", true);
	}
	
	public void playFadeOut()
	{
		animationPlayer.Play("fadeOut");
	}

	public void playFadeIn()
	{
		animationPlayer.Play("fadeIn");
	}


    private void _on_AnimationPlayer_animation_finished(String anim_name)
    {
        EmitSignal("fade_finished", anim_name);
	}
}
