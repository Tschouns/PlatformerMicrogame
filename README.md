# PlatformerMicrogame
Contains the Unity sample *Platformer Microgame*, modded for educational purposes.

## Mods
### Restart, Quick Save, Quick Load
New buttons have been added to the main menu (press "Esc" to open):
* **Restart** - restarts the scene, restoring its original states.
* **Quick Save** - saves the scene.
* **Quick Load** - loads the latest savegame for the scene.

#### Implementation
A new *SceneLifecycleController* class was created, and added as a component to the *GameController* object. It exposes three methods -- *ReloadScene()*, *QuickSave()*, and *QuickLoad()* -- which are called when the aforementioned menu buttons are pressed.

*QuickSave* collects the relevant state data, serializes and saves them to a savegame file. *QuickLoad* selects the latest savegame (if any), reloads the scene, and applies the state data from the savegame back to the scene.

The *SceneLifecycleController* contains a *SceneObjectRegister* instance, which collects and holds all the game objects relevant for saving/loading, i.e. game objects with a variable state. The game objects which are saved/loaded are:
- the player
- all the enemies
- all the collectable tokens

The player is unique. The enemies and tokens, however, are not. Thus, for the purpose of saving/loading, they are identified by a key which is derived from their starting position (which is assumed to be unique within the scene).

The player, enemies and tokens each have a corresponding "state" type (e.g. the *PlayerState* class) and a controller which implements a new interface called *IHasState<TState>*. The *IHasState<TState>* interface exposes two methods -- *GetCurrentState()* and *ApplyState(TState state)* -- to retrieve the current state from an object, and apply it back to the object. The controllers (implementations of *IHasState<TState>*) are responsible for collecting and applying all the relevant state data of their corresponding game object.

#### Known Issues
The *Platformer Microgame* probably was't designed for saving/loading the scene's state completely and perfectly. For simplicity's sake, only the important state information is saved and restored on load -- e.g. positions, whether they're dead or alive, etc...

* Animations are not perfectly preserved. A player or enemy might look slightly different
* Enemies might move very and in a weird way after a quick load. The *Mover* class determines the enemies target positions by a formula based on elapsed game time since the start -- rather than updating it incrementally and storing the current position in a variable (which could then be set after a load). This could be fixed by rewriting the *Mover* class.

## Packages
For the implementation NuGet packages were used. To manage packages, [NuGetForUnity](https://github.com/GlitchEnzo/NuGetForUnity/releases) was used, which might be required in order to build the project.