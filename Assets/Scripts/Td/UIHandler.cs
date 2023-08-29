using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public PlayerSpawner spawner;
    public PlayerSpawner enemySpawner;
    public void SpawnUnit(string unitName)
    {
        spawner.SpawnUnit(unitName);
    }

    public void SpawnEnemy(string unitName)
    {
        enemySpawner.SpawnUnit(unitName);
    }
}