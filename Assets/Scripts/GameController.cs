using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public GameObject car1;
    public GameObject car2;
    public GameObject car3;
    public CountLaps countLaps;
    public Text lapsText;
    public Text placeText;

    public MenuManager menu;
    public GameObject finishCanvas;
    public GameObject hud;
    public GameObject menuCanvas;


    public Rewards rewards;

    int place;
    bool paused;
    [HideInInspector]
    public bool gameStarted;

    int[] laps = { 0, 0, 0, 0 };

    bool raceFinished;

    void Start()
    {
        raceFinished = false;
        place = 1;
        gameStarted = false;
        paused = false;
       /* Time.timeScale = 1;*/
        menu = GameObject.FindGameObjectWithTag("menuManager").GetComponent<MenuManager>();
    }

    private void Update()
    {
        if (raceFinished == false)
            FinishRace();
        lapsText.text = laps[0].ToString() + "/4";
        
        PauseGame();
    }

    void PauseGame()
    {

        if (Input.GetButton("Cancel") && gameStarted == true)
        {
            if (paused == false)
            {
                menuCanvas.SetActive(true);
                Time.timeScale = 0;
            }           
        }
    }

    void FinishRace()
    {
        laps = countLaps.Laps;
        if (laps[0] == 4)
        {
            finishCanvas.SetActive(true);
            hud.SetActive(false);
            placeText.text = "YOU FINISHED " + place.ToString();
            menu.money += rewards.getReward();
            setGoldText();

            if (place == 1)
            {
                int star = rewards.getStarReward();

                if (menu.maps[menu.level - 1] < star)
                {
                    menu.maps[menu.level - 1] = star;
                }
            }
            raceFinished = true;
        }
        if (laps[1] == 1)
        {
            place++;
            laps[1] = 100;
        }
        if (laps[2] == 1)
        {
            place++;
            laps[2] = 100;
        }
        if (laps[3] == 1)
        {
            place++;
            laps[3] = 100;
        }
    }
    

    public int Place
    {
        get { return place; }
    }

    public void setGoldText()
    {
        Text moneyText = GameObject.Find("MoneyText").GetComponent<Text>();
        moneyText.text = $"EARNED {rewards.getReward()} GOLD";
    }

}
