using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    private int[,] maze;
    private int width;
    private int height;

    private int cell = 1;
    private int wall = 0;
    private int unvisited = -1;

    private Stack<Vector2Int> activeCells = new Stack<Vector2Int>();

    private void initMaze()
    {
        maze = new int[width, height];
        for (int x = 1; x < width; x++)
        {
            for (int y = 1; y < height; y++)
            {
                if (( x % 2 != 0) && (y % 2 != 0))
                {
                    maze[x, y] = unvisited;
                }
                else
                {
                    maze[x, y] = wall;
                }
            }
        }
    }

    private List<Vector2Int> getUnvisedNeighbors(Vector2Int cell)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        if ((cell[0] - 2) > 0)
        {
            if (maze[cell[0] - 2,cell[1]] == unvisited) neighbors.Add(new Vector2Int(cell[0] - 2, cell[1]));  
        }
        if ((cell[0] + 2) < height-1)
        {
            if (maze[cell[0] + 2, cell[1]] == unvisited) neighbors.Add(new Vector2Int(cell[0] + 2, cell[1]));
        }
        if ((cell[1] - 2) > 0)
        {
            if (maze[cell[0], cell[1] - 2] == unvisited) neighbors.Add(new Vector2Int(cell[0], cell[1] - 2));
        }
        if ((cell[1] + 2) < width-1)
        {
            if (maze[cell[0], cell[1] + 2] == unvisited) neighbors.Add(new Vector2Int(cell[0], cell[1] + 2));
        }

        return neighbors;
    }

    private void removeWall(Vector2Int currentCell, Vector2Int chosenNeighbor)
    {
        Vector2Int sub = currentCell - chosenNeighbor;
        if (sub[0] == -2) maze[currentCell[0] + 1, currentCell[1]] = cell;
        if (sub[0] == 2) maze[currentCell[0] - 1, currentCell[1]] = cell;
        if (sub[1] == -2) maze[currentCell[0], currentCell[1] + 1] = cell;
        if (sub[1] == 2) maze[currentCell[0], currentCell[1] - 1] = cell;
    }

    private void createEntryAndExit()
    {
        bool entryMade = false;
        bool exitMade = false;
        for (int i = 0; i < width; i++)
        {
            if (!entryMade && (maze[1, i] == cell))
            {
                entryMade = true;
                maze[0, i] = cell;
            }
            if (!exitMade && (maze[height - 2, i] == cell))
            {
                exitMade = true;
                maze[height-1, i] = cell;
            }
        }
    }
    public int[,] generateNewMaze(int height, int width)
    {
        this.width = width;
        this.height = height;

        //initializing
        initMaze();

        //pick random starting cell
        int startHeight = Random.Range(1, height - 2);
        while (startHeight % 2 == 0)
        {
            startHeight = Random.Range(1, height - 2);
        }
        int startWidth = Random.Range(1, width - 2);
        while (startWidth % 2 == 0)
        {
            startWidth = Random.Range(1, width - 2);
        }

        Vector2Int currentCell = new Vector2Int(startHeight, startWidth);
        maze[currentCell[0], currentCell[1]] = cell;
        activeCells.Push(currentCell);

        while(activeCells.Count != 0)
        {
            currentCell = activeCells.Pop();
            List<Vector2Int> unvisitedNeighbors = getUnvisedNeighbors(currentCell);

            if (unvisitedNeighbors.Count > 0)
            {
                activeCells.Push(currentCell);
                Vector2Int randNeighbor = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count )];
                removeWall(currentCell, randNeighbor);
                maze[randNeighbor[0], randNeighbor[1]] = cell;
                activeCells.Push(randNeighbor);
            }
        }

        createEntryAndExit();

        return maze;
    }

    private string getListContent(List<Vector2Int> list)
    {
        string listContent = "";
        for (int i = 0; i < list.Count; i++)
        {
            Vector2Int element = list[i];
            listContent += " " + element.ToString();
        }
        return listContent;
    }
}
