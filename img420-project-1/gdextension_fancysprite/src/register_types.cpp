#include <godot_cpp/godot.hpp>
#include <godot_cpp/core/defs.hpp>
#include <godot_cpp/core/class_db.hpp>
#include <godot_cpp/classes/engine.hpp>

#include "FancySprite.hpp"

using namespace godot;

static GDExtensionClassLibraryPtr library;

extern "C" {

// This function runs when the extension is loaded
GDExtensionBool GDE_EXPORT fancysprite_library_init(
        GDExtensionInterfaceGetProcAddress p_get_proc_address,
        const GDExtensionClassLibraryPtr p_library,
        GDExtensionInitialization *r_initialization) {

    GDExtensionBinding::InitObject init_obj(p_get_proc_address, p_library, r_initialization);

    init_obj.register_initializer([](){
        ClassDB::register_class<FancySprite>();
    });

    init_obj.register_terminator([](){
        // nothing needed, but required
    });

    init_obj.set_minimum_library_initialization_level(MODULE_INITIALIZATION_LEVEL_SCENE);

    return init_obj.init();
}

} // extern "C"
