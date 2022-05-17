using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Simple class that handles the start button. 
public class Begin : MonoBehaviour
{
    //String for the game itself. 
    private string gameScene;
    [SerializeField] private Animator transition;

    void Start()
    {
        gameScene = "MainGame";
    }

    //Functions for UI buttons to call. 
    public void StartGame()
    {
        StartCoroutine(StartLevel(gameScene)); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator StartLevel(string sceneName)
    {
        transition.SetTrigger("FadeIn");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName); 
    }
}
