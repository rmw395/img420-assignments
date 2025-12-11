using Godot;
using System;
using System.Collections.Generic;

public partial class EnemySpawner : Node2D
{
	[Export]
	public PackedScene EnemyScene { get; set; } = GD.Load<PackedScene>("res://scenes/enemy.tscn");

	[Export]
	public int SpawnCount { get; set; } = 3;

	[Export]
	public Vector2 SpawnAreaSize { get; set; } = new Vector2(2000, 2000);

	// Track current enemies
	private List<Enemy> enemies = new List<Enemy>();

	public override void _Ready()
	{
		for (int i = 0; i < SpawnCount; i++)
		{
			SpawnEnemy();
		}
	}

	private void SpawnEnemy()
	{
		if (EnemyScene == null)
		{
			GD.PrintErr("EnemyScene not assigned!");
			return;
		}

		// Instantiate enemy
		Enemy enemy = EnemyScene.Instantiate<Enemy>();

		// Random spawn position within area
		var rand = new RandomNumberGenerator();
		rand.Randomize();

		enemy.GlobalPosition = new Vector2(
			rand.RandfRange(GlobalPosition.X - SpawnAreaSize.X / 2, GlobalPosition.X + SpawnAreaSize.X / 2),
			rand.RandfRange(GlobalPosition.Y - SpawnAreaSize.Y / 2, GlobalPosition.Y + SpawnAreaSize.Y / 2)
		);

		// Assign level bounds
		enemy.MinBounds = Vector2.Zero;
		enemy.MaxBounds = SpawnAreaSize;

		// Connect signal
		enemy.Connect(Enemy.SignalName.EnemyHitPlayer, Callable.From(this, nameof(OnEnemyHitPlayer)));

		AddChild(enemy);
		enemies.Add(enemy);
	}

	private void OnEnemyHitPlayer(Enemy enemy)
	{
		if (enemies.Contains(enemy))
		{
			enemies.Remove(enemy);
			SpawnEnemy();
		}
	}
}
