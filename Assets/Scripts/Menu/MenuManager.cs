using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Ecars
{
    Ford,
    F2,
    Cybertruck,    
    F1,
    Lambo
}


public class MenuManager : MonoBehaviour
{
    public LevelsImg levelsImg;

    
    public Text levelNumber;   
    [HideInInspector]
    public string choosedDifficulty;
    [HideInInspector]
    public string choosedLevel;   

    [HideInInspector]
    public int level;

  
    Data data;
    public PlayerData playerData;       


    private void Start()
    {       
        level = 1;       
        data = new Data();
        playerData = new PlayerData();
    }

    private void Update()
    {
        
    }
    public void Play()
    {
        string lvlNumber;        
        lvlNumber = choosedLevel.Replace("Level", "");
        lvlNumber = lvlNumber.Replace(" ", "");  
        level = Int16.Parse(lvlNumber);              
        SceneManager.LoadScene($"Lvl{level}");
        Time.timeScale = 1;
    }
   
    public void ChooseLevel(Button button)
    {
        choosedLevel = button.name;  
        levelNumber.text = choosedLevel.ToUpper();
    }
    public void ChooseDifficulty(Button button)
    {
        choosedDifficulty = button.name;       
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void TryAgain()
    {        
        String scene = SceneManager.GetActiveScene().name;            
        SceneManager.LoadScene(scene);
        
    }

    public void LoadLevels()
    {
        playerData.Maps = data.playerData.Maps;
        levelsImg.Maps = playerData.Maps;        
    }


    public void SaveGame()
    {
        data.SaveData(playerData.Money, playerData.Maps,playerData.Cars, playerData.ChoosedCar);
    }

    public void NewGame()
    {
        playerData = new PlayerData();
        data.SaveData(playerData.Money, playerData.Maps, playerData.Cars,playerData.ChoosedCar);
        levelsImg.Maps = playerData.Maps;
        data.LoadData();
    }

    public void LoadGame()
    {       
        data.LoadData();        
        playerData = data.playerData;
        levelsImg.Maps = playerData.Maps;
    }

    private void Awake()
    {
        MenuManager dontDestroyMenu = this;
        dontDestroyMenu.name = "dontDestroyMenu";
        DontDestroyOnLoad(dontDestroyMenu.gameObject);
    }    

    public void UpgradeCar()
    {
        if(playerData.Cars[playerData.ChoosedCar] == 1 && playerData.Money >= 10)
        {
            playerData.Cars[playerData.ChoosedCar] = 2;
            playerData.Money -= 10;
            Text upgradeText = GameObject.Find("UpgradeText").GetComponent<Text>();       
            upgradeText.text = "UPGRADED";            
        }            
    } 
    
    public void DestroyMenu()
    {
        GameObject menu = GameObject.Find("dontDestroyMenu");
        if(menu != null)
            Destroy(menu);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
