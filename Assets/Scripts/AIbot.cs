using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIBot : MonoBehaviour
{
    private GameManager gameManager;
    private GridManager gridManager;
    private WinChecker winChecker;

    private void Start()
    {
        gameManager = GameManager.instance;
        gridManager = GetComponent<GridManager>();
        winChecker = GetComponent<WinChecker>();

    }

    public void TakeTurn()
    {
        StartCoroutine(AITakeTurn());
    }

    private IEnumerator AITakeTurn()
    {
        yield return new WaitForSeconds(2f);
        //det col where Ai place tok
        int selectedColumn = AIMove();
        //valis col ind  
        if (selectedColumn != -1)
        {
            for (int row = 0; row < gridManager.rows; row++)
            {
                GameObject targetCell = gridManager.GetCell(selectedColumn, row);
                
                //cell not null and isempty
                if (targetCell != null && targetCell.transform.childCount == 0)
                {
                    gameManager.PlaceToken(targetCell);
                    
                    break;
                }
            }
        }
    }

    private int AIMove()
    {
        // Winning move for AI
        int move = FindMove(gameManager.currentPlayer + 1);
        if (move != -1)
        {
            return move;
        }

        // Blocking move against player
        int opponent = (gameManager.currentPlayer + 1) % 2 + 1;
        move = FindMove(opponent);
        
        if (move != -1)
        {
            return move;
        }

        // Random valid move
        return GetRandomMove();
    }


    private int FindMove(int player)
    {
        for (int col = 0; col < gridManager.columns; col++)
        {
            for (int row = 0; row < gridManager.rows; row++)
            {
                if (gridManager.GetCell(col, row).transform.childCount == 0)
                {
                    winChecker.UpdateGrid(col, row, player);
                    
                    bool isWinningMove = winChecker.CheckForWinFromCell(col, row, player);
                    // Reset the cell
                    winChecker.UpdateGrid(col, row, 0);
                    

                    if (isWinningMove)
                        return col;
                    //check lowest empty cell in column
                    break;
                }
            }
        }
        return -1;
    }


    private int GetRandomMove()
    {
        Debug.Log("Random Move Method");
        List<int> validColumns = new List<int>();
        for (int col = 0; col < gridManager.columns; col++)
        {
            if (gridManager.GetCell(col, 0).transform.childCount == 0)
            {
                validColumns.Add(col);              
            }          
        }
        if (validColumns.Count > 0)
        {
            Debug.Log("ValidCountColumn");
            return validColumns[Random.Range(0 , validColumns.Count)];
        }
        return -1;
    }
}



//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AIBot : MonoBehaviour
//{
//    private GameManager gameManager;
//    private GridManager gridManager;
//    private WinChecker winChecker;

//    private void Start()
//    {
//        gameManager = GameManager.instance;
//        gridManager = GetComponent<GridManager>();
//        winChecker = GetComponent<WinChecker>();
//    }

//    public void TakeTurn()
//    {
//        StartCoroutine(AITakeTurn());
//    }

//    private IEnumerator AITakeTurn()
//    {
//        yield return new WaitForSeconds(2f);
//        int selectedColumn = AIMove();
//        if (selectedColumn != -1)
//        {
//            for (int row = 0; row < gridManager.rows; row++)
//            {
//                GameObject targetCell = gridManager.GetCell(selectedColumn, row);

//                if (targetCell != null && targetCell.transform.childCount == 0)
//                {
//                    gameManager.PlaceToken(targetCell);

//                    break;
//                }
//            }
//        }
//    }

//    private int AIMove()
//    {
//        // Winning move for AI
//        int move = FindMove(gameManager.currentPlayer + 1);
//        if (move != -1)
//        {
//            return move;
//        }

//        // Blocking move against player
//        int opponent = (gameManager.currentPlayer + 1) % 2 + 1;
//        move = FindMove(opponent);
//        if (move != -1)
//        {
//            return move;
//        }

//        // Random valid move
//        return GetRandomMove();
//    }

//    private int FindMove(int player)
//    {
//        for (int col = 0; col < gridManager.columns; col++)
//        {
//            if (gridManager.GetCell(col, 0).transform.childCount != 0)
//            {
//                // Skip this column if it's full
//                continue;
//            }

//            for (int row = 0; row < gridManager.rows; row++)
//            {
//                if (gridManager.GetCell(col, row).transform.childCount == 0)
//                {
//                    winChecker.UpdateGrid(col, row, player);
//                    bool isWinningMove = winChecker.CheckForWinFromCell(col, row, player);
//                    // Reset the cell
//                    winChecker.UpdateGrid(col, row, 0);

//                    if (isWinningMove)
//                        return col;
//                    // Check lowest empty cell in column
//                    break;
//                }
//            }
//        }
//        return -1;
//    }

//    private int GetRandomMove()
//    {
//        Debug.Log("Random Move Method");
//        List<int> validColumns = new List<int>();
//        for (int col = 0; col < gridManager.columns; col++)
//        {
//            if (gridManager.GetCell(col, 0).transform.childCount == 0)
//            {
//                validColumns.Add(col);
//            }
//        }
//        if (validColumns.Count > 0)
//        {
//            Debug.Log("ValidCountColumn");
//            return validColumns[Random.Range(0, validColumns.Count)];
//        }
//        return -1;
//    }
//}
