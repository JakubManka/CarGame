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
        if(menu.choosedDifficulty.ToLower().Equals("easy"))
        {
            switch (gameController.Place)
            {
                case 1:
                    reward = 10;
                    break;

                case 2:
                    reward = 5;
                    break;

                case 3:
                    reward = 2;
                    break;

                default:
                    reward = 0;
                    break;
            }

        }
        else if (menu.choosedDifficulty.ToLower().Equals("medium"))
        {
            switch (gameController.Place)
            {
                case 1:
                    reward = 20;
                    break;

                case 2:
                    reward = 10;
                    break;

                case 3:
                    reward = 5;
                    break;

                default:
                    reward = 1;
                    break;
            }
        }
        else if (menu.choosedDifficulty.ToLower().Equals("hard"))
        {
            switch (gameController.Place)
            {
                case 1:
                    reward = 40;
                    break;

                case 2:
                    reward = 20;
                    break;

                case 3:
                    reward = 10;
                    break;

                default:
                    reward = 5;
                    break;
            }
        }
        return reward;
    }

    public int getStarReward()
    {
        int star = 0;
        if (menu.choosedDifficulty.ToLower().Equals("easy"))
        {
            star = 1;
        }else if (menu.choosedDifficulty.ToLower().Equals("medium"))
        {
            star = 2;
        }
        else if (menu.choosedDifficulty.ToLower().Equals("hard"))
        {
            star = 3;
        }
        return star;
    }
}
