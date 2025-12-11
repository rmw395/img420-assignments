#include "FancySprite.hpp"
#include <godot_cpp\core\class_db.hpp>
#include <godot_cpp\variant\utility_functions.hpp>
#include <godot_cpp\variant\variant.hpp>
#include <cmath>

using namespace godot;

FancySprite::FancySprite() {
    rotation_speed = 30.0;
    pulse_speed = 2.0;
    pulse_amplitude = 0.15;
    _time_accum = 0.0;
    // Ensure processing runs
    set_process(true);
}

FancySprite::~FancySprite() { }

void FancySprite::_process(double delta) {
    // Update time
    _time_accum += delta;

    // 1) Rotation: rotate sprite by rotation_speed degrees/sec
    double deg = Math::rad2deg(get_rotation());
    deg += rotation_speed * delta;
    // keep degrees in reasonable range
    if (deg > 360.0) {
        deg = fmod(deg, 360.0);
        // emit threshold signal when rotation wraps (example behavior)
        Variant arr[1] = { Variant((int)1) };
        // emits a signal named "threshold_reached" with an int payload
        emit_signal("threshold_reached", arr, 1);
    }
    set_rotation(Math::deg2rad(deg));

    // 2) Pulse scaling: scale = 1.0 + sin( time * 2pi * pulse_speed ) * pulse_amplitude
    double s = 1.0 + std::sin(_time_accum * 2.0 * Math_PI * pulse_speed) * pulse_amplitude;
    set_scale(Vector2((real_t)s, (real_t)s));
}

void FancySprite::set_rotation_speed(double s) {
    rotation_speed = s;
}
double FancySprite::get_rotation_speed() const {
    return rotation_speed;
}

void FancySprite::set_pulse_speed(double s) {
    pulse_speed = s;
}
double FancySprite::get_pulse_speed() const {
    return pulse_speed;
}

void FancySprite::set_pulse_amplitude(double a) {
    pulse_amplitude = a;
}
double FancySprite::get_pulse_amplitude() const {
    return pulse_amplitude;
}

// Callable method — can be called from other nodes when they emit a signal
void FancySprite::boost_pulse(double amount) {
    // temporarily increase amplitude
    pulse_amplitude += amount;
    // clamp to reasonable values
    if (pulse_amplitude < 0.0) pulse_amplitude = 0.0;
    if (pulse_amplitude > 2.0) pulse_amplitude = 2.0;
    // optionally schedule decay using time or other mechanism (left simple)
}

// Bind methods, properties and signals
void FancySprite::_bind_methods() {
    ClassDB::bind_method(D_METHOD("set_rotation_speed", "speed"), &FancySprite::set_rotation_speed);
    ClassDB::bind_method(D_METHOD("get_rotation_speed"), &FancySprite::get_rotation_speed);

    ClassDB::bind_method(D_METHOD("set_pulse_speed", "speed"), &FancySprite::set_pulse_speed);
    ClassDB::bind_method(D_METHOD("get_pulse_speed"), &FancySprite::get_pulse_speed);

    ClassDB::bind_method(D_METHOD("set_pulse_amplitude", "amplitude"), &FancySprite::set_pulse_amplitude);
    ClassDB::bind_method(D_METHOD("get_pulse_amplitude"), &FancySprite::get_pulse_amplitude);

    ClassDB::bind_method(D_METHOD("boost_pulse", "amount"), &FancySprite::boost_pulse);

    // Properties: show up in the editor
    ADD_PROPERTY(PropertyInfo(Variant::FLOAT, "rotation_speed", PROPERTY_HINT_NONE, "", PROPERTY_USAGE_DEFAULT, "Rotation speed (deg/s)"),
                 "set_rotation_speed", "get_rotation_speed");
    ADD_PROPERTY(PropertyInfo(Variant::FLOAT, "pulse_speed", PROPERTY_HINT_NONE, "", PROPERTY_USAGE_DEFAULT, "Pulse frequency (Hz)"),
                 "set_pulse_speed", "get_pulse_speed");
    ADD_PROPERTY(PropertyInfo(Variant::FLOAT, "pulse_amplitude", PROPERTY_HINT_NONE, "", PROPERTY_USAGE_DEFAULT, "Pulse amplitude (scale)"),
                 "set_pulse_amplitude", "get_pulse_amplitude");

    // Register signal "threshold_reached" with one int parameter "count"
    // The ClassDB::register_signal template is available in godot-cpp; if not, fallback to ClassDB::register_signal with Array of PropertyInfo.
    Array sig_args;
    sig_args.append(PropertyInfo(Variant::INT, "count"));
    ClassDB::register_signal("FancySprite", "threshold_reached", sig_args);
}
