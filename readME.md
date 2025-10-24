# Ultrahand Recreation

A quick recreation of the **Ultrahand** ability from *The Legend of Zelda: Tears of the Kingdom*, built in **Unity (C#)**.  
This prototype focuses on replicating the feel of manipulating and fusing objects together in a physically consistent and responsive way.

---

## Overview

The project simulates the **Ultrahand** mechanic — allowing the player to grab, move, rotate, and attach objects dynamically within a 3D environment.  
The emphasis is on smooth object handling, accurate physics interactions, and readable feedback through particles and visual effects.

---

## Inspiration

This project was inspired by *The Legend of Zelda: Tears of the Kingdom*.  
In the original game, Link uses the Ultrahand ability to lift and assemble objects into creative structures.  
This recreation focuses on capturing that same tactile and systemic feel — the sense of “grabbing the world” and making it behave intuitively.

---

## Implementation Details

### Player
- Handles **input management** and player control logic.  
- Controls a **particle system and particle field** used for the Ultrahand beam effect.  
- Sends interaction commands to objects that implement the `IManipulable` interface.  

### IManipulable
- Interface extended by any object that can be manipulated.  
- Defines core interaction functions — called by the player or by other manipulable objects.  
- Ensures modular and consistent behavior across different object types.  

### Blocks
- Contain most of the system’s **functional logic**.  
- **Grabbing:** Moves the block toward the screen’s center point using velocity-based movement for smooth, collision-friendly motion.  
- **Attaching and Detaching:** Uses **Fixed Joints** to connect rigidbodies. This provides more stable physics behavior than direct transform parenting.  
  - Transform-based connections were tested but produced less predictable motion.  
- **Rotation:** Adjusts block orientation relative to the camera for intuitive control.  
  - The system generally behaves as expected, though jointing physics can occasionally cause unexpected rotations.  

---

## Technical Focus

This project demonstrates:
- Object interaction systems in Unity.  
- Physics-based movement and joint constraints.  
- Modular interface design for reusable object behavior.  
- Visual feedback through particle systems.  

---

## Technologies

**Engine:** Unity (C#)  
**Focus Areas:** Physics systems, input handling, modular interaction architecture  
**Status:** Prototype  

---

## Author

**Ranjan Sikand**  
Game Designer & Developer  
[LinkedIn](https://www.linkedin.com/in/ranjan-sikand) | [GitHub](https://github.com/ranjansikand)
