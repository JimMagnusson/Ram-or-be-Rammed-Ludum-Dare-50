using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyCarController : MonoBehaviour
{
    public Rigidbody rb;

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

    private CarController3 player;

    // Start is called before the first frame update
    void Start()
    {
        rb.transform.parent = null;
        player = FindObjectOfType<CarController3>();
    }

    // Update is called once per frame
    void Update()
    {
        speedInput = forwardNormalAccel * 1000f;
        //speedInput = 0;
        //turnInput = Input.GetAxis("Horizontal");

        // Need to know if enemy should turn or not. Linear algebra!
        // Project the vector between enemy and player to x-z plane of the enemy.
        Vector3 normal = transform.up;
        Vector3 toPlayer = player.transform.position - transform.position;

        Vector3 toPlayerProjectedInPlane = toPlayer - OrthogonalProjection(toPlayer, normal);

        // Then calculate angle, returns between -180 and 180.
        float angle = Vector3.SignedAngle(transform.forward, toPlayerProjectedInPlane, normal);

        float clampedAngle = Mathf.Clamp(angle, -angleForMaxTurning, angleForMaxTurning);

        float turnInput = clampedAngle / angleForMaxTurning;

        // Rotation
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround) && grounded)
        {
            transform.RotateAround(transform.position, hit.normal, turnInput * turnStrength * Time.deltaTime);
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

        }
        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) + 90, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn - 90, leftFrontWheel.localRotation.eulerAngles.z);

        if (grounded)
        {
            rb.drag = dragOnGround;

            if (Mathf.Abs(speedInput) > 0 && rb.velocity.magnitude < maxChargeSpeed)
            {
                rb.AddForce(transform.forward * speedInput);
            }
        }
        else
        {
            rb.drag = 0.1f;
        }
    }

    // Projects u onto v.
    private Vector3 OrthogonalProjection(Vector3 u, Vector3 v)
    {
        return v * (Vector3.Dot(u, v) / Mathf.Pow(v.magnitude, 2));
    }
}