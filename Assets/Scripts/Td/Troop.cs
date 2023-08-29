using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Troop : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public float borderDistance;
    public float range;
    public bool usesBullet;
    public float attackSpeed;
    public float attackDamage;
    public Color gizmoColor = Color.black;
    [Header("DO NOT CHANGE")] public float lastAttackTime;

    public void Attack()
    {
        
    }

    public void DamageReceived()
    {
        
    }

    public void HasDied()
    {
        print(this.name + " has died in war.");
    }
    private void OnDrawGizmos()
    {
        var position = transform.position;
        Vector3 left = position;
        left.x -= borderDistance;
        Vector3 right = position;
        right.x += borderDistance;
        Gizmos.DrawLine(left, right);
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(position, range);
        Gizmos.DrawWireCube(position, new Vector3(range * 2, range * 2));
    }
}