using Godot;
using System;

public partial class GameManager : Node
{
	private Node player;

	public override void _Ready()
	{
		// Find the first node in the "player" group
		player = GetTree().GetFirstNodeInGroup("player");

		if (player != null)
		{
			// Connect signals
			player.Connect("health_changed", Callable.From(this, nameof(OnHealthChanged)));
			player.Connect("blood_changed", Callable.From(this, nameof(OnBloodChanged)));
			player.Connect("player_died", Callable.From(this, nameof(OnPlayerDied)));
			player.Connect("player_won", Callable.From(this, nameof(OnPlayerWon)));
		}
		else
		{
			GD.PrintErr("Player node not found in scene!");
		}
	}

	public override void _Process(double delta)
	{
		if (player == null) return;

		// Assuming Player has public properties health and blood_level
		var healthProp = player.Get("health");
		var bloodProp = player.Get("blood_level");

		if (healthProp is int health && health <= 0)
		{
			OnPlayerDied();
		}
		else if (bloodProp is int blood && blood >= 3)
		{
			OnPlayerWon();
		}
	}

	private void OnHealthChanged(int newHealth)
	{
		// HUD handles display
	}

	private void OnBloodChanged(int newBlood)
	{
		// HUD handles display
	}

	private void OnPlayerDied()
	{
		GD.Print("You lost!");
		GetTree().Paused = true;

		if (GetNodeOrNull<Label>("LoseLabel") is Label loseLabel)
			loseLabel.Visible = true;

		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}

	private void OnPlayerWon()
	{
		GD.Print("You won!");
		GetTree().Paused = true;

		if (GetNodeOrNull<Label>("WinLabel") is Label winLabel)
			winLabel.Visible = true;

		// Queue free player safely
		player?.CallDeferred("queue_free");

		GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
	}
}
