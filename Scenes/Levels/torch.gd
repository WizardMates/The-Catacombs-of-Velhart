extends OmniLight3D

@export var base_energy = 2.0
@export var flicker_speed = 3.0
@export var flicker_amount = 0.3

func _process(delta):
	var flicker = sin(get_tree().get_frame() * flicker_speed * delta) * flicker_amount
	light_energy = base_energy + flicker
