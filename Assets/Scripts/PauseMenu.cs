using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Manager m;
    public AudioManager audioM;

    private bool isPaused = false;

    void OnEnable()
    {
        isPaused = true;
    }
    /*
    void Update()
    {
        if (isPaused)
        {
            if (Input.GetKey("escape"))
            {
                unpause();
            }
        }
    }
    */
    public void unpause()
    {
        m.unpauseGame();
    }

    public void exitGame()
    {
        m.exit();
    }

    void OnDisable()
    {
        isPaused = false;
    }
}
