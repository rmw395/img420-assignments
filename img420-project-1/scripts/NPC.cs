using Godot;
using System;

public partial class NPC : CharacterBody2D
{
	[Export]
	public float MoveSpeed { get; set; } = 100f;

	[Export]
	public Vector2 MinBounds { get; set; } = Vector2.Zero;

	[Export]
	public Vector2 MaxBounds { get; set; } = new Vector2(1152, 648);

	private Vector2 wanderDir = Vector2.Zero;
	private float changeTimer = 0f;

	private RandomNumberGenerator rng = new RandomNumberGenerator();

	public override void _Ready()
	{
		rng.Randomize();
		wanderDir = new Vector2(rng.RandfRange(-1f, 1f), rng.RandfRange(-1f, 1f)).Normalized();

		// Connect signal if using an Area2D for collisions
		if (GetNodeOrNull<Area2D>("CollisionShape2D") is Area2D area)
			area.BodyEntered += OnBodyEntered;
	}

	public override void _Process(double delta)
	{
		changeTimer -= (float)delta;
		if (changeTimer <= 0f)
		{
			wanderDir = new Vector2(rng.RandfRange(-1f, 1f), rng.RandfRange(-1f, 1f)).Normalized();
			changeTimer = rng.RandfRange(1f, 3f);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Velocity = wanderDir * MoveSpeed;
		MoveAndSlide();

		// Bounce off bounds
		if (GlobalPosition.X <= MinBounds.X || GlobalPosition.X >= MaxBounds.X)
			wanderDir.X = -wanderDir.X;
		if (GlobalPosition.Y <= MinBounds.Y || GlobalPosition.Y >= MaxBounds.Y)
			wanderDir.Y = -wanderDir.Y;

		GlobalPosition = GlobalPosition.Clamped(MinBounds, MaxBounds);
	}

	private void OnBodyEntered(Node body)
	{
		if (body.IsInGroup("player"))
		{
			// Call collect_blood method on player
			body.Call("collect_blood", 1);

			// Queue free safely
			CallDeferred(nameof(QueueFree));
		}
	}
}
