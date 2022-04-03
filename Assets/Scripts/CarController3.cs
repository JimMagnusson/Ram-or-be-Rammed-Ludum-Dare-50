using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarController3 : MonoBehaviour
{
    public Rigidbody rb;

    public float forwardAccel = 8f, reverseAccel = 4f, maxSpeed = 50f, turnStrength = 180f, gravityForce = 10f, dragOnGround = 3f;

    //private float speedInput, turnInput;

    private bool grounded;
    private Vector3 normal;

    public LayerMask whatIsGround;
    public float groundRayLength = 0.5f;
    public Transform groundRayPoint;

    public Transform leftFrontWheel, rightFrontWheel;
    public float maxWheelTurn = 25;

    private float timeCount = 0.0f;

    //public Rigidbody thisRb;

    private float turnInput;
    private float speedInput;


    public float moveSpeed = 10f;
    public float rotationSpeed = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        speedInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxisRaw("Horizontal");
    }


    private void FixedUpdate()
    {
        /*
        grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;
            normal = hit.normal;
            //.RotateAround(transform.position, hit.normal, turnInput * turnStrength * Input.GetAxis("Vertical") * Time.deltaTime);
            Quaternion finalRotation = transform.rotation * Quaternion.AngleAxis(turnInput * turnStrength * Time.deltaTime, normal);
            
            transform.rotation = finalRotation;
            
            
            //transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, Time.deltaTime);

        }
        */

        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180 - 90, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn + 90, leftFrontWheel.localRotation.eulerAngles.z);

        if(speedInput != 0)
        {
            rb.MovePosition(rb.position + -transform.right * speedInput * moveSpeed * Time.fixedDeltaTime);
        }
        
        Vector3 yRotation = Vector3.up * turnInput * rotationSpeed * Time.fixedDeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(yRotation);
        Quaternion targetRotation = rb.rotation * deltaRotation;
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 50f * Time.deltaTime));


        /*
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

            rb.AddForce(normal * -gravityForce * 100);
        }
        */

        //rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
        //thisRb.MovePosition(rb.position);
        //transform.position = rb.transform.position;
    }
}
