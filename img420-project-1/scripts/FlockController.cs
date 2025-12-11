using Godot;
using System.Collections.Generic;

public partial class FlockController : Node2D
{
	[Export] public PackedScene BoidScene { get; set; }
	[Export] public int BoidCount { get; set; } = 50;

	[Export] public float NeighborRadius { get; set; } = 100f;
	[Export] public float SeparationRadius { get; set; } = 50f;

	[Export] public float AlignmentWeight { get; set; } = 1.0f;
	[Export] public float CohesionWeight { get; set; } = 1.0f;
	[Export] public float SeparationWeight { get; set; } = 1.5f;

	[Export] public Vector2 ScreenSize { get; set; } = new Vector2(1152, 648);

	private List<Boid> _boids = new List<Boid>();

	public override void _Ready()
	{
		for (int i = 0; i < BoidCount; i++)
			SpawnBoid();
	}

	private void SpawnBoid()
	{
		if (BoidScene == null) return;

		Boid boid = BoidScene.Instantiate<Boid>();

		boid.GlobalPosition = new Vector2(
			GD.RandRange(0, ScreenSize.X),
			GD.RandRange(0, ScreenSize.Y)
		);

		AddChild(boid);
		_boids.Add(boid);
	}

	public override void _PhysicsProcess(double delta)
	{
		foreach (var boid in _boids)
		{
			Vector2 alignment = Vector2.Zero;
			Vector2 cohesion = Vector2.Zero;
			Vector2 separation = Vector2.Zero;
			int neighborCount = 0;

			foreach (var other in _boids)
			{
				if (other == boid) continue;

				float dist = boid.GlobalPosition.DistanceTo(other.GlobalPosition);

				if (dist < NeighborRadius)
				{
					alignment += other.Velocity;
					cohesion += other.GlobalPosition;
					neighborCount++;

					if (dist < SeparationRadius)
						separation += (boid.GlobalPosition - other.GlobalPosition).Normalized() / dist;
				}
			}

			if (neighborCount > 0)
			{
				alignment = (alignment / neighborCount).Normalized();
				cohesion = ((cohesion / neighborCount) - boid.GlobalPosition).Normalized();
			}

			Vector2 newVelocity =
				alignment * AlignmentWeight +
				cohesion * CohesionWeight +
				separation * SeparationWeight;

			boid.ApplySteering(newVelocity);
			WrapBoid(boid);
		}
	}

	private void WrapBoid(Boid boid)
	{
		Vector2 pos = boid.GlobalPosition;

		if (pos.X < 0) pos.X = ScreenSize;
	}
}
