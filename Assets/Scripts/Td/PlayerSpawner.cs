using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public BaseHandler baseHandler;
    public SpawnableTroop[] spawnableTroops;
    public string baseName;
    public Vector3 relativeSpawnPoint;
    public int count = 0;

    public void SpawnUnit(string name)
    {
        foreach (SpawnableTroop spawnableTroop in spawnableTroops)
        {
            if (spawnableTroop.name != name)
            {
                continue;
            }

            //Remove Coins from Base
            GameObject createdTroop = Instantiate(spawnableTroop.troop, transform);
            Troop troop = createdTroop.GetComponent<Troop>();
            baseHandler.troops.Add(troop);
            createdTroop.transform.position = relativeSpawnPoint + transform.position;
            createdTroop.transform.name = $"{baseName}-{name}-{count}";
            count++;
            return;
        }
    }
}