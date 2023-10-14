using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BaseHandler : MonoBehaviour
{
    public bool headingRight = true;
    public float troopSpeedMultiplier = 1;
    public BaseHandler enemyBase;
    public List<Troop> troops = new List<Troop>();
    [Header("Money")] public float money = 100;
    public float moneyEarnRate = 3;
    [Header("Set automagically")] public float enemyX;
    public Troop enemyTroop;
    public bool haveTroopsBeenUpdatedThisFrame = false;
    public Troop baseTroop;
    private bool active = true;
    [Header("Set by enemy")] public bool isAnyTroopDead;

    private void Start()
    {
        baseTroop = GetComponent<Troop>();
    }

    private void Update()
    {
        if (!active)
        {
            return;
        }

        haveTroopsBeenUpdatedThisFrame = false;
        enemyX = enemyBase.GetNearestTroopX();
        enemyTroop = enemyBase.GetNearestTroop();
        for (var i = 0; i < troops.Count; i++)
        {
            UpdateTroop(i, Time.deltaTime);
        }
    }

    private void UpdateFrontTroop(Troop currentTroop, Vector3 troopPosition)
    {
        if (headingRight)
        {
            if (troopPosition.x + currentTroop.borderDistance >= enemyX)
            {
                troopPosition.x = enemyX - currentTroop.borderDistance;
            }

            if (enemyTroop != null && troopPosition.x + currentTroop.range > enemyTroop.transform.position.x)
            {
                //In Range, don't move
                return;
            }
        }

        if (!headingRight)
        {
            if (troopPosition.x - currentTroop.borderDistance <= enemyX)
            {
                troopPosition.x = enemyX + currentTroop.borderDistance;
            }

            if (enemyTroop != null && troopPosition.x - currentTroop.range < enemyTroop.transform.position.x)
            {
                //In Range, don't move
                return;
            }
        }

        currentTroop.transform.position = troopPosition;
        return;
    }

    private void UpdateTroop(int currentIndex, float delta)
    {
        Troop currentTroop = troops[currentIndex];
        Vector3 troopPosition = currentTroop.transform.position;
        troopPosition.x += troopSpeedMultiplier * currentTroop.speed * delta;
        // Front troop only (others avoid going through the front troop)
        if (currentIndex == 0)
        {
            UpdateFrontTroop(currentTroop, troopPosition);
            return;
        }

        Troop previousTroop = troops[currentIndex - 1];
        if (headingRight)
        {
            //Borders shall fit (right border from the left unit never goes further than left border from right unit)
            float previousLeftBorder =
                previousTroop.transform.position.x -
                previousTroop.borderDistance;
            float currentRightBorder = troopPosition.x + currentTroop.borderDistance;
            if (currentRightBorder > previousLeftBorder)
            {
                troopPosition.x -= currentRightBorder - previousLeftBorder;
                if (troopPosition.x < currentTroop.transform.position.x)
                {
                    troopPosition.x = currentTroop.transform.position.x;
                }
            }
        }
        else
        {
            //Borders shall fit (right border from the left unit never goes further than left border from right unit)
            float previousRightBorder =
                previousTroop.transform.position.x +
                previousTroop.borderDistance;
            float currentLeftBorder = troopPosition.x - currentTroop.borderDistance;
            if (currentLeftBorder < previousRightBorder)
            {
                troopPosition.x += previousRightBorder - currentLeftBorder;
                if (troopPosition.x > currentTroop.transform.position.x)
                {
                    troopPosition.x = currentTroop.transform.position.x;
                }
            }
        }

        currentTroop.transform.position = troopPosition;
    }

    public void TryAttackEnemy(Troop currentTroop)
    {
        if (enemyTroop == null)
        {
            print("No Enemy Found (This should never happen)");
            return;
        }

        if (currentTroop.lastAttackTime + 1000 / currentTroop.attackSpeed > Time.time)
        {
            return;
        }

        currentTroop.Attack();
        enemyTroop.health -= currentTroop.attackDamage;
        enemyTroop.DamageReceived();
        if (enemyTroop.health <= 0)
        {
            enemyBase.isAnyTroopDead = true;
        }

        currentTroop.lastAttackTime = Time.time;
    }

    public float GetNearestTroopX()
    {
        Troop nearestTroop;
        if (troops.Count == 0)
        {
            nearestTroop = baseTroop;
        }
        else
        {
            nearestTroop = troops[0];
        }

        if (headingRight)
        {
            return nearestTroop.transform.position.x + nearestTroop.borderDistance;
        }

        return nearestTroop.transform.position.x - nearestTroop.borderDistance;
    }

    public Troop GetNearestTroop()
    {
        if (troops.Count == 0)
        {
            return baseTroop;
        }

        return troops[0];
    }

    public void AttackWithTroops()
    {
        foreach (Troop currentTroop in troops)
        {
            Vector3 troopPosition = currentTroop.transform.position;

            if (headingRight)
            {
                if (troopPosition.x + currentTroop.range >= enemyX)
                {
                    TryAttackEnemy(currentTroop);
                }
            }
            else
            {
                // if (troopPosition.x <= enemyX + currentTroop.range)
                // {
                //     TryAttackEnemy(currentTroop);
                // }
                if (enemyTroop.transform.position.x + currentTroop.range >=
                    currentTroop.transform.position.x - currentTroop.borderDistance)
                {
                    TryAttackEnemy(currentTroop);
                }
            }
        }
    }

    public void DestroyDeadTroops()
    {
        if (!isAnyTroopDead)
        {
            return;
        }

        List<Troop> troopsToRemove = new List<Troop>();
        foreach (Troop troop in troops)
        {
            if (troop.health <= 0)
            {
                troopsToRemove.Add(troop);
            }
        }

        foreach (Troop troopToRemove in troopsToRemove)
        {
            troopToRemove.HasDied();
            enemyBase.money += troopToRemove.moneyDropOnDeath;
            troops.Remove(troopToRemove);
            Destroy(troopToRemove.gameObject);
        }
    }

    public void LateUpdate()
    {
        if (haveTroopsBeenUpdatedThisFrame)
        {
            return;
        }

        haveTroopsBeenUpdatedThisFrame = true;
        enemyBase.haveTroopsBeenUpdatedThisFrame = true;
        enemyBase.AttackWithTroops();
        AttackWithTroops();
        enemyBase.DestroyDeadTroops();
        DestroyDeadTroops();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = headingRight ? Color.cyan : Color.yellow;
        float borderDistance = baseTroop ? baseTroop.borderDistance : 0;
        Gizmos.DrawWireCube(transform.position, new Vector3(borderDistance, 1));
    }
}