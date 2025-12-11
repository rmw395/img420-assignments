using Godot;
using System;

public partial class Player : CharacterBody2D
{
	// --- Exported Variables ---
	[Export] public float MoveSpeed { get; set; } = 200f;
	[Export] public int MaxHealth { get; set; } = 100;
	[Export] public int MaxBlood { get; set; } = 3;

	[Export] public Vector2 MinBounds { get; set; } = new Vector2(0, 0);
	[Export] public Vector2 MaxBounds { get; set; } = new Vector2(1152, 648);

	// --- Runtime Values ---
	public int Health { get; private set; }
	public int BloodLevel { get; private set; }

	// --- Signals ---
	[Signal] public delegate void HealthChangedEventHandler(int newHealth);
	[Signal] public delegate void BloodChangedEventHandler(int newBlood);
	[Signal] public delegate void PlayerDiedEventHandler();
	[Signal] public delegate void PlayerWonEventHandler();

	public override void _Ready()
	{
		Health = MaxHealth;
		BloodLevel = 0;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 input = Vector2.Zero;

		input.X = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
		input.Y = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");

		Velocity = input.Normalized() * MoveSpeed;

		MoveAndSlide();

		// Clamp player inside level bounds
		GlobalPosition = new Vector2(
			Mathf.Clamp(GlobalPosition.X, MinBounds.X, MaxBounds.X),
			Mathf.Clamp(GlobalPosition.Y, MinBounds.Y, MaxBounds.Y)
		);
	}

	// --- Gameplay Methods ---
	public void TakeDamage(int amount)
	{
		Health -= amount;
		EmitSignal(SignalName.HealthChanged, Health);

		if (Health <= 0)
			EmitSignal(SignalName.PlayerDied);
	}

	public void CollectBlood(int amount = 1)
	{
		BloodLevel += amount;
		EmitSignal(SignalName.BloodChanged, BloodLevel);

		if (BloodLevel >= MaxBlood)
			EmitSignal(SignalName.PlayerWon);
	}
}
