using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class labyrinth_generator : MonoBehaviour
{
    private int[,] maze;
    private int width;
    private int height;
    private int cell = 1;
    private int wall = 0;
    private int unvisited = -1;
    private List<Vector2Int> walls = new List<Vector2Int>();

    private void initMaze()
    {
        maze = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = unvisited;
            }
        }
    }

    private int countSurroundingCells(Vector2Int cell)
    {
        int sCells = 0;
        if (maze[cell[0] - 1, cell[1]] == 1) sCells++;
        if (maze[cell[0] + 1, cell[1]] == 1) sCells++;
        if (maze[cell[0], cell[1] - 1] == 1) sCells++;
        if (maze[cell[0], cell[1] + 1] == 1) sCells++;

        return sCells;
    }

    private void makeWalls()
    {
        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (maze[x, y] == unvisited)
                {
                    maze[x, y] = wall;
                }
            }
        }
    }

    private void createEntranceAndExit()
    {
        for (int x = 0; x < width; x++)
        {
            if (maze[x,1] == cell)
            {
                maze[x,0] = cell;
            }
            if (maze[x, height-2] == cell)
            {
                maze[x, height-1] = cell;
            }
        }
    }

    public int[,] generateNewMaze(int w, int h)
    {
        Debug.Log("generating maze");

        this.width = w;
        this.height = h;

        //initializing
        initMaze();
        
        //pick random starting field
        int startHeight = Random.Range(1, height-2);
        int startWidth = Random.Range(1, width - 2);

        maze[startWidth, startHeight] = cell;
        walls.Add(new Vector2Int(startWidth + 1, startHeight));
        walls.Add(new Vector2Int(startWidth - 1, startHeight));
        walls.Add(new Vector2Int(startWidth, startHeight + 1));
        walls.Add(new Vector2Int(startWidth, startHeight - 1));

        maze[startWidth + 1, startHeight] = wall;
        maze[startWidth - 1, startHeight] = wall;
        maze[startWidth, startHeight + 1] = wall;
        maze[startWidth, startHeight - 1] = wall;

        int fieldAmount = width * height;
        int counter = 1;
        while ((walls.Count > 0) && (counter < 3*fieldAmount))
        {
            //pick random element from walls
            int randId = Random.Range(0, walls.Count);
            Vector2Int randWall = walls[randId];
            Debug.Log(randWall[0]);
            counter++;

            if ((randWall[1] != 0) && (maze[randWall[0],randWall[1] - 1] == unvisited) && (maze[randWall[0], randWall[1] + 1] == cell))
            {
                Debug.Log("case 1");
                int sCells = countSurroundingCells(randWall);
                if (sCells < 2) {
                    maze[randWall[0], randWall[1]] = cell;

                    if (randWall[0] != 0)
                    {
                        if (maze[randWall[0] - 1, randWall[1]] != cell)
                        {
                            maze[randWall[0] - 1,randWall[1]] = wall;
                            Vector2Int wallNew1 = new Vector2Int(randWall[0] - 1, randWall[1]);
                            if (!walls.Contains(wallNew1)) walls.Add(wallNew1);
                        }
                        if (maze[randWall[0] + 1, randWall[1]] != cell)
                        {
                            maze[randWall[0] + 1, randWall[1]] = wall;
                            Vector2Int wallNew2 = new Vector2Int(randWall[0] + 1, randWall[1]);
                            if (!walls.Contains(wallNew2)) walls.Add(wallNew2);

                        }
                        if (maze[randWall[0], randWall[1] - 1] != cell)
                        {
                            maze[randWall[0], randWall[1] - 1] = wall;
                            Vector2Int wallNew3 = new Vector2Int(randWall[0], randWall[1] - 1);
                            if (!walls.Contains(wallNew3)) walls.Add(wallNew3);

                        }
                    }
                }
                walls.RemoveAt(randId);
            }

            if ((randWall[0] != 0) && (maze[randWall[0]-1, randWall[1]] == unvisited) && (maze[randWall[0] + 1, randWall[1]] == cell))
            {
                Debug.Log("case 2");
                int sCells = countSurroundingCells(randWall);
                if (sCells < 2)
                {
                    maze[randWall[0], randWall[1]] = cell;

                    if (randWall[1] != 0)
                    {
                        if (maze[randWall[0], randWall[1]-1] != cell)
                        {
                            maze[randWall[0], randWall[1] - 1] = wall;
                            Vector2Int wallNew = new Vector2Int(randWall[0], randWall[1] - 1);
                            if (!walls.Contains(wallNew))
                            {
                                walls.Add(wallNew);
                            }
                        }
                        if (maze[randWall[0], randWall[1] + 1] != cell)
                        {
                            maze[randWall[0], randWall[1] + 1] = wall;
                            Vector2Int wallNew = new Vector2Int(randWall[0], randWall[1] + 1);
                            if (!walls.Contains(wallNew))
                            {
                                walls.Add(wallNew);
                            }
                        }
                        if (maze[randWall[0] - 1, randWall[1]] != cell)
                        {
                            maze[randWall[0], randWall[1] - 1] = wall;
                            Vector2Int wallNew = new Vector2Int(randWall[0] - 1, randWall[1]);
                            if (!walls.Contains(wallNew))
                            {
                                walls.Add(wallNew);
                            }
                        }
                    }
                }
                walls.RemoveAt(randId);
            }

            if ((randWall[0] != height - 1) && (maze[randWall[0] + 1, randWall[1]] == unvisited) && (maze[randWall[0] - 1, randWall[1]] == cell))
            {
                Debug.Log("case 4");
                int sCells = countSurroundingCells(randWall);
                if (sCells < 2)
                {
                    maze[randWall[0], randWall[1]] = cell;

                    if (randWall[1] != width - 1)
                    {
                        if (maze[randWall[0] - 1, randWall[1]] != cell)
                        {
                            maze[randWall[0] - 1, randWall[1]] = wall;
                            Vector2Int wallNew1 = new Vector2Int(randWall[0] - 1, randWall[1]);
                            if (!walls.Contains(wallNew1)) walls.Add(wallNew1);
                        }
                        if (maze[randWall[0] + 1, randWall[1]] != cell)
                        {
                            maze[randWall[0] + 1, randWall[1]] = wall;
                            Vector2Int wallNew2 = new Vector2Int(randWall[0] + 1, randWall[1]);
                            if (!walls.Contains(wallNew2)) walls.Add(wallNew2);

                        }
                        if (maze[randWall[0], randWall[1] + 1] != cell)
                        {
                            maze[randWall[0], randWall[1] - 1] = wall;
                            Vector2Int wallNew3 = new Vector2Int(randWall[0], randWall[1] - 1);
                            if (!walls.Contains(wallNew3)) walls.Add(wallNew3);

                        }
                    }
                }
                walls.RemoveAt(randId);
            }

            if ((randWall[1] != width - 1) && (maze[randWall[0], randWall[1] + 1] == unvisited) && (maze[randWall[0], randWall[1] - 1] == cell))
            {
                Debug.Log("case 3");
                int sCells = countSurroundingCells(randWall);
                if (sCells < 2)
                {
                    maze[randWall[0], randWall[1]] = cell;

                    if (randWall[0] != height - 1)
                    {
                        if (maze[randWall[0], randWall[1] - 1] != cell)
                        {
                            maze[randWall[0], randWall[1] - 1] = wall;
                            Vector2Int wallNew = new Vector2Int(randWall[0], randWall[1] - 1);
                            if (!walls.Contains(wallNew))
                            {
                                walls.Add(wallNew);
                            }
                        }
                        if (maze[randWall[0], randWall[1] + 1] != cell)
                        {
                            maze[randWall[0], randWall[1] + 1] = wall;
                            Vector2Int wallNew = new Vector2Int(randWall[0], randWall[1] + 1);
                            if (!walls.Contains(wallNew))
                            {
                                walls.Add(wallNew);
                            }
                        }
                        if (maze[randWall[0] + 1, randWall[1]] != cell)
                        {
                            maze[randWall[0], randWall[1] - 1] = wall;
                            Vector2Int wallNew = new Vector2Int(randWall[0] - 1, randWall[1]);
                            if (!walls.Contains(wallNew))
                            {
                                walls.Add(wallNew);
                            }
                        }
                    }
                }
                walls.RemoveAt(randId);
            }
        }
        

        makeWalls();

        return maze;
    }
}
