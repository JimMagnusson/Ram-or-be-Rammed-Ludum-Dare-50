using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound = null;
    [SerializeField] private Transform body;

    private float shortDelay = 0.1f;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // VFX and SFX when collected.
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(audioSource != null)
            {
                audioSource.PlayOneShot(collectSound);
            }
            gameObject.GetComponent<BoxCollider>().enabled = false;
            if (body != null)
            {
                body.gameObject.SetActive(false);
            }
            Destroy(gameObject, 1f);
        }
    }
}
