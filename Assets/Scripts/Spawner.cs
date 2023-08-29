using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int createCount = 10;
    public GameObject objectToCreate;
    public bool shallDestroyAtFinish = true;
    private int createdCount = 0;

    private void Update()
    {
        if (createdCount >= createCount)
        {
            if (shallDestroyAtFinish)
                Destroy(this);
            return;
        }

        Instantiate(objectToCreate, transform);
        createdCount++;
    }
}