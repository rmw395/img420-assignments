extends Node

var player: Node = null

func _ready():
	player = get_tree().get_first_node_in_group("player")
	
	player.health_changed.connect(_on_health_changed)
	player.blood_changed.connect(_on_blood_changed)
	player.player_died.connect(_on_player_died)
	player.player_won.connect(_on_player_won)

func _on_health_changed(new_health):
	pass  # HUD handles display

func _on_blood_changed(new_blood):
	pass  # HUD handles display

func _process(delta):
	if player.health <= 0:
		_on_player_died()
	elif player.blood_level >= 3:
		_on_player_won()

func _on_player_died():
	print("You lost!")
	get_tree().paused = true
	$LoseLabel.visible = true
	get_tree().change_scene_to_file("res://scenes/main_menu.tscn")

func _on_player_won():
	print("You won!")
	get_tree().paused = true
	$WinLabel.visible = true
	player.call_deferred("queue_free")
	get_tree().change_scene_to_file("res://scenes/main_menu.tscn")
