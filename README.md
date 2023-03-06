# Arcade-RaycastCarController
Second go at writing a vehicle controller from the ground up. This time it's an arcade, raycast-based car controller!

The goal for my new challenge was to write a script for operating a 3D car with a single rigidbody object with a box collider and NO wheel colliders, inspired by games like OutRun, Ridge Racer or DiRT 5. 

I also wrote the controller's raycast to only disable the car's forces when it is not grounded and not to freeze the rigidbody to make it more natural.
