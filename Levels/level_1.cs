using Godot;
using System;

public partial class level_1 : Node2D
{
	PackedScene axe;
	public CharacterBody2D player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		axe = (PackedScene) GD.Load("res://throwing_axe.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsActionJustPressed("ui_accept"))
		{
			
			CharacterBody2D axe_instance = (CharacterBody2D) axe.Instantiate();
			
			/*
			if(! CharacterBody2D.IsInstanceValid(axe_instance))
			{
				GD.Print("Invalid axe instance.");
			}
			*/
			
			player = GetNode<CharacterBody2D>("Player");
			axe_instance.Position = player.Position;
			
			// axe_instance.HorizontalVelocity = 150.0f * player_instance.Direction;
			// axe_instance.VerticalVelocity = -400.0f;
			
			this.AddChild(axe_instance);
		}
	}
}

/*
	public AnimatedSprite2D SpriteAnimation;
	
	public override void _Ready()
	{
		base._Ready();
		SpriteAnimation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		SpriteAnimation.Play("idle");
	}
*/
