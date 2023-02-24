using Godot;
using System;

public partial class player : CharacterBody2D
{
	public const float Speed = 150.0f;
	public const float JumpVelocity = -350.0f;

	public float Direction = 1.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public AnimatedSprite2D SpriteAnimation;
	public Timer jump_button_timer;
	
	public override void _Ready()
	{
		base._Ready();
		SpriteAnimation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		SpriteAnimation.Play("idle");
		jump_button_timer = GetNode<Timer>("JumpButtonTimer");
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 new_velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			new_velocity.Y += gravity * (float)delta;
			
			if(new_velocity.Y <= 0)
			{
				SpriteAnimation.Play("jump");
			}
			else
			{
				SpriteAnimation.Play("fall");
			}
			
		}

		// Handle Jump
		if (Input.IsActionJustPressed("jump"))
		{
			if(IsOnFloor())
			{
				new_velocity.Y = JumpVelocity;
			}
			else
			{
				// Set timer
				jump_button_timer.Start(0.10f);
				
				// If close enough to floor, then jump
				/*
				var spaceState = GetWorld2D().DirectSpaceState;
				var query = PhysicsRayQueryParameters2D.Create(globalPosition, enemyPosition, CollisionMask, new Godot.Collections.Array<Rid> { GetRid() });
				var result = spaceState.IntersectRay(query);
				*/
			}		
		}
		
		if(IsOnFloor() && ! jump_button_timer.IsStopped())
		{
			new_velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		// Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (direction != Vector2.Zero)
		{
			new_velocity.X = direction.X * Speed;
			
			SpriteAnimation.FlipH = (new_velocity.X < 0);
		}
		else
		{
			new_velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
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

		Direction = (SpriteAnimation.FlipH) ? -1.0f : 1.0f;

		Velocity = new_velocity;
		MoveAndSlide();
	}
}
