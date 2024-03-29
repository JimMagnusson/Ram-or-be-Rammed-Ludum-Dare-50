﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private AudioClip destroySound = null;
    
    [SerializeField] private Transform[] bodies;

    [SerializeField] private int partsRequiredToDestroy = 5;

    [SerializeField] private ParticleSystem isDestroyedParticles;

    private AudioSource audioSource;
    private bool collidedWith = false;
    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public int GetPartsRequiredToDestroy()
    {
        return partsRequiredToDestroy;
    }

    public void DoDestructionEffects()
    {
        if (collidedWith) { return; }
        collidedWith = true;

        if (audioSource != null)
        {
            audioSource.PlayOneShot(destroySound);
        }


        if(GetComponent<MeshRenderer>() == null)
        {
            foreach (Transform body in bodies)
            {
                body.gameObject.SetActive(false);
            }
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
        }

        if(isDestroyedParticles != null)
        {
            Debug.Log("Play: " + isDestroyedParticles);
            isDestroyedParticles.Play();
        }

        Destroy(gameObject, 1f);
    }

}
