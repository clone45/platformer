using Godot;
using System;

public partial class player : CharacterBody2D
{
	public const float Speed = 150.0f;
	public const float JumpVelocity = -350.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public AnimatedSprite2D SpriteAnimation;
	
	public override void _Ready()
	{
		base._Ready();
		SpriteAnimation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		SpriteAnimation.Play("idle");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y += gravity * (float)delta;
			
			if(velocity.Y <= 0)
			{
				SpriteAnimation.Play("jump");
			}
			else
			{
				SpriteAnimation.Play("fall");
			}
			
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
			velocity.Y = JumpVelocity;

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
			
			SpriteAnimation.FlipH = (velocity.X < 0);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		if (IsOnFloor())
		{
			if (direction.X == 0)
			{
				SpriteAnimation.Play("idle");	
			}
			else
			{
				SpriteAnimation.Play("run");
			}		
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
