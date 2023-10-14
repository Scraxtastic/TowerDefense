using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public PlayerSpawner spawner;
    public PlayerSpawner enemySpawner;
    public TextMeshProUGUI playerMoney;
    public TextMeshProUGUI enemyMoney;
    private BaseHandler playerBaseHandler;
    private BaseHandler enemyBaseHandler;

    private void Start()
    {
        playerBaseHandler = spawner.GetComponent<BaseHandler>();
        enemyBaseHandler = enemySpawner.GetComponent<BaseHandler>();
    }

    private void Update()
    {
        playerMoney.text = "" + playerBaseHandler.money;
        enemyMoney.text = "" + enemyBaseHandler.money;
    }

    public void SpawnUnit(string unitName)
    {
        spawner.SpawnUnit(unitName);
    }

    public void SpawnEnemy(string unitName)
    {
        enemySpawner.SpawnUnit(unitName);
    }
}