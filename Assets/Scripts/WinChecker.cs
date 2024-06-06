using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public class WinChecker : MonoBehaviour
{
    private int[,] grid = new int[7, 6];
    private List<Vector2Int> winningCells = new List<Vector2Int>();
    private List<Vector2Int> cells;

    public bool CheckForWin(int player)
    {
        winningCells.Clear();
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                if (CheckDirection(x, y, 1, 0, player) ||
                    CheckDirection(x, y, 0, 1, player) ||
                    CheckDirection(x, y, 1, 1, player) ||
                    CheckDirection(x, y, 1, -1, player))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool CheckForWinFromCell(int startX, int startY, int player)
    {
        return CheckDirection(startX, startY, 1, 0, player) ||  // Horizontal
               CheckDirection(startX, startY, 0, 1, player) ||  // Vertical
               CheckDirection(startX, startY, 1, 1, player) ||  // Diagonal up
               CheckDirection(startX, startY, 1, -1, player);   // Diagonal down
    }

    bool CheckDirection(int startX, int startY, int dirX, int dirY, int player)
    {
        int count = 0;
        cells = new List<Vector2Int>();

        // Check in the positive direction
        for (int i = 0; i < 4; i++)
        {
            int x = startX + i * dirX;
            int y = startY + i * dirY;

            if (x < 0 || x >= 7 || y < 0 || y >= 6)
                break;

            if (grid[x, y] == player)
            {
                count++;
                cells.Add(new Vector2Int(x, y));
            }
            else
                break;
        }

        // Check in the negative direction
        for (int i = 1; i < 4; i++)
        {
            int x = startX - i * dirX;
            int y = startY - i * dirY;

            if (x < 0 || x >= 7 || y < 0 || y >= 6)
                break;

            if (grid[x, y] == player)
            {
                count++;
                cells.Add(new Vector2Int(x, y));
            }
            else
                break;
        }

        if (count >= 4)
        {
            winningCells.AddRange(cells);
            return true;
        }
        return false;
    }

    public void UpdateGrid(int x, int y, int player)
    {
        grid[x, y] = player;
    }

    public List<Vector2Int> GetWinningCells()
    {
        return winningCells;
    }    

    public void ResetGame()
    {
        grid = new int[7, 6];
        winningCells.Clear();
    }
}*/



public class WinChecker : MonoBehaviour
{
    private int[,] grid;
    private int columns;
    private int rows;
    private List<Vector2Int> winningCells = new List<Vector2Int>();

    public void InitializeGrid(int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;
        grid = new int[columns, rows];
    }

    public bool CheckForWinFromCell(int startX, int startY, int player)
    {
        return CheckDirection(startX, startY, 1, 0, player) ||  // Horizontal
               CheckDirection(startX, startY, 0, 1, player) ||  // Vertical
               CheckDirection(startX, startY, 1, 1, player) ||  // Diagonal up
               CheckDirection(startX, startY, 1, -1, player);   // Diagonal down
    }

    bool CheckDirection(int startX, int startY, int dirX, int dirY, int player)
    {
        int count = 0;
        List<Vector2Int> cells = new List<Vector2Int>();

        // Check in the positive direction
        for (int i = 0; i < 4; i++)
        {
            int x = startX + i * dirX;
            int y = startY + i * dirY;

            if (x < 0 || x >= columns || y < 0 || y >= rows)
            {
                break;
            }

            if (grid[x, y] == player)
            {
                count++;
                cells.Add(new Vector2Int(x, y));
            }
            else
                break;
        }

        // Check in the negative direction
        for (int i = 1; i < 4; i++)
        {
            int x = startX - i * dirX;
            int y = startY - i * dirY;

            if (x < 0 || x >= columns || y < 0 || y >= rows)
            {
                break;
            }

            if (grid[x, y] == player)
            {
                count++;
                cells.Add(new Vector2Int(x, y));
            }
            else
            {
                break;
            }
        }

        if (count >= 4)
        {
            winningCells.AddRange(cells);
            return true;
        }
        return false;
    }

    public void UpdateGrid(int x, int y, int player)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
        {
            grid[x, y] = player;
        }
    }

    public List<Vector2Int> GetWinningCells()
    {
        return winningCells;
    }

    public void ResetGame()
    {
        grid = new int[columns, rows];
        winningCells.Clear();
    }
}
