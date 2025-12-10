extends Node2D

# level boundaries (used for clamping positions)
@export var min_bounds := Vector2(0, 0)
@export var max_bounds := Vector2(1920, 1080)

# spawners
@onready var npc_spawner := $NPCSpawner
@onready var enemy_spawner := $EnemySpawner

# player
@onready var player := $Player

func _ready():
	# set boundaries for player
	if player:
		player.min_bounds = min_bounds
		player.max_bounds = max_bounds

	# spawn npcs and set their boundaries
	if npc_spawner:
		for npc in npc_spawner.get_children():
			if npc.has_method("set_bounds"):
				npc.set_bounds(min_bounds, max_bounds)
			else:
				npc.min_bounds = min_bounds
				npc.max_bounds = max_bounds

	# spawn enemies and set their boundaries
	if enemy_spawner:
		for enemy in enemy_spawner.get_children():
			if enemy.has_method("set_bounds"):
				enemy.set_bounds(min_bounds, max_bounds)
			else:
				enemy.min_bounds = min_bounds
				enemy.max_bounds = max_bounds
