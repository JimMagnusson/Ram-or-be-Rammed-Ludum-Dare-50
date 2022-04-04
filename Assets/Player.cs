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
    private CarController4 carController4;

    private UIManager uIManager;


    private void Start()
    {
        partCollector = GetComponent<PartCollector>();
        carController4 = GetComponent<CarController4>();
        uIManager = FindObjectOfType<UIManager>();
    }
    private void DoDestroyedEffects()
    {
        carController4.ToggleCar(false);

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (partCollector.GetPartsCollected() >= partsRequiredToWin)
            {
                other.GetComponent<Enemy>().DoGetDestroyedEffects();

                uIManager.ShowWinLevelScreen();
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
            Destructible destructible = other.transform.GetComponentInParent<Destructible>();
            if (partCollector.GetPartsCollected() >= destructible.GetPartsRequiredToDestroy())
            {
                destructible.DoDestructionEffects();
            }
        }
    }

}
