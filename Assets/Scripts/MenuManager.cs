using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public LevelsImg levelsImg;

    
    public Text levelNumber;
    [HideInInspector]
    public string choosedDifficulty;
    [HideInInspector]
    public string choosedLevel;
    [HideInInspector]
    public int money;
    [HideInInspector]
    public int level;

    public List<int> maps;
    
    Data data;
    PlayerData playerData;
    MenuManager menu;


    private void Start()
    {
        menu = GameObject.FindGameObjectWithTag("menuManager").GetComponent<MenuManager>();
        level = 1;
        maps = new List<int>();
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
        menu.maps = data.playerData.Maps;
        levelsImg.Maps = menu.maps;        
    }

    public void LoadGold()
    {
        Text goldText = GameObject.Find("GoldText").GetComponent<Text>();
        goldText.text = data.playerData.Money.ToString();
    }

    public void SaveGame()
    {
        menu.data.SaveData(menu.money, menu.maps);
    }

    public void SaveOnce()
    {
        for (int i = 0; i < 8; i++)
            maps.Add(0);
        money = 10;
        menu.data.SaveData(money, maps);
    }

    public void LoadGame()
    {       
        data.LoadData();
        maps = data.playerData.Maps;
        levelsImg.Maps = maps;
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }    
}
