using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MazeCreator : MonoBehaviour
{
    public GameObject defaultWall;
    public GameObject[] walls1;
    public GameObject[] walls2;
    public GameObject[] walls3;
    
    private MazeGenerator mazeGen;
    private GameObject floor;
    private GameObject start;
    private GameObject end;
    private GameObject player;

    public int width = 11;
    public int height = 11;

    public GameObject item;
    public GameObject enemy;

    public GameObject labyrinth;
    private GameObject items;
    private GameObject enemies;

    public GameObject floor2;

    void Start()
    {
        Debug.Log("start was called");
        items = new GameObject("items_1");
        enemies = new GameObject("enemies_1");

        player = GameObject.Find("Rin2_");
        //floor = GameObject.Find("floor");
        start = GameObject.Find("start");
        end = GameObject.Find("end");

        mazeGen = GameObject.Find("MazeGenerator").GetComponent<MazeGenerator>();
    }

    static int[,] default_maze = {
        {0,1,0,0,0,0,0,0,0,0,0},
        {0,1,1,1,1,1,0,1,1,1,0},
        {0,0,0,0,0,1,0,1,0,1,0},
        {0,1,1,1,1,1,0,1,0,1,0},
        {0,1,0,1,0,0,0,0,0,1,0},
        {0,1,0,1,0,1,1,1,0,1,0},
        {0,1,0,0,0,1,0,1,0,1,0},
        {0,1,1,1,1,1,0,1,1,1,0},
        {0,1,0,0,0,0,0,0,0,0,0},
        {0,1,1,1,1,1,1,1,1,1,0},
        {0,0,0,0,0,0,0,0,0,1,0},
    };


    public void createNewMaze(int level)
    {
        if (level % 2 != 0)
        {
            width = 10 + level;
            height = 10 + level;
        }
        else
        {
            width = 10 + level - 1;
            height = 10 + level - 1;
        }

        //floor2.transform.localScale = new Vector3(height+3, 1, width+1);
        //floor2.transform.position = new Vector3(height/2, floor.transform.position[1], width/2);

        Destroy(labyrinth);
        Destroy(items);
        Destroy(enemies);
        labyrinth = new GameObject("labyrinth_" + level);
        items = new GameObject("items_" + level);
        enemies = new GameObject("enemies_" + level);

        //GENERATE MAZE
        int[,] newMaze = mazeGen.generateNewMaze(height, width);
        makeMaze(newMaze, level);

        player.transform.position = new Vector3(-1, 0, 1);
    }

    private GameObject chooseWall()
    {
        // float rando = Random.Range(0f, 1f);
        // if (rando < 0.1f)
        // {
        //     return walls2[0];
        // }
        // else if (rando < 0.3f)
        // {
        //     return walls2[1];
        // }
        // else
        // {
        //     return box;
        // }

        return defaultWall;
    }

    private void makeMaze(int[,] maze, int level)
    {
        //Debug.Log(printMazeForDebug(maze));
        for (int x = 0; x < maze.GetLength(0); x++) {
            for (int y = 0; y < maze.GetLength(1); y++) {

                //generate walls
                if (maze[x,y] == 0) {
                    GameObject randomWall = chooseWall();
                    GameObject wallblock = GameObject.Instantiate(randomWall);
                    wallblock.transform.position = new Vector3(x,-0.44f,y);
                    wallblock.transform.SetParent(labyrinth.transform);
                }

                //generate items
                if (maze[x, y] == 1)
                {
                    float chance = Random.Range(0f, 1f);
                    if (chance < 0.1f)
                    {
                        GameObject newItem = GameObject.Instantiate(item);
                        newItem.transform.position = new Vector3(x, newItem.transform.position.y, y);
                        newItem.transform.SetParent(items.transform);
                    }
                }
                //generate start und end
                if((x == 0) && (maze[x,y] == 1))
                {
                    start.transform.position = new Vector3(x, 0, y);
                }
                if ((x == height-1) && (maze[x, y] == 1))
                {
                    end.transform.position = new Vector3(x+1, 0, y);
                }

            }
        }

        int enemyAmount = (int) (1+Mathf.Ceil(level/4));
        for (int x = 0; x < enemyAmount; x++) {
            //generate enemies
            GameObject newEnemy = GameObject.Instantiate(enemy);
            newEnemy.transform.position = new Vector3(Random.Range(0, width), 0, Random.Range(0, height));
            newEnemy.transform.SetParent(enemies.transform);
        }
    }

    private string printMazeForDebug(int[,] maze)
    {
        string print = "";
        print += "\n";
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            for (int y = 0; y < maze.GetLength(1); y++)
            {
                print += maze[x,y];
            }
            print += "\n";
        }
        return print;
    }

    void OnApplicationQuit()
    {
        Destroy(items);
        Destroy(enemies);
    }
    
}
