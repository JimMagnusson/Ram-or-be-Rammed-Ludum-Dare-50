using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private AudioClip isDestroyedSound = null;

    [SerializeField] private ParticleSystem isDestroyedParticles;

    private AudioSource audioSource;
    private EnemyCarController enemyCarController;
    private bool destroyed = false;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        enemyCarController = GetComponent<EnemyCarController>();
    }

    public void DoGetDestroyedEffects()
    {
        if(destroyed) { return; }
        enemyCarController.TurnOffCar();

        // SFX
        if (audioSource != null && isDestroyedSound != null)
        {
            audioSource.PlayOneShot(isDestroyedSound);
        }

        //VFX
        isDestroyedParticles.Play();

    }

    public void GoStraight()
    {
        enemyCarController.GoStraight();
    }

    public void Stop()
    {
        enemyCarController.TurnOffCar();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destructible"))
        {
            other.GetComponentInParent<Destructible>().DoDestructionEffects();
        }
    }
}
