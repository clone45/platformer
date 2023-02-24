using Godot;
using System;

public partial class throwing_axe : CharacterBody2D
{
	public float HorizontalVelocity = 150.0f;
	public float VerticalVelocity = -400.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

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
		// visibility_notifier.ScreenExited += () => GD.Print("screen exited!");	
	}

	public override void _PhysicsProcess(double delta)
	{

		Vector2 velocity = Velocity;

		// Add the gravity.
		// Eventually change this
		if (!IsOnFloor())
			velocity.Y += gravity * (float)delta;

		Velocity = velocity;
		var collision = MoveAndCollide(Velocity * (float)delta);
		
		if (collision != null)
		{
			GD.Print("I collided with ", ((Node)collision.GetCollider()).Name);
			
			this.QueueFree();
		}		
	}
	
	public void OnVisibilityNotifier2DScreenExited()
	{
		// Deletes the bullet when it exits the screen.
		GD.Print("deleted axe");
		this.QueueFree();
	}	
}
