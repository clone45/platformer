//
// TODO: add comentators
//

using Godot;
using System;

public partial class level_1 : Node2D
{
	PackedScene axe;
	public player ninjafrog;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		axe = (PackedScene) GD.Load("res://throwing_axe.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsActionJustPressed("throw"))
		{
			
			throwing_axe axe_instance = (throwing_axe) axe.Instantiate();
			
			if(! CharacterBody2D.IsInstanceValid(axe_instance))
			{
				GD.Print("Invalid axe instance.");
			}
			
			ninjafrog = GetNode<player>("Player");
			axe_instance.Position = ninjafrog.Position;
			
			axe_instance.HorizontalVelocity = 300.0f * ninjafrog.Direction;
			
			this.AddChild(axe_instance);
		}
	}
}
