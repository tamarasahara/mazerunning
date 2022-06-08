using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Debug.Log("quitting game");
            Application.Quit();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void exit()
    {
        Debug.Log("quitting game");
        Application.Quit();
    }
}
