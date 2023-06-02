using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    private bool isGrounded = true;
    private float accel;

    [Header("Car Setup")]
    public float power;
    public float steerSpeed = 1f;
    public float handBrake = 1.3f;
    public float weight = 800;
    public float topSpeed = 50;

    [Header("Car Parts")]
    public GameObject car;
    public Transform wheelFL;
    public Transform wheelFR;
    public Transform wheelRL;
    public Transform wheelRR;

    [Header("Target Force")]
    public Transform targetForward;

    [Header("Raycast")]
    public Transform raycastTarget;
    public LayerMask ground;
    public float raycastDistance = 0.3f;

    [Header("VFX")]
    public GameObject smoke;
    public GameObject trail;


    [Header("SFX")]
    public AudioSource engine;
    public GameObject tireScreech;

    Vector3 com;


    void Start()
    {
        car.GetComponent<Rigidbody>().mass = weight;
        car.GetComponent<Rigidbody>().centerOfMass = com;
    }
   

    // FixedUpdate stores forces applied to the Rigidbody.
    void FixedUpdate()
    {
        RaycastHit groundHit;
        if (Physics.Raycast(raycastTarget.position, -raycastTarget.up, out groundHit, raycastDistance, ground))
        {
            isGrounded = true;
            Debug.DrawRay(raycastTarget.position, -raycastTarget.up);
            Debug.Log("Car is grounded");
        }
        else
        {
            isGrounded = false;
            Debug.DrawRay(raycastTarget.position, -raycastTarget.up);
            Debug.Log("Car is NOT grounded");
        }

        if (isGrounded)
        {
            if (Keyboard.current.upArrowKey.isPressed)
            {
                car.GetComponent<Rigidbody>().AddForce(targetForward.forward * accel * 1000 * Time.deltaTime);

                if (Keyboard.current.spaceKey.isPressed)
                {
                    car.GetComponent<Rigidbody>().AddForce(-targetForward.forward * accel * 1000 * Time.deltaTime);
                }
            }

            if (Keyboard.current.downArrowKey.isPressed)
            {
                car.GetComponent<Rigidbody>().AddForce(-targetForward.forward * accel * 1000 * Time.deltaTime);
            }

            if (car.GetComponent<Rigidbody>().velocity.magnitude >= 1)
            {
                car.transform.Rotate(0, Input.GetAxis("Horizontal") * steerSpeed * Time.deltaTime, 0, Space.Self);

                if (Keyboard.current.spaceKey.isPressed)
                {
                    car.transform.Rotate(0, Input.GetAxis("Horizontal") * handBrake * Time.deltaTime, 0, Space.Self);
                    smoke.SetActive(true);
                    trail.SetActive(true);
                    tireScreech.SetActive(true);
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
        else
        {
            smoke.SetActive(false);
            trail.SetActive(false);
            tireScreech.SetActive(false);
        }

        wheelFL.transform.Rotate(Input.GetAxis("Vertical") * car.GetComponent<Rigidbody>().velocity.magnitude * 1000 * Time.deltaTime, 0, 0);
        wheelFR.transform.Rotate(Input.GetAxis("Vertical") * car.GetComponent<Rigidbody>().velocity.magnitude * 1000 * Time.deltaTime, 0, 0);
        wheelRL.transform.Rotate(Input.GetAxis("Vertical") * car.GetComponent<Rigidbody>().velocity.magnitude * 1000 * Time.deltaTime, 0, 0);
        wheelRR.transform.Rotate(Input.GetAxis("Vertical") * car.GetComponent<Rigidbody>().velocity.magnitude * 1000 * Time.deltaTime, 0, 0);

        engine.pitch = Mathf.Clamp(car.GetComponent<Rigidbody>().velocity.sqrMagnitude, 400, 3000) * Time.deltaTime / 30;

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
