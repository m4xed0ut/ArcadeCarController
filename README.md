# Arcade-RaycastCarController
Second go at writing a vehicle controller from the ground up using C# and Unity after taking notes from my DroneController.

This one's a simple script Arcade car controller that needs only a single rigidbody component with a box collider and 2 target force objects with Trigger Colliders (one in the front, one beside the car), and where the wheels that the script animates are just transforms.

I also wrote the controller's raycast to only disable the car's forces when it is not grounded and not to freeze the rigidbody's rotation to make it more natural.

Like my Drone Controller, it uses Unity's new Input System (although with a few lines making use of the old Input Manager).

# Controls

Up Arrow - Throttle

Down Arrow - Brakes/Reverse

Left, Right Arrow - Steering

Space - Handbrake
