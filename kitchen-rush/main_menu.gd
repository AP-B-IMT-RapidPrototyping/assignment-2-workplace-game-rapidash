extends Control

func _ready():
	$VBoxContainer/Button.pressed.connect(_on_play_pressed)
	$VBoxContainer/Button2.pressed.connect(_on_options_pressed)
	$VBoxContainer/Button3.pressed.connect(_on_exit_pressed)

func _on_play_pressed():
	get_tree().change_scene_to_file("res://scenes/Game.tscn")  # Pas aan naar jouw spel-scène

func _on_options_pressed():
	get_tree().change_scene_to_file("res://scenes/Options.tscn")  # Of open een popup

func _on_exit_pressed():
	get_tree().quit()
