using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TroopHealth : MonoBehaviour
{
    public Troop relativeTroop;

    [Header("Set automagically")] public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = relativeTroop.health;
        slider.maxValue = relativeTroop.maxHealth;
    }
}
