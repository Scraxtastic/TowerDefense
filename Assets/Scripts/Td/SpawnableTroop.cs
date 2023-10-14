using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public struct SpawnableTroop
{
    public string name;
    public float requiredMoney;
    public float moneyEarnedOnDeath;
    public GameObject troop;
}
