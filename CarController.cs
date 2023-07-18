using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Gear
{
    public float minPitch;
    public float maxPitch;
    public float minSpeed;
    public float maxSpeed;
}

public class CarController : MonoBehaviour
{
    float raycastDistance = 1;
    private bool isGrounded = true;
    private float fallSpeed = 1000;
    private float accel;

    [Header("Car Setup")]
    public float power;
    public float steerSpeed = 1f;
    public float handBrake = 1.3f;
    public float weight = 800;
    public float topSpeed;
    public bool grip = true;

    [Header("Car Parts")]
    public GameObject car;
    public Transform wheelFL;
    public Transform wheelFR;
    public Transform wheelRL;
    public Transform wheelRR;

    [Header("Target Force")]
    public Transform targetForward;
    public Transform targetLeft;

    [Header("Raycast")]
    public Transform raycastTarget;
    public LayerMask ground;

    [Header("SFX")]
    public AudioSource engine;
    public Gear[] gears;
    public int currentGearIndex = 0;
    private float targetPitch;
    private float currentPitch;
    public float pitchSpeed = 5f;

    [Header("VFX")]
    public GameObject brakes;
    public GameObject shadow;


    void Start()
    {
        car.GetComponent<Rigidbody>().mass = weight;

    }

    void Update()
    {

        if (isGrounded)
        {
            shadow.SetActive(true);
        }
        else
        {
            shadow.SetActive(false);
        }

        if (car.GetComponent<Rigidbody>().velocity.magnitude >= 1)
        {
            if (isGrounded)
            {
                car.transform.Rotate(0, Input.GetAxis("Horizontal") * steerSpeed * Time.deltaTime, 0, Space.Self);

                if (Keyboard.current.downArrowKey.isPressed)
                {
                    car.GetComponent<Rigidbody>().drag = 0.5f;
                    brakes.SetActive(true);
                }
                else
                {
                    car.GetComponent<Rigidbody>().drag = 0;
                    brakes.SetActive(false);
                }
            }
            else
            {
                car.GetComponent<Rigidbody>().drag = 0;
                brakes.SetActive(false);
            }

            wheelFL.localEulerAngles = new Vector3(0, 0, 90) + Vector3.up * Mathf.Clamp(Input.GetAxis("Horizontal") * 100, -45, 45);
            wheelFR.localEulerAngles = new Vector3(0, 0, 90) + Vector3.up * Mathf.Clamp(Input.GetAxis("Horizontal") * 100, -45, 45);

            wheelFL.transform.Rotate(0, Input.GetAxis("Vertical") * accel * 1000 * Time.deltaTime, 0);
            wheelFR.transform.Rotate(0, Input.GetAxis("Vertical") * accel * 1000 * Time.deltaTime, 0);
            wheelRL.transform.Rotate(0, Input.GetAxis("Vertical") * accel * 1000 * Time.deltaTime, 0);
            wheelRR.transform.Rotate(0, Input.GetAxis("Vertical") * accel * 1000 * Time.deltaTime, 0);

            if (Keyboard.current.spaceKey.isPressed)
            {
                if (isGrounded)
                    car.transform.Rotate(0, Input.GetAxis("Horizontal") * handBrake * Time.deltaTime, 0, Space.Self);

                wheelRL.localEulerAngles = new Vector3(0, 0, 90);
                wheelRR.localEulerAngles = new Vector3(0, 0, 90);

            }
        }

        // Calculate the current speed based on the rigidbody's velocity magnitude
        float speed = car.GetComponent<Rigidbody>().velocity.magnitude;

        // Get the current gear
        Gear currentGear = gears[currentGearIndex];

        // Calculate the target pitch based on the current speed
        float normalizedSpeed = Mathf.InverseLerp(currentGear.minSpeed, currentGear.maxSpeed, speed);
        targetPitch = Mathf.Lerp(currentGear.minPitch, currentGear.maxPitch, normalizedSpeed);

        // Smoothly change the pitch towards the target pitch
        currentPitch = Mathf.Lerp(currentPitch, targetPitch, Time.deltaTime * pitchSpeed);

        // Apply the pitch to the audio source
        engine.pitch = currentPitch;

        // Check if we need to shift gears
        if (speed > currentGear.maxSpeed && currentGearIndex < gears.Length - 1)
        {
            currentGearIndex++;
        }
        else if (speed < currentGear.minSpeed && currentGearIndex > 0)
        {
            currentGearIndex--;
        }
    }

    void FixedUpdate()
    {
        RaycastHit groundHit;

        if (Physics.Raycast(raycastTarget.position, -raycastTarget.up, out groundHit, raycastDistance, ground))
        {
            isGrounded = true;
            Debug.DrawRay(raycastTarget.position, -raycastTarget.up * 10);
            Debug.Log("Car is grounded");
        }
        else
        {
            isGrounded = false;
            Debug.DrawRay(raycastTarget.position, -raycastTarget.up * 10);
            Debug.Log("Car is NOT grounded");
        }

        // Downforce applied to the rigidbody
        car.GetComponent<Rigidbody>().AddForce(Vector3.down * fallSpeed * 1000 * Time.deltaTime);

        if (isGrounded)
        {
            // Keyboard controls - Throttle, Reverse/Brake, Handbrake
            if (Keyboard.current.upArrowKey.isPressed)
            {
                car.GetComponent<Rigidbody>().AddForce(targetForward.forward * accel * 1000 * Time.deltaTime);

                if (Keyboard.current.spaceKey.isPressed)
                {

                    car.GetComponent<Rigidbody>().AddForce(-targetForward.forward * accel * 1000 * Time.deltaTime);
                }

                if (Keyboard.current.rightArrowKey.isPressed)
                {
                    if (grip && car.GetComponent<Rigidbody>().velocity.magnitude >= 70)
                    {
                        car.GetComponent<Rigidbody>().AddForce(targetLeft.right * accel * 1000 * Time.deltaTime);
                    }
                }

                if (Keyboard.current.leftArrowKey.isPressed)
                {
                    if (grip && car.GetComponent<Rigidbody>().velocity.magnitude >= 70)
                    {
                        car.GetComponent<Rigidbody>().AddForce(-targetLeft.right * accel * 1000 * Time.deltaTime);
                    }
                }

            }
            else
            {
                car.GetComponent<Rigidbody>().AddForce(Vector3.down * fallSpeed * 1000 * Time.deltaTime);
            }
        }

        if (car.GetComponent<Rigidbody>().velocity.magnitude >= topSpeed)
        {
            accel = 0;
        }
        else
        {
            accel = power;
        }
    }
}
