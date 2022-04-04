using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    turnedOff,
    recharging,
    preCharging,
    charging,
    normal
}

public class EnemyCarController : MonoBehaviour
{
    public Rigidbody rb;
    public Transform garage;

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

    private Player player;
    [SerializeField] private bool turnedOnAtStart = false;

    public float rechargeTime = 4f;
    private float rechargeTimer = 0f;

    public float preChargeTime = 2f;
    private float preChargeTimer = 0f;

    public float chargeCooldownMinTime = 3f;
    public float chargeCooldownMaxTime = 6f;

    private float chargeCooldownTime = 0f;
    private float chargeCooldownTimer = 0f;

    public float chargeTime = 3f;
    private float chargeTimer = 0f;

    public bool chargeEnabled = true;

    private bool goingStraight = false;

    public EnemyState state = EnemyState.normal;

    private FauxGravity2 fauxGravity2;

    public ParticleSystem trailParticlesLeft;
    public ParticleSystem trailParticlesRight;

    public void TurnOffCar()
    {
        state = EnemyState.turnedOff;
        trailParticlesLeft.Stop();
        trailParticlesRight.Stop();
    }
    public void TurnOnCar()
    {
        state = EnemyState.normal;
        if(garage != null && garage.gameObject.activeSelf)
        {
            garage.gameObject.SetActive(false);
        }
        trailParticlesLeft.Play();
        trailParticlesRight.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        fauxGravity2 = GetComponent<FauxGravity2>();
        rb.gameObject.SetActive(turnedOnAtStart);
        if(turnedOnAtStart)
        {
            state = EnemyState.normal;
        }
        else { state = EnemyState.turnedOff; }

        rb.transform.parent = null;
        player = FindObjectOfType<Player>();

        // Randomize charge cooldown time
        chargeCooldownTime = Random.Range(chargeCooldownMinTime, chargeCooldownMaxTime);
        Debug.Log("chargeCooldownTime: " + chargeCooldownTime);
        trailParticlesLeft.Stop();
        trailParticlesRight.Stop();

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(!turnedOn)
        {
            speedInput = 0;
            turnInput = 0;
            return;
        }
        */
        // Need to know if enemy should turn or not. Linear algebra!
        // Project the vector between enemy and player to x-z plane of the enemy.
        Vector3 normal = transform.up;
        Vector3 toPlayer = player.transform.position - transform.position;

        Vector3 toPlayerProjectedInPlane = toPlayer - OrthogonalProjection(toPlayer, normal);

        // Then calculate angle, returns between -180 and 180.
        float angle = Vector3.SignedAngle(transform.forward, toPlayerProjectedInPlane, normal);

        float clampedAngle = Mathf.Clamp(angle, -angleForMaxTurning, angleForMaxTurning);

        

        switch (state)
        {
            case EnemyState.turnedOff:
                speedInput = 0;
                turnInput = 0;
                rb.gameObject.SetActive(false);
                return;

            case EnemyState.normal:

                speedInput = forwardNormalAccel * 1000f;
                turnInput = clampedAngle / angleForMaxTurning;
                rb.gameObject.SetActive(true);


                // Countdown time until preCharge
                chargeCooldownTimer += Time.deltaTime;
                if(chargeCooldownTimer >= chargeCooldownTime)
                {
                    state = EnemyState.preCharging;
                    // Randomize charge cooldown time
                    chargeCooldownTime = Random.Range(chargeCooldownMinTime, chargeCooldownMaxTime);
                    Debug.Log("chargeCooldownTime: " + chargeCooldownTime);
                    chargeCooldownTimer = 0;
                    trailParticlesLeft.Play();
                    trailParticlesRight.Play();
                }

                break;
            case EnemyState.charging:
                speedInput = forwardChargeAccel * 1000f;
                turnInput = 0;
                rb.gameObject.SetActive(true);

                fauxGravity2.ToggleHighGravity(true);

                // Countdown time until Recharging
                chargeTimer += Time.deltaTime;
                if (chargeTimer >= chargeTime)
                {
                    state = EnemyState.recharging;
                    chargeTimer = 0;
                    fauxGravity2.ToggleHighGravity(false);
                    rb.velocity = Vector3.zero;
                    trailParticlesLeft.Stop();
                    trailParticlesRight.Stop();
                }
                break;
            case EnemyState.recharging:
                speedInput = 0;
                turnInput = 0;
                rb.velocity = Vector3.zero;
                rb.gameObject.SetActive(false);

                // Countdown time until normal
                rechargeTimer += Time.deltaTime;
                if (rechargeTimer >= rechargeTime)
                {
                    state = EnemyState.normal;
                    rechargeTimer = 0;
                    trailParticlesLeft.Play();
                    trailParticlesRight.Play();
                }
                return;
            case EnemyState.preCharging:
                turnInput = clampedAngle / angleForMaxTurning;
                speedInput = 0;
                rb.gameObject.SetActive(false);

                // Countdown time until charge
                preChargeTimer += Time.deltaTime;
                if (preChargeTimer >= preChargeTime)
                {
                    state = EnemyState.charging;
                    preChargeTimer = 0;
                    trailParticlesLeft.Play();
                    trailParticlesRight.Play();
                }
                break;
        }

        // Special case:
        if(goingStraight) { turnInput = 0; }

        // Rotation
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround) && grounded)
        {
            transform.RotateAround(transform.position, hit.normal, turnInput * turnStrength * Time.deltaTime);
        }

        transform.position = rb.transform.position;
    }

    public void GoStraight()
    {
        goingStraight = true;
    }


    private void FixedUpdate()
    {
        grounded = false;
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;

        }

        // Rotate along planet
        Quaternion newRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = newRotation;

        leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) + 90, leftFrontWheel.localRotation.eulerAngles.z);
        rightFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn - 90, leftFrontWheel.localRotation.eulerAngles.z);

        if (grounded)
        {
            rb.drag = dragOnGround;

            float maxSpeed = 0;
            switch (state)
            {
                case EnemyState.normal:
                    maxSpeed = maxNormalSpeed;
                    break;
                case EnemyState.charging:
                    maxSpeed = maxChargeSpeed;
                    break;
                case EnemyState.recharging:
                    return;
                default:
                    break;
            }
            if (Mathf.Abs(speedInput) > 0 && rb.velocity.magnitude < maxSpeed)
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