extends CharacterBody2D

@export var move_speed := 200.0
@export var max_health := 100
@export var max_blood := 3

var health := max_health
var blood_level := 0

signal health_changed(new_health)
signal blood_changed(new_blood)
signal player_died
signal player_won

func _physics_process(delta):
	var input_vector = Vector2(
		Input.get_action_strength("move_right") - Input.get_action_strength("move_left"),
		Input.get_action_strength("move_down") - Input.get_action_strength("move_up")
	).normalized()

	velocity = input_vector * move_speed
	move_and_slide()

func take_damage(amount: int):
	health -= amount
	emit_signal("health_changed", health)

	if health <= 0:
		emit_signal("player_died")

func collect_blood(amount: int = 1):
	blood_level += amount
	emit_signal("blood_changed", blood_level)

	if blood_level >= max_blood:
		emit_signal("player_won")
