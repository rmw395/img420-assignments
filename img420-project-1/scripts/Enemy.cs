using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	// Signal
	[Signal]
	public delegate void EnemyHitPlayerEventHandler(Enemy enemy);

	// Exported properties
	[Export]
	public float Speed { get; set; } = 150f;

	[Export]
	public Vector2 MinBounds { get; set; } = Vector2.Zero;

	[Export]
	public Vector2 MaxBounds { get; set; } = new Vector2(1152, 648);

	// Reference to player
	private Node2D player;

	public override void _Ready()
	{
		// Find the player in the current scene
		player = GetTree().CurrentScene.GetNode<Node2D>("Player");
		if (player == null)
		{
			GD.PrintErr("Enemy cannot find Player node!");
		}

		// Optional: connect body entered signal if using an Area2D
		if (GetNodeOrNull<Area2D>("CollisionShape2D") is Area2D area)
		{
			area.BodyEntered += OnBodyEntered;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (player != null)
		{
			Vector2 dir = (player.GlobalPosition - GlobalPosition).Normalized();
			Velocity = dir * Speed;
			MoveAndSlide();
		}

		// Clamp to level bounds
		GlobalPosition = GlobalPosition.Clamp(MinBounds, MaxBounds);
	}

	private void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("player"))
		{
			EmitSignal(SignalName.EnemyHitPlayer, this);
			// queue_free safely
			CallDeferred(nameof(QueueFree));
		}
	}
}
