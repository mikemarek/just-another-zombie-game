# Game Engine Overview

## Components

Components allow game actors to perform all of their tasks during the game. Actors can gain certain functionality by being given a component. This functionality can vary from providing a simple health counter to a full inventory management system. *Universal* components can either be given to a generic actor, or are used to provide base inheritance for actor-specific components (which are classified and attached to specific actor types).

## Controllers

Controllers are objects that read Unity3D input and converts it to specific game controller inputs that the game can use. This abstraction allows us to maintain standard Unity3D inputs while allowing support for a variety of external game controllers (PS3/4, XBox 360/One, etc.)

## Environment

Scripts that modify or change the game world during game play are placed here. This includes environmental visual effects, animations, doodad placement, destruction scripts, etc.

## Equippables

*Equippables*, or, more generically, *items*, are picked up and equipped by actors and used during the game. This includes items with and without functionality (healing items vs. ammunition packs), as well as weapons and firearms.

## Interactables

Interactables allow interaction between a specific object and a player. The interactable also provides an outlet for displaying prompt messages, and collision zones that activate the interaction. For example, item containers, doors, and readable signs all fall under this category.

## Managers

Managers are the highest abstraction of the game logic, and thus control the flow of the game. These tasks range from player and camera setup, level generation, scene and menu management, and handling of global game data and structures.

## Projectiles

The stuff that comes out the end of your gun. *Projectiles* come in two flavours - hitscan and dynamic. **Hitscan projectiles** utilize raytracing to determine where the projectile will collide and affect instantaneously. **Dynamic projectiles** are your classic 'physics-based' projectile that travel through the air at a finite speed until colliding with something.

## States

States allow for the transition of functionality for game entities. There are two types of states - **actor states** and **weapon states**.

*Actor states* provide the core functionality for game actors. A state can essentially enable or disable components attached to an actor, as well as call functions on that component, which allow the actor to actually utilize the components.

* *technical note*: Actor states are structured as a [pushdown automaton](https://en.wikipedia.org/wiki/Pushdown_automaton "Pushdown Automaton")

*Weapon states* provide functionality for the various weapons in the game. A weapon starts out in its *idle state*, and when fired, transitioned to a *firing state*. By swapping out what these *firing states* are, we can gain a vast amount of flexibility and functionality out of a small amount of code.

* *technical note*: Weapon states are structured as a [non-deterministic finite automaton](https://en.wikipedia.org/wiki/Nondeterministic_finite_automaton "Non-Deterministic Finite Automaton")

## Structures

Various in-game data structures are stored here. Nothing special. Stuff like integer vectors, controller button maps, etc.

## UI

The player has both a HUD and inventory display. Containers display the contents that they hold. Code under this category is responsible for controlling and displaying in-game UI elements.
