using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target; //Empty transform placed as child on player at offset

    Vector3 velocity = Vector3.zero;
    [SerializeField] float smoothTime = 0.4f;
    [SerializeField] float rotTime = 0.4f;

    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime*Time.fixedDeltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotTime * Time.fixedDeltaTime);
    }
}
