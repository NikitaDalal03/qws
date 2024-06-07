using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Timeline;



public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject[] playerTokens;
    public GameObject player1TokenUI;
    public GameObject player2TokenUI;

    private List<GameObject> tokenList;

    [SerializeField] private WinChecker winChecker;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private PlayersNameScreen playerName;

    public GameObject player1TurnPanel;
    public GameObject player2TurnPanel;

    public LineRenderer lineRenderer;

    private bool isGameOver = false;
    public bool isInputOn;

    public int currentPlayer = 0;
    private int totalMoves = 0;

    public bool isAgainstAI = false;
    private AIBot aiBot;

    public GameObject playerIndicatorPrefab;
    private GameObject playerIndicator;
    private float indicatorHeight = 0.03f;

    public GameState currentGameState = GameState.Initializing;
    private bool canTakeTurn = true;

    public string player1Name = "Player 1";
    public string player2Name = "Player 2";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        int columns = gridManager.columns;
        int rows = gridManager.rows;
        winChecker.InitializeGrid(columns, rows);

        tokenList = new List<GameObject>();
        HighlightCurrentPlayer();
        lineRenderer.enabled = false;

        isInputOn = true;

        playerIndicator = Instantiate(playerIndicatorPrefab, Vector3.zero, Quaternion.identity);
        UpdatePlayerIndicator();
        UpdateIndicatorPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        aiBot = gameObject.GetComponent<AIBot>();
      
    }

    void Update()
    {
        if (isGameOver)
            return;

        switch (currentGameState)
        {
            case GameState.PlayerTurn:
                HandlePlayerInput();
                break;
            case GameState.AITurn:
                HandleAITurn();
                break;
            case GameState.CheckingWin:
                
                break;
            default:
                break;
        }
    }


    private void HandlePlayerInput()
    {
        if (!isInputOn)
        {
            return;
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        bool isMouseAboveBoard = (hit.collider != null && (hit.collider.CompareTag("cell"))); //|| hit.collider.CompareTag("token")));
        playerIndicator.SetActive(isMouseAboveBoard);

        if (isMouseAboveBoard && isInputOn)
        {
            UpdateIndicatorPosition(mousePosition);
        }

        if (Input.GetMouseButtonDown(0) && isMouseAboveBoard)
        {
            Debug.Log("Check Mouse Input");
            if (hit.collider != null)
            {
                Debug.Log("Hit Collider");
                if(hit.collider.CompareTag("cell")) //|| hit.collider.CompareTag("token"))
                PlaceToken(hit.collider.gameObject);
            }
        }
    }


    private void HandleAITurn()
    {
        canTakeTurn = false;
        aiBot.TakeTurn();
        currentGameState = GameState.CheckingWin;
    }


    public void PlaceToken(GameObject cell)
    {
        Vector2Int cellIndex = GetCellIndex(cell);

        if (cellIndex.x != -1 && cellIndex.y != -1)
        {
            int column = cellIndex.x;
            bool isCellEmpty = false;

            for (int row = 0; row < gridManager.rows; row++)
            {
                GameObject targetCell = gridManager.GetCell(column, row);

                if (targetCell != null && targetCell.transform.childCount == 0)
                {
                    isCellEmpty = true;
                

                    Vector3 startPosition = new Vector3(targetCell.transform.position.x, gridManager.rows, targetCell.transform.position.z);
                    GameObject token = Instantiate(playerTokens[currentPlayer], startPosition, Quaternion.identity, targetCell.transform);
                    tokenList.Add(token);
                  

                    MoveTokenWithDOTween(token, targetCell.transform.position);
                    SoundManager.inst.PlaySound(SoundName.Ball);

                    winChecker.UpdateGrid(column, row, currentPlayer + 1);
                    totalMoves++;

                    bool isWinningMove = winChecker.CheckForWinFromCell(column, row, currentPlayer + 1);
                    if (isWinningMove)
                    {
                        HighlightWinningCells(winChecker.GetWinningCells(), currentPlayer + 1);
                        StartCoroutine(HandleWin(currentPlayer));
                        isGameOver = true;
                        currentGameState = GameState.GameOver;
                    }
                    else if (totalMoves >= gridManager.columns * gridManager.rows)
                    {
                        StartCoroutine(HandleDraw());
                        isGameOver = true;
                        currentGameState = GameState.GameOver;
                    }
                    else
                    {
                        currentPlayer = (currentPlayer + 1) % playerTokens.Length;
                        HighlightCurrentPlayer();
                        UpdatePlayerIndicator();

                        if (currentPlayer == 1 && isAgainstAI)
                        {
                            currentGameState = GameState.AITurn;
                        }
                        else
                        {
                            currentGameState = GameState.PlayerTurn;
                        }
                    }
                    break;
                }
            }

            if (!isCellEmpty)
            {
                //cell already occupied
                return;
            }
        }
    }


    Vector2Int GetCellIndex(GameObject cell)
    {
        for (int x = 0; x < gridManager.columns; x++)
        {
            for (int y = 0; y < gridManager.rows; y++)
            {
                if (gridManager.GetCell(x, y) == cell)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }


    IEnumerator HandleWin(int winningPlayer)
    {
        yield return new WaitForSeconds(2f);

        if (winningPlayer == 0)
        {
            UIManager.instance.SwitchScreen(GameScreens.Win1);
            lineRenderer.enabled = false;
        }
        else
        {
            UIManager.instance.SwitchScreen(GameScreens.Win2);
            lineRenderer.enabled = false;
        }
    }


    IEnumerator HandleDraw()
    {
        yield return new WaitForSeconds(1.5f);
        UIManager.instance.SwitchScreen(GameScreens.Draw);
    }


    void MoveTokenWithDOTween(GameObject token, Vector3 finalPosition)
    {
        float moveSpeed = 10f;
        float distance = Vector3.Distance(token.transform.position, finalPosition);
        float duration = distance / moveSpeed;

        token.transform.DOMove(finalPosition, duration).SetEase(Ease.Linear);
    }


    void HighlightWinningCells(List<Vector2Int> winningCells, int player)
    {
        Color highlightColor = Color.red;

        foreach (Vector2Int cellPos in winningCells)
        {
            GameObject cell = gridManager.GetCell(cellPos.x, cellPos.y);

            if (cell.transform.childCount > 0)
            {
                var spriteRenderer = cell.transform.GetChild(0).GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = highlightColor;
                }
            }
        }
        DrawWinningLine(winningCells);
    }


    void HighlightCurrentPlayer()
    {
        if (currentPlayer == 0)
        {
            player1TurnPanel.SetActive(true);
            player2TurnPanel.SetActive(false);
        }
        else
        {
            player1TurnPanel.SetActive(false);
            player2TurnPanel.SetActive(true);
        }
    }


    void DrawWinningLine(List<Vector2Int> winningCells)
    {
        if (winningCells.Count > 1)
        {
            StartCoroutine(AnimateLineDrawing(winningCells));
        }
    }


    IEnumerator AnimateLineDrawing(List<Vector2Int> winningCells)
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = winningCells.Count;

        Vector3[] positions = new Vector3[winningCells.Count];
        for (int i = 0; i < winningCells.Count; i++)
        {
            Vector2Int cellPos = winningCells[i];
            GameObject cell = gridManager.GetCell(cellPos.x, cellPos.y);
         
            if (cell != null)
            {
                Vector3 cellPosition = cell.transform.position;
                cellPosition.z -= 0.1f;
                positions[i] = cellPosition;
            }
        }

        for (int i = 0; i < positions.Length; i++)
        {
            lineRenderer.SetPosition(i, positions[0]);
        }

        float totalDuration = 1.0f;
        float segmentDuration = totalDuration / (positions.Length - 1);

        for (int i = 1; i < positions.Length; i++)
        {
            Vector3 startPosition = positions[i - 1];
            
            Vector3 endPosition = positions[i];

            yield return DOTween.To(() => startPosition, x => UpdateLineSegment(i, x), endPosition, segmentDuration)
                                .SetEase(Ease.Linear)
                                .WaitForCompletion();
        }
    }


    void UpdateLineSegment(int index, Vector3 position)
    {
        lineRenderer.SetPosition(index, position);
    }


    public void RestartGame()
    {
        isGameOver = false;
        ClearTokenList();
        winChecker.ResetGame();
        totalMoves = 0;
        currentPlayer = 0;
        HighlightCurrentPlayer();
        UpdatePlayerIndicator();
        playerName.Reset();
    }


    private void ClearTokenList()
    {
        for (int i = tokenList.Count - 1; i >= 0; i--)
        {
            GameObject.Destroy(tokenList[i]);
        }
        tokenList.Clear();
    }


    void UpdateIndicatorPosition(Vector2 mousePosition)
    {
        playerIndicator.transform.position = new Vector3(mousePosition.x, gridManager.rows + indicatorHeight, playerIndicator.transform.position.z);
    }


    void UpdatePlayerIndicator()
    {
        SpriteRenderer spriteRenderer = playerIndicator.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = playerTokens[currentPlayer].GetComponent<SpriteRenderer>().sprite;
    }


    public void SetGameMode(bool againstAI)
    {
        isAgainstAI = againstAI;
        currentGameState = GameState.PlayerTurn;
    }
}

public enum GameState
{
    Initializing,
    PlayerTurn,
    AITurn,
    CheckingWin,
    GameOver
}