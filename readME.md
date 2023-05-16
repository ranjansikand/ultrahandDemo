# Ultrahand Recreation

This is a quick recreation of the Ultrahand ability in Unity (C#).

## Inspiration
Obviously, Tears of the Kingdom was the inspiration for this project. You can use the Ultrahand ability to manipulate objects and build new ones. 

## Implementation

###Player
- Mostly handles input
- Controls a particle system and particle field for the beam effect

###IManipulable
- Interface extended by manipulable objects
- Contains functions called by player on objects, or called by objects on one another

###Blocks
- The blocks perform most of the functionality of the script
- Grab moves the block to the screen's centern point using velocities, to act nicer with collisions and create a more appealing motion.
- Attaching and detaching is a little sloppy, but the system works using Fixed Joints. These allow two rigidbodies to snap together, so movement is joined. Using transforms also worked, but it was a much messier solution.
- Rotation simply adjusts the direction of the object relative to the camera. Objects generally rotate as expected, but occasionally the jointing system can cause strange behavior.


