
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThanksForPlaying : MonoBehaviour
{
    
    public void ExitGame()
    {

        Application.Quit();


    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(sceneName: "SC_Combat");
    }
}