using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsImg : MonoBehaviour
{
    public Texture2D texture;
    
    List<int> maps;
    Sprite[] sprites;
    GameObject[] images;   
    GameObject[] buttons;
    // Use this for initialization
    void Start()
    {
        buttons = new GameObject[8];
        images = new GameObject[8];
        sprites = Resources.LoadAll<Sprite>(texture.name);

        images[0] = GameObject.Find("lvl1");
        images[1] = GameObject.Find("lvl2");
        images[2] = GameObject.Find("lvl3");
        images[3] = GameObject.Find("lvl4");
        images[4] = GameObject.Find("lvl5");
        images[5] = GameObject.Find("lvl6");
        images[6] = GameObject.Find("lvl7");
        images[7] = GameObject.Find("lvl8");

        /*images[0].GetComponent<Image>().sprite = sprites[0];
        images[1].GetComponent<Image>().sprite = sprites[0];
        images[2].GetComponent<Image>().sprite = sprites[0];
        images[3].GetComponent<Image>().sprite = sprites[0];
        images[4].GetComponent<Image>().sprite = sprites[0];
        images[5].GetComponent<Image>().sprite = sprites[0];
        images[6].GetComponent<Image>().sprite = sprites[0];
        images[7].GetComponent<Image>().sprite = sprites[0];*/

        buttons[0] = GameObject.Find("Level1");
        buttons[1] = GameObject.Find("Level2");
        buttons[2] = GameObject.Find("Level3");
        buttons[3] = GameObject.Find("Level4");
        buttons[4] = GameObject.Find("Level5");
        buttons[5] = GameObject.Find("Level6");
        buttons[6] = GameObject.Find("Level7");
        buttons[7] = GameObject.Find("Level8");
        
    }   
    
    public void SetImg(int levelNumber, int starsNumber) 
    {
        images[levelNumber].GetComponent<Image>().sprite = sprites[starsNumber];
    }

    public List<int> Maps
    {
        get
        {
            return maps;
        }
        set
        {
            maps = value;
        }
    }

    private void Update()
    {
        for (int i = 0; i < 8; i++)
            SetImg(i, maps[i]);
    }

}
