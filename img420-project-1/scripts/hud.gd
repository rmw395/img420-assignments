extends CanvasLayer

var player

func _ready():
	player = get_tree().get_first_node_in_group("player")

	player.health_changed.connect(_update_health)
	player.blood_changed.connect(_update_blood)

	_update_health(player.health)
	_update_blood(player.blood_level)

func _update_health(val):
	$HealthLabel.text = "Health: %s" % str(val)

func _update_blood(val):
	$BloodLabel.text = "Blood: %s / 3" % str(val)
