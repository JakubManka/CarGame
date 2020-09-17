using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewards : MonoBehaviour
{

  
    public GameController gameController;
    MenuManager menu;
    private void Start()
    {
        menu = GameObject.FindGameObjectWithTag("menuManager").GetComponent<MenuManager>();
    }

    public int getReward()
    {
        int reward = 0;   
        switch (gameController.Place)
        {
            case 1:
                reward = 100;
                break;

            case 2:
                reward = 50;
                break;

            case 3:
                reward = 20;
                break;

            default:
                reward = 0;
                break;
        }             
        return reward;
    }
}
