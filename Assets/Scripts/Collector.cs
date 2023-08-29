using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Collector : MonoBehaviour
{
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.transform.CompareTag("Collectable"))
        {
            return;
        }

        Collectable collectable = other.transform.GetComponent<Collectable>();
        
    }
}
