extends CharacterBody2D

@export var move_speed := 200.0
@export var max_health := 100
@export var max_blood := 3

@export var min_bounds := Vector2(0, 0)
@export var max_bounds := Vector2(1152, 648)

var health := max_health
var blood_level := 0

signal health_changed(new_health)
signal blood_changed(new_blood)
signal player_died
signal player_won

func _physics_process(delta):
	var input_vector = Vector2.ZERO
	input_vector.x = Input.get_action_strength("move_right") - Input.get_action_strength("move_left")
	input_vector.y = Input.get_action_strength("move_down") - Input.get_action_strength("move_up")
	velocity = input_vector.normalized() * 200

	move_and_slide()

	global_position = global_position.clamp(min_bounds, max_bounds)

func take_damage(amount: int):
	health -= amount
	emit_signal("health_changed", health)

	if health <= 0:
		emit_signal("player_died")

func collect_blood(amount: int = 1):
	blood_level += amount
	emit_signal("blood_changed", blood_level)
	
	$Sprite2D.boost_pulse(0.1)

	if blood_level >= max_blood:
		emit_signal("player_won")
