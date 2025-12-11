using Godot;

public partial class Boid : Node2D
{
	public Vector2 Velocity = Vector2.Zero;

	[Export] public float Speed { get; set; } = 120f;

	public override void _Ready()
	{
		Velocity = new Vector2(
			GD.RandRange(-1, 1),
			GD.RandRange(-1, 1)
		).Normalized() * Speed;
	}

	public void ApplySteering(Vector2 steering)
	{
		Velocity = (Velocity + steering).Normalized() * Speed;
	}

	public override void _PhysicsProcess(double delta)
	{
		GlobalPosition += Velocity * (float)delta;
	}
}
