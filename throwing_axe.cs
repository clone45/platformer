using Godot;
using System;

public partial class throwing_axe : CharacterBody2D
{
	public const float Speed = 150.0f;
	public const float VerticalVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		base._Ready();
		
		Vector2 velocity = Velocity;
		
		velocity.X = Speed;
		velocity.Y = VerticalVelocity;
		
		Velocity = velocity;
	}

	public override void _PhysicsProcess(double delta)
	{

		Vector2 velocity = Velocity;

		// Add the gravity.
		// Eventually change this
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		// Handle Jump.
		// if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
		// 	velocity.Y = VerticalVelocity;

		//
		// I'll have to figure out the direction.  Maybe I set a variable in the class for that?
		//
		
		/*
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}
		*/

		Velocity = velocity;
		// MoveAndSlide();
		var collision = MoveAndCollide(Velocity * (float)delta);
		
		if (collision != null)
		{
			GD.Print("I collided with ", ((Node)collision.GetCollider()).Name);
			
			this.QueueFree();
		}		
	}
}
