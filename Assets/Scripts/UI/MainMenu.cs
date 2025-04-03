using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menu;
    public void ExitGame()
    {
        
            Application.Quit();
        

    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(sceneName: "SC_Combat");
    }

    public void Update()
    {
        if (Input.GetKey("escape") && menu!=null)
        {
            menu.gameObject.SetActive(true);
        }

    }
}
