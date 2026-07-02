# Haunted Maze

A procedural 3D maze game where the player must navigate a haunted environment, collect gems, and avoid roaming zombies.

## Game Design & Logic

### 1. Procedural Map Generation (`MapManager.cs`)
- **Image-Based Layouts**: The maze layout is procedurally generated from a set of 2D textures. The `MapManager` reads pixel data from a randomly selected texture: pixels matching the wall color (black) become walls, and transparent pixels become open path spaces.
- **Dynamic Spawning**:
  - **Gems**: 2 gems are randomly spawned in the available open path positions.
  - **Zombies**: 5 zombies are randomly spawned in the maze.
  - **Lighting**: 35 point lights are distributed throughout the maze. An algorithm ensures they are evenly spread out by selecting valid NavMesh positions that maximize the distance from all other previously placed lights.

### 2. Player Mechanics (`Player.cs`)
- **Movement & Controls**: The player uses standard inputs to move relative to their facing direction. The mouse cursor is locked and hidden during active gameplay.
- **Audio/Visual Feedback**: Moving triggers footstep sounds and updates the running animation state.
- **Enemy Encounters**: If the player collides with a zombie, movement is disabled, footsteps stop, a scream sound effect plays, a death animation is triggered, and the Game Over screen appears.

### 3. Win/Loss Conditions
- **Victory**: The player must collect all spawned gems (2 total). Upon collecting the final gem, zombies are disabled and a success Game Over screen is shown.
- **Defeat**: The player is caught by a zombie.
