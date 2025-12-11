using Godot;
using System;

public partial class Level : Node2D
{
	// Level boundaries
	[Export]
	public Vector2 MinBounds { get; set; } = Vector2.Zero;

	[Export]
	public Vector2 MaxBounds { get; set; } = new Vector2(1920, 1080);

	// Spawners
	private Node npcSpawner;
	private Node enemySpawner;

	// Player
	private Node player;

	public override void _Ready()
	{
		npcSpawner = GetNodeOrNull<Node>("NPCSpawner");
		enemySpawner = GetNodeOrNull<Node>("EnemySpawner");
		player = GetNodeOrNull<Node>("Player");

		// Set boundaries for player
		if (player != null)
		{
			if (player.HasMethod("set_bounds"))
			{
				player.Call("set_bounds", MinBounds, MaxBounds);
			}
			else
			{
				player.Set("min_bounds", MinBounds);
				player.Set("max_bounds", MaxBounds);
			}
		}

		// Set boundaries for NPCs
		if (npcSpawner != null)
		{
			foreach (Node npc in npcSpawner.GetChildren())
			{
				if (npc.HasMethod("set_bounds"))
				{
					npc.Call("set_bounds", MinBounds, MaxBounds);
				}
				else
				{
					npc.Set("min_bounds", MinBounds);
					npc.Set("max_bounds", MaxBounds);
				}
			}
		}

		// Set boundaries for Enemies
		if (enemySpawner != null)
		{
			foreach (Node enemy in enemySpawner.GetChildren())
			{
				if (enemy.HasMethod("set_bounds"))
				{
					enemy.Call("set_bounds", MinBounds, MaxBounds);
				}
				else
				{
					enemy.Set("min_bounds", MinBounds);
					enemy.Set("max_bounds", MaxBounds);
				}
			}
		}
	}
}
