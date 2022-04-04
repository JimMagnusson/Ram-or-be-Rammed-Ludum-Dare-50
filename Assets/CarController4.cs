using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarController4 : MonoBehaviour
{
    public Rigidbody rb;
    public Transform slerpers;
    public Transform bodies;

    public float forwardNormalAccel = 2f, maxNormalSpeed = 10f;
    public float forwardChargeAccel = 4f, maxChargeSpeed = 30f;
    public float turnStrength = 180f, dragOnGround = 3f;

    private float speedInput, turnInput;

    private bool grounded;

    public LayerMask whatIsGround;
    public float groundRayLength = 0.5f;
    public Transform groundRayPoint;

    public Transform leftFrontWheel, rightFrontWheel;
    public float maxWheelTurn = 25;


    public float angleForMaxTurning = 90;

    [SerializeField] private bool turnedOn = false;

    public float rotationSmoothFactor = 5;
    public float rotationSmoothFactorBodies = 5;

    public MovementSphere movementSphere;

    public void ToggleCar(bool boolean)
    {
        rb.gameObject.SetActive(boolean);
        turnedOn = boolean;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb.gameObject.SetActive(turnedOn);
        rb.transform.parent = null;

        slerpers.parent = null;
        bodies.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        speedInput = Input.GetAxis("Vertical") * forwardNormalAccel * 1000f;

        turnInput = Input.GetAxis("Horizontal");

    }

    private void FixedUpdate()
    {
        grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;

        }

        Quaternion newRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        Quaternion slerpedRotation = Quaternion.Slerp(slerpers.rotation, newRotation, rotationSmoothFactor * Time.fixedDeltaTime);
        Quaternion slerpedRotationForBodies = Quaternion.Slerp(bodies.rotation, newRotation, rotationSmoothFactorBodies * Time.fixedDeltaTime);
        transform.rotation = newRotation;
        slerpers.rotation = slerpedRotation;
        bodies.rotation = slerpedRotationForBodies;


        if (grounded)
        {
            transform.RotateAround(transform.position, transform.up, turnInput * turnStrength * Time.fixedDeltaTime);
        }

        transform.position = rb.transform.position;
        slerpers.transform.position = rb.transform.position;
        bodies.transform.position = rb.transform.position;

        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) + 90, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn - 90, leftFrontWheel.localRotation.eulerAngles.z);

        if (grounded)
        {
            rb.drag = dragOnGround;

            if (Mathf.Abs(speedInput) > 0 && rb.velocity.magnitude < maxChargeSpeed)
            {

                rb.AddForce(-transform.right * speedInput);
            }
        }
        else
        {
            rb.drag = 0.1f;
        }
    }
}