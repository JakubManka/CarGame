using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [HideInInspector]
    public GameObject player;
    [HideInInspector]
    public GameObject choosedCar;

    public GameObject car1;
    public GameObject car2;
    public GameObject car3;

    public CountLaps countLaps;
    public Text lapsText;
    public Text placeText;
    public List<GameObject> PlayerCars;

    [HideInInspector]
    public MenuManager menu;

    public GameObject finishCanvas;
    public GameObject hud;
    public GameObject menuCanvas;
    public Speedometr speedometr;


    public Rewards rewards;

    private GameObject minimap;

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
        minimap = GameObject.Find("MinimapCamera");
        SetActiveCar();
        player = choosedCar;
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
        if (laps[0] == 1)
        {
            finishCanvas.SetActive(true);
            hud.SetActive(false);
            placeText.text = "YOU FINISHED " + place.ToString();
            menu.playerData.Money += rewards.getReward();
            setGoldText();
            
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

    private void SetActiveCar()
    {
        string name;

        foreach(var car in PlayerCars)
        {
            name = car.name.ToLower().Replace("player", "");
            if (name == menu.playerData.ChoosedCar.ToString().ToLower())
            {
                choosedCar = car;
                choosedCar.name = "ChoosedCar";
                minimap.transform.parent = choosedCar.transform;
                speedometr.rb = choosedCar.GetComponent<Rigidbody>();
                countLaps.cars[0] = choosedCar.GetComponent<CarEngineAgent>();
                setCarSpeed();
            }
            else           
                car.SetActive(false);                       
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

    public void SaveGame()
    {
        menu.SaveGame();
    }

    public void TryAgain()
    {
        menu.TryAgain();
    }

    public void ExitToMenu()
    {
        menu.ExitToMenu();
        menu.DestroyMenu();
    }

    public void ResumeGame()
    {
        menu.ResumeGame();
    }

    private void setCarSpeed()
    {
        if (menu.playerData.Cars[menu.playerData.ChoosedCar] == 2)
        {
            choosedCar.GetComponent<CarEngineAgent>().maxAcceleration += 1;
        }
    }
}
