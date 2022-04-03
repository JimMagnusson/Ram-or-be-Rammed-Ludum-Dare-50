using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private AudioClip isDestroyedSound = null;

    [SerializeField] private GameObject isDestroyedParticles;

    private AudioSource audioSource;
    private EnemyCarController enemyCarController;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        enemyCarController = GetComponent<EnemyCarController>();
    }

    public void DoGetDestroyedEffects()
    {
        enemyCarController.ToggleCar(false);

        // SFX
        if (audioSource != null && isDestroyedSound != null)
        {
            audioSource.PlayOneShot(isDestroyedSound);
        }

        // TODO: VFX
        if (isDestroyedParticles != null)
        {
            isDestroyedParticles = Instantiate(isDestroyedParticles, transform.position, Quaternion.identity, null);
        }
    }

    public void Stop()
    {
        enemyCarController.ToggleCar(false);
    }
}
