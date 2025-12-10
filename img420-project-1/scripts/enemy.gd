extends CharacterBody2D

@export var move_speed := 150.0
var player: Node2D

@export var min_bounds := Vector2(0, 0)
@export var max_bounds := Vector2(1920, 1080)

func _ready():
	player = get_tree().get_first_node_in_group("player")

func _physics_process(delta):
	if player:
		var dir = (player.global_position - global_position).normalized()
		velocity = dir * move_speed
		move_and_slide()
		global_position = global_position.clamp(min_bounds, max_bounds)

func _on_enemy_body_entered(body: Node2D) -> void:
	if body.is_in_group("player"):
		body.take_damage(10)
