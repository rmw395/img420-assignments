extends Node2D

@export var enemy_scene: PackedScene = preload("res://scenes/enemy.tscn")
@export var spawn_count := 3
@export var spawn_area_size := Vector2(2000, 2000)

# Track current enemies
var enemies: Array = []

func _ready():
	for i in range(spawn_count):
		spawn_enemy()

func spawn_enemy():
	if enemy_scene == null:
		push_error("enemy_scene not assigned!")
		return

	var enemy = enemy_scene.instantiate()
	enemy.global_position = Vector2(
		randf_range(global_position.x - spawn_area_size.x/2,
					global_position.x + spawn_area_size.x/2),
		randf_range(global_position.y - spawn_area_size.y/2,
					global_position.y + spawn_area_size.y/2)
	)
	
	# Assign level bounds
	enemy.min_bounds = Vector2.ZERO
	enemy.max_bounds = spawn_area_size

	# Connect the enemy signal for respawn
	enemy.connect("enemy_hit_player", Callable(self, "_on_enemy_hit_player"))
	
	add_child(enemy)
	enemies.append(enemy)

func _on_enemy_hit_player(enemy):
	enemies.erase(enemy)
	spawn_enemy()
