using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{

    float raycastDistance = 1;
    private bool isGrounded = true;
    private float fallSpeed = 100;

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

    void Start()
    {
        car.GetComponent<Rigidbody>().mass = weight;
    }

    void Update()
    {
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
                    car.transform.Rotate(0, Input.GetAxis("Horizontal") * handBrake * Time.deltaTime, 0, Space.Self);

                wheelRL.localEulerAngles = new Vector3(0, 0, 90);
                wheelRR.localEulerAngles = new Vector3(0, 0, 90);

            }
        }

        if (Keyboard.current != null && Keyboard.current.downArrowKey.IsActuated(1))
        {
            wheelFL.localEulerAngles = new Vector3(0, 0, 90) + Vector3.up * Mathf.Clamp(Input.GetAxis("Horizontal") * 100, -45, 45);
            wheelFR.localEulerAngles = new Vector3(0, 0, 90) + Vector3.up * Mathf.Clamp(Input.GetAxis("Horizontal") * 100, -45, 45);

            wheelFL.transform.Rotate(0, -Input.GetAxis("Vertical") * -accel * 1000 * Time.deltaTime, 0);
            wheelFR.transform.Rotate(0, -Input.GetAxis("Vertical") * -accel * 1000 * Time.deltaTime, 0);
            wheelRL.transform.Rotate(0, -Input.GetAxis("Vertical") * -accel * 1000 * Time.deltaTime, 0);
            wheelRR.transform.Rotate(0, -Input.GetAxis("Vertical") * -accel * 1000 * Time.deltaTime, 0);

            if (isGrounded)
                car.transform.Rotate(0, -Input.GetAxis("Horizontal") * steerSpeed * Time.deltaTime, 0, Space.Self);

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
                car.GetComponent<Rigidbody>().AddForce(-targetForward.forward * accel * 1000 * Time.deltaTime);
            }

        }
    }
}
