# Arcade-RaycastCarController
Second go at writing a vehicle controller from the ground up using C# and Unity after taking notes from my DroneController. This time it's an arcade car controller, which is a lot more advanced!

The goal for my new challenge was to write a script for operating a 3D car with a single rigidbody object with a box collider, NO wheel colliders and 2 target force objects with Trigger Colliders (one in the front, one beside the car), and where the wheels that the script animates are just transforms.

The handling of the controller is inspired by games like OutRun, Ridge Racer and DiRT 5. 

I also wrote the controller's raycast to only disable the car's forces when it is not grounded and not to freeze the rigidbody to make it more natural.

# Controls

Up Arrow - Throttle

Down Arrow - Brakes/Reverse

Left, Right Arrow - Steering

Space - Handbrake
