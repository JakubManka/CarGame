using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUpdate : MonoBehaviour
{

    public MenuManager menu;  
    public List<GameObject> cars;
    

    public void Update()
    {
        setCarActive();
    }

    public void ResetCars()
    {     
        foreach(var car in cars)
        {
            car.SetActive(true);           
        }
    }

    public void setCarActive()
    {       
        string carUpgrade = menu.playerData.ChoosedCar.ToString() + "Upgrade";

        foreach (var car in cars)
        {
            if (car.name != carUpgrade)
            {
                car.SetActive(false);                       
            }
        }

        if (menu.playerData.Cars[menu.playerData.ChoosedCar] == 1)
        {
            Text upgradeText = GameObject.Find("UpgradeText").GetComponent<Text>();
            upgradeText.text = "UPGRADE\n 10$";
        }

        if (menu.playerData.Cars[menu.playerData.ChoosedCar] == 2)
        {
            Text upgradeText = GameObject.Find("UpgradeText").GetComponent<Text>();
            upgradeText.text = "UPGRADED";
        }
    }
}
