using Godot;
using System;

public partial class cherries : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.BodyEntered += GetCherry;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void GetCherry(Node body)
	{
		GD.Print("got cherry");
		QueueFree();
	}	
}
