using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCarUpdate : MonoBehaviour
{
    Text carText;
    public MenuManager menu;

    Dictionary<Ecars, int> carCost;    

    private void Start()
    {
        carCost = new Dictionary<Ecars, int>();

        carCost.Add(Ecars.Ford ,0);
        carCost.Add(Ecars.F2,100);
        carCost.Add(Ecars.Cybertruck, 150);        
        carCost.Add(Ecars.F1, 300);
        carCost.Add(Ecars.Lambo, 500);
    }

    void Update()
    {
        updateText();
    }

    void updateText()
    {
        string carString;
        foreach (var car in menu.playerData.Cars)
            {
            if (car.Value >= 1)
            {
                carString = car.Key.ToString() + "Text";
                carText = GameObject.Find(carString).GetComponent<Text>();              
                carText.text = "OWNED";
            }
            if(car.Key == menu.playerData.ChoosedCar)
            {
                carString = car.Key.ToString() + "Text";
                carText = GameObject.Find(carString).GetComponent<Text>();
                carText.text = "CHOOSED";
            }
            if (car.Value == 0)
            {
                print(car.Key);
                carString = car.Key.ToString() + "Text";                
                carText = GameObject.Find(carString).GetComponent<Text>();              
                carText.text = carCost[car.Key].ToString() + "$";
            }
        }
    }

    public void BuyCar(Button button)
    {
        string name = button.name;

       foreach(var car in carCost)
        {
            if(car.Key.ToString() + "Button" == name)
            {
                if(menu.playerData.Money >= car.Value && menu.playerData.Cars[car.Key] == 0)
                {
                    menu.playerData.Money -= car.Value;
                    menu.playerData.Cars[car.Key] = 1;
                }else if(menu.playerData.Cars[car.Key] > 0)
                {
                    menu.playerData.ChoosedCar = car.Key;
                }
            }
        }
    }
}
