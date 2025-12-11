# img420-assignments

### Flocking (Boids) — Assignment 3

This project implements Craig Reynolds' Boids flocking algorithm to create groups of autonomous agents that move with lifelike cohesion, alignment, and separation. The flocking behavior is implemented in `Boid.cs` and instantiated via `BoidSpawner.cs`.

**How it's used in the game**
- A `BoidSpawner` places a flock of Boid instances (pigs) into the level.
- Each Boid computes three steering forces each frame:
  - **Separation** — avoid crowding neighbors,
  - **Alignment** — align direction to match neighbors,
  - **Cohesion** — move toward the average position of neighbors.
- Forces are clamped with `MaxForce` and velocities are limited by `MaxSpeed`, which prevents jitter and produces smooth flock motion.

**Design/feel improvements**
- Flocks add life to the level. Groups of pigs move together realistically instead of wandering independently.
- The emergent behavior from simple rules makes the environment feel dynamic and believable, improving player immersion.
- Inspector-exposed parameters allow quick tuning of flock tightness, speed, and responsiveness, so we can adjust difficulty and visual style without code changes.

**Files**
- `scripts/Boid.cs` — the flocking agent (separation/alignment/cohesion).
- `scenes/Boid.tscn` — Boid scene (CharacterBody2D + Sprite2D).
- `scripts/BoidSpawner.cs` — spawns and registers boids for neighbor queries.
