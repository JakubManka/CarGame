using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountLaps : MonoBehaviour
{    
    public CarEngineAgent[] cars;   

    int[] laps = { 0, 0, 0, 0 };
    int[] checkpoints = { 0, 0, 0, 0 };

    MenuManager menu;
    public GameController gameController;

    bool[] done = { false, false, false, false };
   
    public int[] Laps
    {
        get { return laps; }
    }


    void Start()
    {
        menu = GameObject.FindGameObjectWithTag("menuManager").GetComponent<MenuManager>();
        cars[0] = gameController.choosedCar.GetComponent<CarEngineAgent>();
    }

    
    void Update()
    {
        checkpoints[0] = cars[0].CheckPointIndex;
        checkpoints[1] = cars[1].CheckPointIndex;
        checkpoints[2] = cars[2].CheckPointIndex;
        checkpoints[3] = cars[3].CheckPointIndex;

        SetCheckpoints();
    }

    void SetCheckpoints()
    {
        for(int i = 0; i < 4; i++)
        {
            if(checkpoints[i] == 0 && done[i] == false)
            {                
                done[i] = true;
                laps[i]++;              
            }

            if(checkpoints[i] == 3 && done[i] == true)
            {
                done[i] = false;
            }
        }
    }
}
