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
	public Timer death_timer;
	public Timer respawn_timer;
	
	enum States : int
	{
		gameplay,
		death,
		respawn
	}
	
	States state = States.gameplay;
	
	public override void _Ready()
	{
		base._Ready();
		
		SpriteAnimation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		SpriteAnimation.Play("idle");
		
		jump_button_timer = GetNode<Timer>("JumpButtonTimer");
		death_timer = GetNode<Timer>("DeathTimer");
		respawn_timer = GetNode<Timer>("RespawnTimer");
	}

	public override void _PhysicsProcess(double delta)
	{
		switch(state)
		{
			case States.gameplay:
				ProcessGameplay(delta);
				break;
			case States.death:
				ProcessDeath(delta);
				break;
			case States.respawn:
				ProcessRespawn(delta);
				break;				
		}
	}
	
	private void ProcessGameplay(double delta)
	{
		Vector2 new_velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			// Computer gravity
			new_velocity.Y += gravity * (float)delta;
			
			if(new_velocity.Y <= 0) {
				SpriteAnimation.Play("jump");
			}
			else {
				SpriteAnimation.Play("fall");
			}
		}

		// Handle Jump
		if (Input.IsActionJustPressed("jump"))
		{
			if(IsOnFloor()) {
				new_velocity.Y = JumpVelocity;
			}
			else {
				// Set timer
				jump_button_timer.Start(0.10f);
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
		
		for (int i = 0; i < GetSlideCollisionCount(); i++)
		{
			var collision = GetSlideCollision(i);

			if((collision.GetCollider() as Node).IsInGroup("touch_of_death"))
			{
				state = States.death;
				death_timer.Start(0.25f);
			}
		}
	}

	private void ProcessDeath(double delta)
	{
		SpriteAnimation.Play("disappearing");
		
		if(death_timer.IsStopped())
		{
			// this.QueueFree();
			// SetPosition(Vector2(218.0f, -93.0f));
			Position = new Vector2(218.0f, -140.0f);
			state = States.respawn;
			respawn_timer.Start(0.25f);
		}
	}

	private void ProcessRespawn(double delta)
	{
		SpriteAnimation.Play("appearing");
		
		if(respawn_timer.IsStopped())
		{
			state = States.gameplay;
		}
	}

}
