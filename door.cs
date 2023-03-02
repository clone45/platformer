using Godot;
using System;

public partial class door : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.BodyEntered += ChangeLevel;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
			
	}
	
	public void ChangeLevel(Node body)
	{
		GetTree().ChangeSceneToFile("res://Levels/level_2.tscn");
	}	
}
