using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerData
{
    private int money;
    private List<int> maps;
    private List<string> cars;

    public PlayerData() { }

    public PlayerData(int money)
    {
        this.money = money;        
    }
    public PlayerData(int money, List<int> maps)
    {
        this.money = money;
        this.maps = maps;
    }

    public List<string> Cars
    {
        get { return cars; }

        set { cars = value; }
    }

    public int Money
    {
        get { return money; } 

        set { money = value; }
    }

    public List<int> Maps
    {
        get { return maps; }

        set { maps = value; }
    }
      

       

}
