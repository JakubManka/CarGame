using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerData
{
    private int money;
    private List<int> maps;
    private Ecars choosedCar;
    private Dictionary<Ecars, int> cars; //int -> 0 - Don't have car / 1 - have car / 2 - have upgraded car

    public PlayerData() 
    {
        maps = new List<int>();
        cars = new Dictionary<Ecars, int>();

        for (int i = 0; i < 8; i++)
            maps.Add(0);

        
        cars.Add(Ecars.Cybertruck, 0);
        cars.Add(Ecars.F1, 0);
        cars.Add(Ecars.F2, 0);
        cars.Add(Ecars.Ford, 1);
        cars.Add(Ecars.Lambo, 0);

        money = 0;

        choosedCar = Ecars.Ford;
    }

    public PlayerData(int money, List<int> maps, Dictionary<Ecars, int> cars, Ecars choosedCar)
    {
        this.money = money;
        this.maps = maps;
        this.cars = cars;
        this.choosedCar = choosedCar;
    }

    public Dictionary<Ecars, int> Cars
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

    public Ecars ChoosedCar
    {
        get { return choosedCar; }

        set { choosedCar = value; }
    }
}
