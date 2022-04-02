using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravity : MonoBehaviour
{
    public LayerMask whatIsGround;
    public float groundRayLength = 10f;
    public Transform groundRayPoint;
    public Rigidbody rb;
    public bool alignToPlanet = true;
    public float gravityConstant = 9.8f;

    private void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            //transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            rb.AddForce(-hit.normal * gravityConstant, ForceMode.Acceleration);

            if (alignToPlanet)
            {
                Quaternion q = Quaternion.FromToRotation(transform.up, hit.normal);
                q = q * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, q, 1);
            }
        }
    }
}
