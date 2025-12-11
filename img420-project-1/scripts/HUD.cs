using Godot;
using System;

public partial class Hud : CanvasLayer
{
	private Node player;

	// Node references
	private Label HealthLabel;
	private Label BloodLabel;

	public override void _Ready()
	{
		// Get labels
		HealthLabel = GetNode<Label>("VBoxContainer/HealthLabel");
		BloodLabel = GetNode<Label>("VBoxContainer/BloodLabel");

		// Find the first node in the "player" group
		player = GetTree().GetFirstNodeInGroup("player");

		if (player != null)
		{
			// Connect signals
			player.Connect("HealthChanged", new Callable(this, nameof(UpdateHealth)));
			player.Connect("BloodChanged", new Callable(this, nameof(UpdateBlood)));

			// Initialize HUD
			var healthProp = player.Get("health");
			var bloodProp = player.Get("blood_level");

			if (healthProp.VariantType == Variant.Type.Int)
				UpdateHealth(healthProp.AsInt32());

			if (bloodProp.VariantType == Variant.Type.Int)
				UpdateBlood(bloodProp.AsInt32());
		}
		else
		{
			GD.PrintErr("Player node not found in scene!");
		}
	}

	private void UpdateHealth(int val)
	{
		if (HealthLabel != null)
			HealthLabel.Text = $"Health: {val}";
	}

	private void UpdateBlood(int val)
	{
		if (BloodLabel != null)
			BloodLabel.Text = $"Blood: {val} / 3";
	}
}
