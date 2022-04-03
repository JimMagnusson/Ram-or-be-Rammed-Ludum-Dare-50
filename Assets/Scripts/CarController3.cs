using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CarController3 : MonoBehaviour
{
    public Rigidbody rb;

    //dragOnGround = 3f;
    private bool grounded;
    private Vector3 normal;

    public LayerMask whatIsGround;
    public float groundRayLength = 0.5f;
    public Transform groundRayPoint;

    public LayerMask whatIsObstacle;
    public float obstacleCheckRayLength = 0.2f;
    public float obstacleCheckRayRadius = 0.2f;
    public Transform frontRayPoint;
    public Transform leftRayPoint;
    public Transform rightRayPoint;


    public Transform leftFrontWheel, rightFrontWheel;
    public float maxWheelTurn = 25;

    //public Rigidbody thisRb;

    private float turnInput;
    private float speedInput;


    public float moveSpeed = 10f;
    public float rotationSpeed = 10f;

    private PartCollector partCollector;
    public int partsRequiredForObjects = 5;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        partCollector = GetComponent<PartCollector>();
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


        // Check for collisions with obstacles:
        bool hitObstacleInFront = false;
        bool hitObstacleLeft = false;
        bool hitObstacleRight = false;
        RaycastHit hit;

        // IN front:
        //if (Physics.Raycast(frontRayPoint.position, -transform.right, out hit, obstacleCheckRayLength, whatIsObstacle))
        if(Physics.SphereCast(frontRayPoint.position, obstacleCheckRayRadius, -transform.right, out hit, obstacleCheckRayLength, whatIsObstacle))
        {
            if(partCollector.GetPartsCollected() < partsRequiredForObjects)
            {
                hitObstacleInFront = true;
            }
        }

        // Left

        if (Physics.SphereCast(leftRayPoint.position, obstacleCheckRayRadius, -transform.forward, out hit, obstacleCheckRayLength, whatIsObstacle))
        {
            if (partCollector.GetPartsCollected() < partsRequiredForObjects)
            {
                hitObstacleLeft = true;
            }
        }

        // Right:

        if (Physics.SphereCast(rightRayPoint.position, obstacleCheckRayRadius, transform.forward, out hit, obstacleCheckRayLength, whatIsObstacle))
        {
            if (partCollector.GetPartsCollected() < partsRequiredForObjects)
            {
                hitObstacleRight = true;
            }
        }

        if ((speedInput > 0 && !hitObstacleInFront) || speedInput < 0)
        {
            // Move forward
            rb.MovePosition(rb.position + -transform.right * speedInput * moveSpeed * Time.fixedDeltaTime);
        }
        
        Vector3 yRotation = Vector3.up * turnInput * rotationSpeed * Time.fixedDeltaTime;
        Quaternion deltaRotation = Quaternion.Euler(yRotation);
        Quaternion targetRotation = rb.rotation * deltaRotation;

        // Turn left
        if((turnInput < 0 && !hitObstacleLeft) || (turnInput > 0 && !hitObstacleRight))
        {
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 50f * Time.deltaTime));
        }
    }
}
