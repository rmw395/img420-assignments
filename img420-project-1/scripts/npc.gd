extends CharacterBody2D

@export var move_speed := 100.0
var wander_dir = Vector2.ZERO
var change_timer := 0.0

@export var rotation_speed = 30.0
@export var pulse_speed = 2.0
@export var pulse_amplitude = 0.2

@export var min_bounds := Vector2(0, 0)
@export var max_bounds := Vector2(1152, 648)

func _ready():
	# Random initial direction
	wander_dir = Vector2(randf_range(-1,1), randf_range(-1,1)).normalized()

func _process(delta):
	set_rotation_degrees(get_rotation_degrees() + rotation_speed * delta);

	change_timer -= delta
	
	if change_timer <= 0:
		wander_dir = Vector2(randf_range(-1, 1), randf_range(-1, 1)).normalized()
		change_timer = randf_range(1, 3)

func _physics_process(delta):
	velocity = wander_dir * move_speed
	move_and_slide()
	
	if global_position.x <= min_bounds.x or global_position.x >= max_bounds.x:
		wander_dir.x = -wander_dir.x
	if global_position.y <= min_bounds.y or global_position.y >= max_bounds.y:
		wander_dir.y = -wander_dir.y
		
	global_position = global_position.clamp(min_bounds, max_bounds)

func _on_npc_body_entered(body: Node2D) -> void:
	if body.is_in_group("player"):
		body.collect_blood(1)
		call_deferred("queue_free")
