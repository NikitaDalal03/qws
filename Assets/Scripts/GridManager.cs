/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public int rows = 6;
    public int columns = 7;
    public float cellSpacing = 0f;


    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                
                // Calculate the position for each cell
                Vector3 position = new Vector3(x * cellSpacing, y * cellSpacing, 0);

                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                // Name the cell 
                cell.name = $"Cell ({x},{y})";
                // Set the GridManager as the parent of the cell 
                cell.transform.parent = this.transform;
            }
        }
    }
}*/


using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject cellPrefab;
    public int rows = 6;
    public int columns = 7;
    public float cellSpacing = 1f;

    private GameObject[,] gridCells;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        gridCells = new GameObject[columns, rows];
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                Vector3 position = new Vector3(x * cellSpacing, y * cellSpacing, 0);
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity);
                cell.name = $"Cell ({x},{y})";
                cell.transform.parent = this.transform;
                gridCells[x, y] = cell;
            }
        }
    }

    public GameObject GetCell(int x, int y)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
        {
            return gridCells[x, y];
        }
        return null;
    }
}