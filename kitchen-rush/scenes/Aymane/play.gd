extends Node3D

func _ready():
	$Area3D.input_event.connect(_on_input_event)

func _on_input_event(camera, event, position, normal, shape_idx):
	if event is InputEventMouseButton and event.pressed:
		get_tree().change_scene_to_file("res://scenes/Main.tscn")
