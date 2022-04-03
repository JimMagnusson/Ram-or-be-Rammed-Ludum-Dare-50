using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyColliderTrigger : MonoBehaviour
{
    private Destructible destructible;
    private void Start()
    {
        destructible = GetComponentInParent<Destructible>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(destructible == null) { return; }
        destructible.CollideWith(other);
    }
}
