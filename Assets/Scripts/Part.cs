using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound = null;
    [SerializeField] private Transform[] bodies;
    [SerializeField] private ParticleSystem particles;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }



    // VFX and SFX when collected.
    public void DoCollectEffects()
    {

        //VFX
        particles.Play();
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
