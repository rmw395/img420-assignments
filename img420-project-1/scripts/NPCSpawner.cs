using Godot;
using System;
using System.Collections.Generic;

public partial class NPCSpawner : Node2D
{
	[Export]
	public PackedScene NPCScene { get; set; } = GD.Load<PackedScene>("res://scenes/npc.tscn");

	[Export]
	public int SpawnCount { get; set; } = 5;

	[Export]
	public Vector2 SpawnAreaSize { get; set; } = new Vector2(2000, 2000);

	private List<Npc> npcs = new List<Npc>();
	private RandomNumberGenerator rng = new RandomNumberGenerator();

	public override void _Ready()
	{
		rng.Randomize();
		SpawnNPCs();
	}

	private void SpawnNPCs()
	{
		for (int i = 0; i < SpawnCount; i++)
		{
			SpawnSingleNPC();
		}
	}

	private void SpawnSingleNPC()
	{
		if (NPCScene == null)
		{
			GD.PrintErr("NPCScene not assigned!");
			return;
		}

		Npc npc = NPCScene.Instantiate<Npc>();

		// Random position inside spawn area
		npc.GlobalPosition = new Vector2(
			rng.RandfRange(GlobalPosition.X - SpawnAreaSize.X / 2, GlobalPosition.X + SpawnAreaSize.X / 2),
			rng.RandfRange(GlobalPosition.Y - SpawnAreaSize.Y / 2, GlobalPosition.Y + SpawnAreaSize.Y / 2)
		);

		// Set bounds for NPC movement
		npc.MinBounds = Vector2.Zero;
		npc.MaxBounds = SpawnAreaSize;

		// Connect collision signal (assuming NPC uses Area2D collision)
		if (npc.GetNodeOrNull<Area2D>("CollisionShape2D") is Area2D area)
		{
			area.BodyEntered += (body) => OnNPCCollected(npc);
		}

		AddChild(npc);
		npcs.Add(npc);
	}

	private void OnNPCCollected(Npc npc)
	{
		if (npcs.Contains(npc))
		{
			npcs.Remove(npc);
			SpawnSingleNPC(); // Spawn a replacement
		}
	}
}
