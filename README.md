# ArcadeCarController-NoWheelColliders
Second go at writing a vehicle controller from the ground up using C# and Unity.

This one's a simple Arcade car controller that only needs a single rigidbody component with a box collider and 2 target force objects with Trigger Colliders (one in the front, one beside the car), and where the wheels that the script animates are just transforms. The Raycast disables the car's input and its forces when it's not grounded.

Like my Drone Controller, it uses Unity's new Input System (although with a few lines making use of the old Input Manager).

# Controls

Up Arrow - Throttle

Down Arrow - Brakes/Reverse

Left, Right Arrow - Steering

Space - Handbrake
