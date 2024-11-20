# Procedural Space Environment in Godot 4

A procedural space environment built with Godot 4 and C#. Features volumetric fog, procedural objects for orientation, and a flexible camera system.

![Space Environment Screenshot](/.github/assets/screenshots/game.png)
![Godot Editor Screenshot](/.github/assets/screenshots/godot.png)

## Overview

This project is procedural environment generation in Godot 4 using C# as always. All objects, fog, and lighting are generated at runtime without pre-made assets.

## Project Structure

```sh
.
├── scenes/
│ ├── camera/ # Camera control system
│ ├── environment/ # Space environment and fog
│ ├── space_objects/ # Procedural space objects
│ └── main_3d.tscn # Main scene
├── resources/ # Resource files (empty)
└── scripts/
└── utils/ # Utility scripts
````

## Controls

I'm too lazy and it's early in the morning for this but here we go, basic controls for the game:

- **WASD/Arrow Keys**: Movement
- **Space/E**: Move Up
- **Q**: Move Down
- **Right Mouse Button**: Rotate Camera
- **Middle Mouse Button**: Pan Camera
- **Mouse Wheel**: Zoom
- **Shift**: Speed Boost
- **Tab**: Switch Camera Modes
  - Fly Mode: Free flight
  - Orbit Mode: Rotate around point
  - Pan Mode: Top-down view

## Technical Details

- Built with Godot 4.4 (.NET Version!)
- Pure C# implementation
- Procedural generation for all objects
- Volumetric fog system
- Dynamic lighting system
- No external assets or dependencies

## Getting Started

1. Clone the repository and open it in godot mono lol.
