#ifndef FANCY_SPRITE_HPP
#define FANCY_SPRITE_HPP

#include <godot_cpp/classes/sprite2d.hpp>
#include <godot_cpp/core/class_db.hpp>
#include <godot_cpp/variant/utility_functions.hpp>

using namespace godot;

class FancySprite : public Sprite2D {
    GDCLASS(FancySprite, Sprite2D);

private:
    double rotation_speed;     // degrees per second
    double pulse_speed;        // cycles per second
    double pulse_amplitude;    // scale amplitude relative to 1.0 base
    double _time_accum;

public:
    FancySprite();
    ~FancySprite();

    // lifecycle
    void _process(double delta);

    // Editor-exposed setters/getters
    void set_rotation_speed(double s);
    double get_rotation_speed() const;

    void set_pulse_speed(double s);
    double get_pulse_speed() const;

    void set_pulse_amplitude(double a);
    double get_pulse_amplitude() const;

    // Method callable from Godot
    void boost_pulse(double amount);

    // binding
    static void _bind_methods();
};

#endif // FANCY_SPRITE_HPP
