using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound = null;
    [SerializeField] private Transform[] bodies;
    [SerializeField] private GameObject particles;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }



    // VFX and SFX when collected.
    public void DoCollectEffects()
    {
        if (particles != null)
        {
            particles = Instantiate(particles, transform.position, Quaternion.identity, null);
        }
        if (audioSource != null)
        {
            audioSource.PlayOneShot(collectSound);
        }
        gameObject.GetComponent<BoxCollider>().enabled = false;

        foreach (Transform body in bodies)
        {
            body.gameObject.SetActive(false);
        }
        Destroy(gameObject, 1f);
    }
}
