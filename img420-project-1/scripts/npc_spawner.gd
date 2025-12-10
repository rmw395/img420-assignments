extends Node2D

@export var npc_scene: PackedScene
@export var spawn_count := 5
@export var spawn_area_size := Vector2(2000, 2000) # width/height area to spawn inside

func _ready():
	spawn_npcs()

func spawn_npcs():
	for i in range(spawn_count):
		spawn_single_npc()

func spawn_single_npc():
	var npc = npc_scene.instantiate()
	add_child(npc)

	npc.global_position = Vector2(
		randf_range(global_position.x - spawn_area_size.x/2,
					global_position.x + spawn_area_size.x/2),
		randf_range(global_position.y - spawn_area_size.y/2,
					global_position.y + spawn_area_size.y/2)
	)
