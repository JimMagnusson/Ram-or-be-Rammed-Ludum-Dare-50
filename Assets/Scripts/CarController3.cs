using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarController3 : MonoBehaviour
{
    public Rigidbody rb;

    public float forwardAccel = 8f, reverseAccel = 4f, maxSpeed = 50f, turnStrength = 180f, gravityForce = 10f, dragOnGround = 3f;

    private float speedInput, turnInput;

    private bool grounded;

    public LayerMask whatIsGround;
    public float groundRayLength = 0.5f;
    public Transform groundRayPoint;

    public Transform leftFrontWheel, rightFrontWheel;
    public float maxWheelTurn = 25;

    // Start is called before the first frame update
    void Start()
    {
        rb.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        speedInput = 0f;
        if (Input.GetAxis("Vertical") > 0)
        {
            speedInput = Input.GetAxis("Vertical") * forwardAccel * 1000f;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            speedInput = Input.GetAxis("Vertical") * reverseAccel * 1000f;
        }

        turnInput = Input.GetAxis("Horizontal");

        // Rotation
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround) && grounded)
        {
            //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(
            //hit.normal.x * turnInput * turnStrength * Time.deltaTime,
            //hit.normal.y * turnInput * turnStrength * Time.deltaTime,
            //0f));
            //}
            //transform.Rotate(hit.normal, turnInput * turnStrength * Time.deltaTime);
            transform.RotateAround(transform.position, hit.normal, turnInput * turnStrength * Time.deltaTime);
        }

        if (grounded)
        {
            
        }

        transform.position = rb.transform.position;
    }

    private void FixedUpdate()
    {
        grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;

            //transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

        }

        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, leftFrontWheel.localRotation.eulerAngles.z);


        if (grounded)
        {
            rb.drag = dragOnGround;

            if (Mathf.Abs(speedInput) > 0)
            {
                rb.AddForce(transform.forward * speedInput);
            }
        }
        else
        {
            rb.drag = 0.1f;

            rb.AddForce(Vector3.up * -gravityForce * 100);
        }
    }
}
