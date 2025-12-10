extends CharacterBody2D

@export var move_speed := 100.0
var wander_dir = Vector2.ZERO
var change_timer := 0.0

func _process(delta):
	change_timer -= delta
	
	if change_timer <= 0:
		wander_dir = Vector2(randf_range(-1, 1), randf_range(-1, 1)).normalized()
		change_timer = randf_range(1, 3)

func _physics_process(delta):
	velocity = wander_dir * move_speed
	move_and_slide()

func _on_npc_body_entered(body: Node2D) -> void:
	if body.is_in_group("player"):
		body.collect_blood(1)
		queue_free()
