﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private int partsRequiredToWin = 5;

    [SerializeField] private AudioClip isDestroyedSound = null;

    [SerializeField] private ParticleSystem isDestroyedParticles;

    private AudioSource audioSource;
    private PartCollector partCollector;
    private CarController4 carController4;

    private UIManager uIManager;

    private bool destroyed = false;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        partCollector = GetComponent<PartCollector>();
        carController4 = GetComponent<CarController4>();
        uIManager = FindObjectOfType<UIManager>();
    }
    private void DoDestroyedEffects()
    {
        if (destroyed) { return; }
        destroyed = true;
        carController4.ToggleCar(false);
        // SFX
        if (audioSource != null && isDestroyedSound != null)
        {
            audioSource.PlayOneShot(isDestroyedSound);
        }
        isDestroyedParticles.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (partCollector.GetPartsCollected() >= partsRequiredToWin)
            {
                other.GetComponent<Enemy>().DoGetDestroyedEffects();

                if(SceneManager.GetActiveScene().buildIndex == 4)
                {
                    uIManager.ShowWonGameImage();
                }
                else
                {
                    uIManager.ShowWinLevelScreen();
                }
            }
            else
            {
                DoDestroyedEffects();
                other.GetComponent<Enemy>().GoStraight();
                uIManager.ShowRetryImage();
            }
        }
        else if (other.CompareTag("Part"))
        {
            partCollector.CollectPart();
            other.GetComponent<Part>().DoCollectEffects();
        }
        else if (other.transform.CompareTag("Destructible"))
        {
            Destructible destructible = GetComponent<Destructible>();
            if(destructible == null)
            {
                destructible = other.transform.GetComponentInParent<Destructible>();
            }

            if (partCollector.GetPartsCollected() >= destructible.GetPartsRequiredToDestroy())
            {
                destructible.DoDestructionEffects();
            }
        }
    }

}
