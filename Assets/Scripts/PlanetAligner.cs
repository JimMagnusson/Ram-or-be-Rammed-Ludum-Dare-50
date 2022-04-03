using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAligner : MonoBehaviour
{
    public LayerMask whatIsGround;
    public float groundRayLength = 10f;
    public Transform groundRayPoint;
    public bool alignToPlanet = true;

    private void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            if (alignToPlanet)
            {
                Quaternion q = Quaternion.FromToRotation(transform.up, hit.normal);
                q = q * transform.rotation;
                transform.rotation = Quaternion.Slerp(transform.rotation, q, 1);
            }
        }
    }
}
