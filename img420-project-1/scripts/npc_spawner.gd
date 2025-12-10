extends Node2D

@export var npc_scene: PackedScene
@export var spawn_count := 5
@export var spawn_area_size := Vector2(2000, 2000) # width/height area to spawn inside

# Keep track of current NPCs
var npcs: Array = []

func _ready():
	spawn_npcs()

func spawn_npcs():
	for i in range(spawn_count):
		spawn_single_npc()

func spawn_single_npc():
	if npc_scene == null:
		push_error("npc_scene not assigned!")
		return

	var npc = npc_scene.instantiate()
	
	# Random position inside spawn area
	npc.global_position = Vector2(
		randf_range(global_position.x - spawn_area_size.x/2,
					global_position.x + spawn_area_size.x/2),
		randf_range(global_position.y - spawn_area_size.y/2,
					global_position.y + spawn_area_size.y/2)
	)
	
	# Set bounds for NPC movement
	npc.min_bounds = Vector2(0, 0)
	npc.max_bounds = spawn_area_size

	# Connect signal from NPC so we can respawn
	npc.connect("body_entered", Callable(self, "_on_npc_collected"))

	# Add to scene and tracking array
	add_child(npc)
	npcs.append(npc)

# Called when NPC is collected by player
func _on_npc_collected(npc):
	npcs.erase(npc)
	spawn_single_npc()  # spawn a replacement NPC
