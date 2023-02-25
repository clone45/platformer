using Godot;
using System;

public partial class throwing_axe : CharacterBody2D
{
	public float HorizontalVelocity = 200.0f;
	public float VerticalVelocity = -250.0f;
	public float RotationSpeed = 20.0f;
	public float RotationDirection = 1.0f;
	
	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public AnimatedSprite2D SpriteAnimation;
	public Timer axe_fade_out_timer;

	public bool stuck = false;

	public override void _Ready()
	{
		base._Ready();
		
		// Set initial velocity
		Vector2 velocity = Velocity;
		velocity.X = HorizontalVelocity;
		velocity.Y = VerticalVelocity;
		Velocity = velocity;
		
		// Connect to screen_exited event
		// see: https://docs.godotengine.org/en/latest/tutorials/scripting/c_sharp/c_sharp_signals.html#custom-signals-as-c-events
		// VisibleOnScreenNotifier2D visibility_notifier = this.GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		// visibility_notifier.Connect("screen_exited", this, "OnVisibilityNotifier2DScreenExited");

		VisibleOnScreenNotifier2D visibility_notifier = this.GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
		visibility_notifier.ScreenExited += OnVisibilityNotifier2DScreenExited;
		
		SpriteAnimation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		axe_fade_out_timer = GetNode<Timer>("AxeFadeOutTimer");
	}

	public override void _PhysicsProcess(double delta)
	{
		if(! this.stuck)
		{
			Vector2 velocity = Velocity;

			// Add the gravity.
			// Eventually change this
			if (!IsOnFloor())
				velocity.Y += gravity * (float)delta;

			Velocity = velocity;
			
			RotationDirection = (velocity.X < 0) ? -1.0f : 1.0f;
			Rotation += RotationDirection * RotationSpeed * (float)delta;

			SpriteAnimation.FlipH = (velocity.X < 0);

			var collision = MoveAndCollide(Velocity * (float)delta);
			
			if (collision != null && axe_fade_out_timer.IsStopped())
			{
				GD.Print("I collided with ", ((Node)collision.GetCollider()).Name);
				
				// this.QueueFree();
				this.stuck = true;
				axe_fade_out_timer.Start(0.5f);
				axe_fade_out_timer.Timeout += AxeFadeOutTimerTimeout;
			}			
		}
		else
		{
			Color modulate = this.Modulate;
			modulate.A = (float) axe_fade_out_timer.TimeLeft * 2.0f;
			this.Modulate = modulate;
		}
	}
	
	public void OnVisibilityNotifier2DScreenExited()
	{
		// Deletes the bullet when it exits the screen.
		GD.Print("deleted axe");
		this.QueueFree();
	}
	
	public void AxeFadeOutTimerTimeout()
	{
		GD.Print("deleted axe");
		this.QueueFree();
	}
}
