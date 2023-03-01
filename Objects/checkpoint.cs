using Godot;
using System;

public partial class checkpoint : Area2D
{
	public AnimatedSprite2D SpriteAnimation;
	public Timer rise_timer;

	enum States : int
	{
		down,
		rising,
		up
	}
	
	States state = States.down;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		state = States.down;
		SpriteAnimation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		this.BodyEntered += RaiseFlag;
		rise_timer = GetNode<Timer>("RiseTimer");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(state == States.rising && rise_timer.IsStopped())
		{
			state = States.up;
			SpriteAnimation.Play("idle");
		}
	}
	
	public void RaiseFlag(Node body)
	{
		if(state == States.down)
		{
			rise_timer.Start(0.75f);
			SpriteAnimation.Play("raise");
			state = States.rising;
		}
	}	
}
