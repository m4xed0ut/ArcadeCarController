using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{

    float raycastDistance = 0.3f;
    private bool isGrounded = true;
    private float fallSpeed = 100;
    float gearRatio = 30;
    float maxRpm = 3000;
    float idle = 400;
    
    [Header("Car Setup")]
    public float accel = 1000;
    public float steerSpeed = 1f;
    public float handBrake = 1.3f;
    public float weight = 800;

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

    [Header("VFX")]
    public GameObject smoke;
    public GameObject trail;
    public GameObject brakeLights;


    [Header("SFX")]
    public AudioSource engine;
    public GameObject tireScreech;


    void Start()
    {
        car.GetComponent<Rigidbody>().mass = weight;
    }

    // Update stores everything that modifies the car's transforms.
    void Update()
    {
        Lights();
        if (Keyboard.current != null && Keyboard.current.upArrowKey.IsActuated(1))
        {
            if (isGrounded)
                car.transform.Rotate(0, Input.GetAxis("Horizontal") * steerSpeed * Time.deltaTime, 0, Space.Self);

            wheelFL.localEulerAngles = new Vector3(0, 0, 90) + Vector3.up * Mathf.Clamp(Input.GetAxis("Horizontal") * 100, -45, 45);
            wheelFR.localEulerAngles = new Vector3(0, 0, 90) + Vector3.up * Mathf.Clamp(Input.GetAxis("Horizontal") * 100, -45, 45);

            wheelFL.transform.Rotate(0, Input.GetAxis("Vertical") * accel * 1000 * Time.deltaTime, 0);
            wheelFR.transform.Rotate(0, Input.GetAxis("Vertical") * accel * 1000 * Time.deltaTime, 0);
            wheelRL.transform.Rotate(0, Input.GetAxis("Vertical") * accel * 1000 * Time.deltaTime, 0);
            wheelRR.transform.Rotate(0, Input.GetAxis("Vertical") * accel * 1000 * Time.deltaTime, 0);

            if (Keyboard.current != null && Keyboard.current.spaceKey.IsActuated(1))
            {
                wheelFL.localEulerAngles = new Vector3(0, 0, 90) + Vector3.up * -Mathf.Clamp(Input.GetAxis("Horizontal") * 100, -45, 45);
                wheelFR.localEulerAngles = new Vector3(0, 0, 90) + Vector3.up * -Mathf.Clamp(Input.GetAxis("Horizontal") * 100, -45, 45);

                if (isGrounded)
                {
                    car.transform.Rotate(0, Input.GetAxis("Horizontal") * handBrake * Time.deltaTime, 0, Space.Self);
                    smoke.SetActive(true);
                    trail.SetActive(true);
                    tireScreech.SetActive(true);
                }

                wheelRL.localEulerAngles = new Vector3(0, 0, 90);
                wheelRR.localEulerAngles = new Vector3(0, 0, 90);

            }
            else
            {
                smoke.SetActive(false);
                trail.SetActive(false);
                tireScreech.SetActive(false);
            }

        }
        else
        {
            smoke.SetActive(false);
            trail.SetActive(false);
            tireScreech.SetActive(false);
        }
    }

    void Lights()
    {

        if (Keyboard.current != null && Keyboard.current.downArrowKey.IsPressed(1))
        {
            brakeLights.SetActive(true);
        }
        else
        {
            brakeLights.SetActive(false);
        }

    }

    // Engine sound with a fake automatic gearbox simulation. 
    void Sound()
    {
        if (Keyboard.current != null && Keyboard.current.upArrowKey.IsActuated(1))
        {
            if (engine.pitch == 2.0)
            {
                Debug.Log("Shifting up");
                gearRatio += 15;
                maxRpm += 1500;
                idle += 150;
                accel -= 135;
            }
        }
        else
        {
            if (car.GetComponent<Rigidbody>().velocity.sqrMagnitude <= 3000)
            {
                Debug.Log("Shifting down");
                gearRatio = 30;
                maxRpm = 3000;
                idle = 400;
                accel = 1300;
            }
        }

    }

    // FixedUpdate stores forces applied to the Rigidbody.
    void FixedUpdate()
    {
        Sound();
        engine.pitch = Mathf.Clamp(car.GetComponent<Rigidbody>().velocity.sqrMagnitude, idle, maxRpm) * Time.deltaTime / gearRatio;
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

        car.GetComponent<Rigidbody>().AddForce(Vector3.down * fallSpeed * 1000 * Time.deltaTime);

        if (isGrounded)
        {
            if (Keyboard.current != null && Keyboard.current.upArrowKey.IsActuated(1))
            {
                car.GetComponent<Rigidbody>().AddForce(targetForward.forward * accel * 1000 * Time.deltaTime);

                if (Keyboard.current != null && Keyboard.current.spaceKey.IsActuated(1))
                {
                    car.GetComponent<Rigidbody>().AddForce(-targetForward.forward * accel * 1000 * Time.deltaTime);
                }

                if (Keyboard.current != null && Keyboard.current.rightArrowKey.IsActuated(1))
                {
                    car.GetComponent<Rigidbody>().AddForce(targetLeft.right * accel * 1000 * Time.deltaTime);
                }

                if (Keyboard.current != null && Keyboard.current.leftArrowKey.IsActuated(1))
                {
                    car.GetComponent<Rigidbody>().AddForce(-targetLeft.right * accel * 1000 * Time.deltaTime);
                }
            }

            if (Keyboard.current != null && Keyboard.current.downArrowKey.IsActuated(1))
            {

                car.GetComponent<Rigidbody>().drag = 1;
            }
            else
            {
                car.GetComponent<Rigidbody>().drag = 0;
            }
        }
    }
}
