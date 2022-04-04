using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartCollector : MonoBehaviour
{
    [SerializeField] private bool togglesCarOnFirstPickup = false;
    [SerializeField] private Transform under;
    [SerializeField] private Transform wheelCovers;
    [SerializeField] private Transform front;
    [SerializeField] private Transform side;
    [SerializeField] private Transform roof;

    [SerializeField] private AudioClip collectSound1 = null;
    [SerializeField] private AudioClip collectSound2 = null;
    [SerializeField] private AudioClip collectSound3 = null;
    [SerializeField] private AudioClip collectSound4 = null;
    [SerializeField] private AudioClip collectSound5 = null;

    private AudioSource audioSource;
    private Grower grower;
    private int partsCollected = 0;
    private EnemyCarController enemyCarController;


    private void Start()
    {
        grower = GetComponent<Grower>();
        enemyCarController = FindObjectOfType<EnemyCarController>();
        audioSource = GetComponent<AudioSource>();
    }

    public int GetPartsCollected()
    {
        return partsCollected;
    }

    public void CollectPart()
    {
        partsCollected++;
        switch (partsCollected)
        {
            case 1:
                if (togglesCarOnFirstPickup)
                {
                    enemyCarController.TurnOnCar();
                }
                under.gameObject.SetActive(true);

                if (audioSource != null)
                {
                    audioSource.PlayOneShot(collectSound1);
                }

                break;
            case 2:
                wheelCovers.gameObject.SetActive(true);

                if (audioSource != null)
                {
                    audioSource.PlayOneShot(collectSound2);
                }
                break;
            case 3:
                side.gameObject.SetActive(true);
                if (audioSource != null)
                {
                    audioSource.PlayOneShot(collectSound3);
                }
                break;
            case 4:
                roof.gameObject.SetActive(true);
                if (audioSource != null)
                {
                    audioSource.PlayOneShot(collectSound4);
                }
                break;
            case 5:
                front.gameObject.SetActive(true);
                if (audioSource != null)
                {
                    audioSource.PlayOneShot(collectSound5);
                }
                break;
            default:
                Debug.LogError("More than 5 parts collected.");
                break;
        }
        if (grower != null)
        {
            grower.Grow();
        }
    }
}
