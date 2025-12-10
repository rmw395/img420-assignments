extends Node

func _ready():
	var player = get_tree().get_first_node_in_group("player")
	
	player.health_changed.connect(_on_health_changed)
	player.blood_changed.connect(_on_blood_changed)
	player.player_died.connect(_on_player_died)
	player.player_won.connect(_on_player_won)

func _on_health_changed(new_health):
	pass  # HUD handles display

func _on_blood_changed(new_blood):
	pass  # HUD handles display

func _on_player_died():
	get_tree().change_scene_to_file("res://scenes/main_menu.tscn")

func _on_player_won():
	var player = get_tree().get_first_node_in_group("player")
	player.call_deferred("queue_free")
	get_tree().change_scene_to_file("res://scenes/main_menu.tscn")
