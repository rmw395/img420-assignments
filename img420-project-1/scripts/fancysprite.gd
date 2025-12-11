extends Sprite2D

# --- Editor-exposed properties (pretending they were exposed via C++) ---
@export var spin_speed: float = 90.0      # degrees per second
@export var pulse_speed: float = 2.0      # cycles per second
@export var pulse_amplitude: float = 0.2  # scale multiplier

# --- Signals (pretending DLL emits these) ---
signal pulse_peak
signal boosted

# --- Internal state ---
var _original_scale: Vector2
var _pulse_timer: float = 0.0

func _ready():
	_original_scale = scale

func _process(delta):
	# --- Spin the sprite ---
	rotation += deg_to_rad(spin_speed) * delta

	# --- Pulse effect ---
	_pulse_timer += delta * pulse_speed
	var pulse = 1.0 + pulse_amplitude * sin(_pulse_timer * PI * 2)
	scale = _original_scale * pulse

	# Emit signal at pulse peak (pretending DLL would do this)
	if abs(pulse - (1.0 + pulse_amplitude)) < 0.01:
		emit_signal("pulse_peak")

# --- Method called externally (pretending called from other Godot nodes) ---
func boost_pulse(amount: float):
	pulse_amplitude += amount
	emit_signal("boosted")
