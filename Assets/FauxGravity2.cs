using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravity2 : MonoBehaviour
{
    public LayerMask whatIsGround;
    public float groundRayLength = 10f;
    public Transform groundRayPoint;

    public Rigidbody rb;

    public float gravityConstant = 9.8f;

    private void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            Vector3 normal = hit.normal;
            rb.AddForce(-normal * gravityConstant, ForceMode.Acceleration);
        }
    }

}
