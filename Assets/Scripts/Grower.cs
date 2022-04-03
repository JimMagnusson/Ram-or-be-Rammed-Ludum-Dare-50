using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grower : MonoBehaviour
{
    [SerializeField] float sizeIncreasePerPart = 0.05f;

    [SerializeField] Transform toGrow;
    public void Grow()
    {
        toGrow.localScale += new Vector3(sizeIncreasePerPart, sizeIncreasePerPart, sizeIncreasePerPart);
    }
}
