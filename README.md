# CarController
Second go at writing a vehicle controller from the ground up using C# and Unity.

This one's a simple Arcade car controller that only needs a single rigidbody component with a box collider and two target force objects, one in front, one next to the car, and where the wheels that the script animates are just transforms. The Raycast disables the car's input and its forces when it's not grounded.

# Controls

Up Arrow - Throttle

Down Arrow - Brakes

Left, Right Arrow - Steering

Space - Handbrake
