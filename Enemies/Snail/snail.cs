using Godot;
using System;


public partial class snail : CharacterBody2D
{
	PathFollow2D path_follow;
	public AnimatedSprite2D SpriteAnimation;
	
	private float speed = 10.0f;
	
	public override void _Ready()
	{
		base._Ready();
		SpriteAnimation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		SpriteAnimation.Play("idle");
		
		path_follow = (PathFollow2D) this.GetParent();
		
		if(! PathFollow2D.IsInstanceValid(path_follow))
		{
			GD.Print("Parent path not found.");
		}
	}

	public override void _PhysicsProcess(double delta)
	{		
		path_follow.Progress += (float) delta * speed;		
		SpriteAnimation.FlipH = (path_follow.ProgressRatio > 0.5f);
	}

}
