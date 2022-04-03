using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    LevelLoader levelLoader;
    private void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Player out of bounds");
        if (levelLoader != null)
        {
            levelLoader.ReloadScene();
        }
    }
}
