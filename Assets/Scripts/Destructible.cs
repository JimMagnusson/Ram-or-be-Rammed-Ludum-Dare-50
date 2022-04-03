using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    [SerializeField] private AudioClip destroySound = null;
    
    [SerializeField] private Transform[] bodies;

    [SerializeField] private int partsRequiredToDestroy = 5;

    private AudioSource audioSource;
    private bool collidedWith = false;
    // Start is called before the first frame update
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // VFX and SFX when collided with.
    public void CollideWith(Collider other)
    {
        if(collidedWith) { return; }
        collidedWith = true;
        
        bool collidedWithBigPlayer = other.gameObject.CompareTag("Player") && other.GetComponent<PartCollector>().GetPartsCollected() >= partsRequiredToDestroy;
        if (collidedWithBigPlayer || other.gameObject.CompareTag("Enemy"))
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(destroySound);
            }

            foreach (Transform body in bodies)
            {
                body.gameObject.SetActive(false);
            }

            // TODO: Trigger particles

            Destroy(gameObject, 1f);
        }
    }

}
