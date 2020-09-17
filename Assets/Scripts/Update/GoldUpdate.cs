using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldUpdate : MonoBehaviour
{
    public MenuManager menu;
    Text gold;

    private void Start()
    {
        gold = GetComponent<Text>();
    }

    void Update()
    {
        gold.text = "$ " + menu.playerData.Money.ToString();

    }
}
