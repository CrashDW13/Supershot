using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//The end gate is the thing the player has to touch in order to beat the game.
public class EndGate : MonoBehaviour
{
    


    private Timer timer;

    private void Start()
    {
        timer = GameObject.Find("Timer").GetComponent<Timer>(); 
    }

    private void GoToEnd(string status)
    {
        RankData.time = timer.GetTimerData();
        RankData.status = status; 
        SceneManager.LoadScene("EndGame");
    }
}
