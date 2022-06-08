using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static System.Math;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI itemText;
    public GameObject levelFinishScreen;
    public GameObject pauseMenu;
    public GameObject timeAndLevel;
    //public TextMeshProUGUI stats;

    public float maxTime = 3;
    public float itemExtraTime = 10;
    public float nextLevelExtraTime = 30;
    public float enemyHitTime = -30;

    private MazeCreator mazeCreator;
    private AudioManager audioM;
    private GameObject player;

    private bool countTime = false;
    private bool levelFinished = false;
    private float startTime;
    private int level;
    private int itemCount;
    private float extraTime;

    private bool screenShown = false;

    // Start is called before the first frame update
    void Start()
    {
        level = 1;
        levelText.SetText("Level 1");
        startTime = maxTime - Time.time;
        itemCount = 0;
        extraTime = 0;

        audioM = GameObject.Find("ManagerAudio").GetComponent<AudioManager>();
        player = GameObject.Find("Rin2_");
        mazeCreator = GameObject.Find("MazeCreator").GetComponent<MazeCreator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("escape pressed");
            pauseGame();
        }

        float timeVal = maxTime + extraTime - Time.time;
        if ((timeVal < 0) || (player.transform.position.y < -10)) {
            gameOver();
        }
        else {
            if(countTime)
            {
                string minutes = ((int)timeVal / 60).ToString();
                string seconds = ((int)(timeVal % 60)).ToString();

                timeText.SetText(minutes + ":" + seconds);
            }

            if(levelFinished)
            {
                showLevelFinishScreen();
            }

        }
    }


    private void gameOver()
    {
        audioM.Play("gameOver");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void showLevelFinishScreen()
    {
        //string timeFinished = timeText.text;
        timeAndLevel.SetActive(false);
        levelFinishScreen.SetActive(true);
        //stats.SetText("You finished level " + level.ToString() + " in ");
    }

    public void nextLevel()
    {
        level++;

        //gen new level 
        mazeCreator.createNewMaze(level);

        levelFinished = false;
        levelText.SetText("Level " + level.ToString());

        levelFinishScreen.SetActive(false);
        timeAndLevel.SetActive(true);
    }

    public void pauseGame()
    {
        if (!levelFinishScreen.activeSelf) {
            player.GetComponent<PlayerController>().isPaused = true;
        player.GetComponent<CharController>().isPaused = true;
        countTime = false;
        timeAndLevel.SetActive(false);
        pauseMenu.SetActive(true);
        }
    }

    public void unpauseGame()
    {
        player.GetComponent<PlayerController>().isPaused = false;
        player.GetComponent<CharController>().isPaused = false;
        countTime = true;
        timeAndLevel.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void startTimer()
    {
        countTime = true;
    }

    public void stopTimer()
    {
        countTime = false;
        levelFinished = true;
    }

    public void gatherItem()
    {
        itemCount++;
        itemText.SetText("Items: " + itemCount.ToString());
        extraTime += itemExtraTime;
    }

    public void enemy1Hit() {
        extraTime += enemyHitTime ;
    }

    public void exit()
    {
        Application.Quit();
    }

}
