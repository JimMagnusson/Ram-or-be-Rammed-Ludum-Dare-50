using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    UIManager uIManager;
    private void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Got hit by: " + collision.gameObject.name);
        uIManager.ShowRetryImage();
    }
}
