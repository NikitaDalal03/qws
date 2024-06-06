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
        gridManager = FindObjectOfType<GridManager>();
        winChecker = FindObjectOfType<WinChecker>();
    }

    public void TakeTurn()
    {
        StartCoroutine(AITakeTurn());
    }

    private IEnumerator AITakeTurn()
    {
        yield return new WaitForSeconds(2f);
        int selectedColumn = AIMove();
        if (selectedColumn != -1)
        {
            for (int row = 0; row < gridManager.rows; row++)
            {
                GameObject targetCell = gridManager.GetCell(selectedColumn, row);
                if (targetCell != null && targetCell.transform.childCount == 0)
                {
                    gameManager.PlaceToken(targetCell);
                    // No need for a break here as PlaceToken handles the game state transition
                    break; // Added a break here to ensure only one token is placed
                }
            }
        }
        else
        {
            // No valid move for the AI, transition to PlayerTurn
            gameManager.currentGameState = GameState.PlayerTurn;
        }
    }
    private int AIMove()
    {
        // Prioritize winning move for AI
        int Move = FindMove(gameManager.currentPlayer + 1);
        if (Move != -1)
            return Move;

        // Prioritize blocking move against player
        int opponent = (gameManager.currentPlayer + 1) % 2 + 1;
        Move = FindMove(opponent);
        if (Move != -1)
            return Move;

        // If no strategic move, choose a random valid move
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
                    winChecker.UpdateGrid(col, row, 0); // Reset the cell

                    if (isWinningMove)
                        return col;

                    break;// Only need to check the lowest empty cell in the column
                }
            }
        }
        return -1;
    }

    private int GetRandomMove()
    {
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
            return validColumns[Random.Range(0, validColumns.Count)];
        }
        return -1;
    }
}



//AI input after pauseScreen
//filled cell click
//userInput name
//Indicator blink
//whoever won will get first chance to play
