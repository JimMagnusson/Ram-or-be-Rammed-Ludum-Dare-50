using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotation : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 5f;
    // Update is called once per frame
    [SerializeField] Vector3 axis = Vector3.right;
    void Update()
    {
        transform.Rotate(axis, Time.deltaTime * rotationSpeed, Space.Self);
    }
}
