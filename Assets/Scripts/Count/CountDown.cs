using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CountDown : MonoBehaviour
{
    public int countdownTime;
    public Text timeText;
    public GameController gameController;   

    private void Start()
    {
        Time.timeScale = 0;
        StartCoroutine(CountdownToStart());
    }
   
    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            timeText.text = countdownTime.ToString();

            yield return new WaitForSecondsRealtime(1f);

            countdownTime--;
        }
        timeText.text = "GO!";

        gameController.gameStarted = true;

        Time.timeScale = 1;

        yield return new WaitForSecondsRealtime(0.5f);

        timeText.gameObject.SetActive(false);

    }


}
