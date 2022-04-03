using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int partsRequiredToWin = 5;

    [SerializeField] private AudioClip isDestroyedSound = null;

    [SerializeField] private GameObject isDestroyedParticles;

    private AudioSource audioSource;
    private PartCollector partCollector;
    private CarController3 carController3;

    private void Start()
    {
        partCollector = GetComponent<PartCollector>();
        carController3 = GetComponent<CarController3>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if(partCollector.GetPartsCollected() >= partsRequiredToWin)
            {
                other.GetComponent<Enemy>().DoGetDestroyedEffects();

                // TODO: Win level!
            }
            else
            {
                DoDestroyedEffects();
                other.GetComponent<Enemy>().Stop();
            }
        }
    }

    private void DoDestroyedEffects()
    {
        carController3.StopCar();

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
}
