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
            //Check if the troopname matches
            if (spawnableTroop.name != name)
            {
                continue;
            }
            //Check if the player / The base has enough money
            if (spawnableTroop.requiredMoney > baseHandler.money)
            {
                //at this point we know, that it's the only possible destination for the loop and no other iteration of the loop is required
                return;
            }
            baseHandler.money -= spawnableTroop.requiredMoney;
            Debug.Log($"{baseHandler.name}.money = {baseHandler.money}");
            GameObject createdTroop = Instantiate(spawnableTroop.troop, transform);
            Troop troop = createdTroop.GetComponent<Troop>();
            troop.moneyDropOnDeath = spawnableTroop.moneyEarnedOnDeath;
            baseHandler.troops.Add(troop);
            createdTroop.transform.position = relativeSpawnPoint + transform.position;
            createdTroop.transform.name = $"{baseName}-{name}-{count}";
            count++;
            return;
        }
    }
}