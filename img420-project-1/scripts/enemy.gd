extends CharacterBody2D

signal enemy_hit_player

@export var speed := 150
@export var min_bounds := Vector2.ZERO
@export var max_bounds := Vector2(1152, 648)

# Reference to player for chasing
var player: Node = null

func _ready():
	# Try to find the player in the scene
	player = get_tree().current_scene.get_node("Player")
	if player == null:
		push_warning("Enemy cannot find Player node!")

func _physics_process(delta):
	if player:
		var dir = (player.global_position - global_position).normalized()
		velocity = dir * speed
		move_and_slide()
	
	# Keep within bounds
	global_position = global_position.clamp(min_bounds, max_bounds)

func _on_body_entered(body):
	if body.is_in_group("player"):
		emit_signal("enemy_hit_player", self)
		call_deferred("queue_free")
