using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    private bool paused;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(timer <= 0)
        {
            timer = 1;
            Pause();
        }
        timer -= Time.deltaTime;*/

    }

    public void OnPause()
    {
        paused = !paused;

        if (paused)
        {
            pauseMenu.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(false);
        }
    }
}
