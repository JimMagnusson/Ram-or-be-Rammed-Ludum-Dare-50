using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSphere : MonoBehaviour
{
    private Vector3 contactNormal;

    public Vector3 GetContactNormal()
    {
        return contactNormal;
    }
    private void OnCollisionStay(Collision collision)
    {
        Vector3 normalSum = new Vector3(0, 0, 0);
        foreach (ContactPoint contact in collision.contacts)
        {
            normalSum += contact.normal;
            //print(contact.thisCollider.name + " hit " + contact.otherCollider.name);
            // Visualize the contact point
            //Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        contactNormal = normalSum / collision.contacts.Length;
    }
}
