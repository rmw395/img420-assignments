extends CharacterBody2D

@export var move_speed := 150.0
var player: Node2D

func _ready():
	player = get_tree().get_first_node_in_group("player")

func _physics_process(delta):
	if player:
		var dir = (player.global_position - global_position).normalized()
		velocity = dir * move_speed
		move_and_slide()
