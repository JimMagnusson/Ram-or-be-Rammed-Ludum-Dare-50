using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private AudioClip isDestroyedSound = null;

    [SerializeField] private ParticleSystem isDestroyedParticles;
    [SerializeField] private ParticleSystem isDestroyedParticles2;

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
        destroyed = true;
        enemyCarController.TurnOffCar();

        // SFX
        if (audioSource != null && isDestroyedSound != null)
        {
            audioSource.PlayOneShot(isDestroyedSound);
        }

        //VFX
        isDestroyedParticles.Play();
        isDestroyedParticles2.Play();

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
            Destructible destructible = GetComponent<Destructible>();
            if (destructible == null)
            {
                destructible = other.transform.GetComponentInParent<Destructible>();
            }
            destructible.DoDestructionEffects();
        }
    }
}
