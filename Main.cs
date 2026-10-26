using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public PackedScene MobScene { get; set; }
	
	private void OnMobTimerTimeout()
	{
		//class Mob object mob, PackedScene class ,MobScene variable have the method/function Instantiate<Mob>()
		Mob mob = MobScene.Instantiate<Mob>();
		
		//Choose random loc on the SpawnPath
		//store the ref to SpawnLocation node
		var mobSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
		
		//random offset
		mobSpawnLocation.ProgressRatio = GD.Randf();
		
		//class Vector3
		//object playerPosition,
		//GetNode is a method of Node class,
		//<Player> is a generic type,
		//"Player" is the name of the node in the scene tree
		Vector3 playerPosition = GetNode<Player>("Player").Position;
		mob.Initialize(mobSpawnLocation.Position, playerPosition);
		
		AddChild(mob);
		
		mob.Squashed += GetNode<ScoreLabel>("UserInterface/ScoreLabel").OnMobSquashed;
	}
	
	private void OnPlayerHit()
	{
		GD.Print("Player hit");
		GetNode<Timer>("MobTimer").Stop();
		
		GetNode<Control>("UserInterface/Again").Show();
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Control>("UserInterface/Again").Hide();
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if(@event.IsActionPressed("ui_accept") && GetNode<Control>("UserInterface/Again").Visible)
		{
			GetTree().ReloadCurrentScene();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
