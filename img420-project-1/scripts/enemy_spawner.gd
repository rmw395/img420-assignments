extends Node2D

@export var enemy_scene: PackedScene
@export var spawn_count := 3
@export var spawn_area_size := Vector2(2000, 2000)

func _ready():
	spawn_enemies()

func spawn_enemies():
	for i in range(spawn_count):
		spawn_single_enemy()

func spawn_single_enemy():
	var enemy = enemy_scene.instantiate()
	add_child(enemy)

	enemy.global_position = Vector2(
		randf_range(global_position.x - spawn_area_size.x/2,
					global_position.x + spawn_area_size.x/2),
		randf_range(global_position.y - spawn_area_size.y/2,
					global_position.y + spawn_area_size.y/2)
	)

@export var respawn_delay := 10.0 # seconds

func _process(delta):
	if get_child_count() < spawn_count:
		if not is_processing():
			set_process(true)
			await get_tree().create_timer(respawn_delay).timeout
			spawn_single_enemy()
			set_process(false)
