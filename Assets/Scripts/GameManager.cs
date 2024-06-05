using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Timeline;

/* class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameObject[] playerTokens;

    public GameObject player1TokenUI;
    public GameObject player2TokenUI;
    private List<GameObject> tokenList;

    [SerializeField] private WinChecker winChecker;
    [SerializeField] private Canvas player1WinScreen;
    [SerializeField] private Canvas player2WinScreen;
    [SerializeField] private Canvas drawScreen;

    public GameObject player1TurnPanel;
    public GameObject player2TurnPanel;

    public LineRenderer lineRenderer;


    private bool isGameOver = false;
    public static bool isInputOn;

    private int currentPlayer = 0;
    private int totalMoves = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {        
        tokenList = new List<GameObject>();
        HighlightCurrentPlayer();
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isGameOver && isInputOn)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null)
            {
                PlaceToken(hit.collider.gameObject);
            }
        }
    }

    void PlaceToken(GameObject cell)
    {
        //determine the column of clicked cell
        int column = (int)Mathf.Round(cell.transform.position.x);

        for (int row = 0; row < 6; row++)
        {
            GameObject targetCell = GameObject.Find($"Cell ({column},{row})");
            
            //check if the cell is empty
            if (targetCell.transform.childCount == 0)
            {
                Vector3 startPosition = new Vector3(targetCell.transform.position.x, targetCell.transform.position.y + 6, targetCell.transform.position.z);
                GameObject token = Instantiate(playerTokens[currentPlayer], startPosition, Quaternion.identity, targetCell.transform);
                tokenList.Add(token);

                MoveTokenWithDOTween(token, targetCell.transform.position);

                //WinChecker winChecker = GetComponent<WinChecker>();
                winChecker.UpdateGrid(column, row, currentPlayer + 1);
                // Increment total moves
                totalMoves++; 

                if (winChecker.CheckForWinFromCell(column, row, currentPlayer + 1))
                {
                    Debug.Log($"Player {currentPlayer + 1} wins!");
                    HighlightWinningCells(winChecker.GetWinningCells(), currentPlayer + 1);
                    StartCoroutine(HandleWin(currentPlayer));
                    isGameOver = true;
                }
                else if (totalMoves >= 42) 
                {
                    StartCoroutine(HandleDraw());
                    isGameOver = true;
                }
                else
                {
                    currentPlayer = (currentPlayer + 1) % playerTokens.Length;
                    HighlightCurrentPlayer();
                }

                break;
            }
        }
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
        float moveSpeed = 15f;
        float distance = Vector3.Distance(token.transform.position, finalPosition);
        float duration = distance / moveSpeed;

        token.transform.DOMove(finalPosition, duration).SetEase(Ease.Linear);
    }

    void HighlightWinningCells(List<Vector2Int> winningCells, int player)
    {
        Debug.Log("Highlighting winning cells");
        Color highlightColor = Color.red;

        foreach (Vector2Int cellPos in winningCells)
        {
            GameObject cell = GameObject.Find($"Cell ({cellPos.x},{cellPos.y})");

            if (cell.transform.childCount > 0)
            {
                var spriteRenderer = cell.transform.GetChild(0).GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    Debug.Log($"Highlighting cell at position: {cellPos}");
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
            GameObject cell = GameObject.Find($"Cell ({cellPos.x},{cellPos.y})");
            if (cell != null)
            {
                Vector3 cellPosition = cell.transform.position;
                // Adjust the z-position to ensure the line is visible
                cellPosition.z -= 0.1f;
                positions[i] = cellPosition;
            }
        }

        // Initialize line renderer positions
        for (int i = 0; i < positions.Length; i++)
        {
            // Set all positions to start initially
            lineRenderer.SetPosition(i, positions[0]); 
        }

        float totalDuration = 1.0f;
        // Duration per segment
        float segmentDuration = totalDuration / (positions.Length - 1); 

        // Animate each segment of the line
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
    }

    private void ClearTokenList()
    {
        Debug.Log("TokenList: " + tokenList.Count);
        for (int i = tokenList.Count - 1; i >= 0; i--)
        {
            Debug.Log(tokenList[i] + ":" + i);
            GameObject.Destroy(tokenList[i]);
        }
        tokenList.Clear();
    }
}*/

public enum GameState
{
    Initializing,
    PlayerTurn,
    AITurn,
    CheckingWin,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject[] playerTokens;
    public GameObject player1TokenUI;
    public GameObject player2TokenUI;

    private List<GameObject> tokenList;

    [SerializeField] private WinChecker winChecker;
    [SerializeField] private GridManager gridManager;

    public GameObject player1TurnPanel;
    public GameObject player2TurnPanel;

    public LineRenderer lineRenderer;

    private bool isGameOver = false;
    public static bool isInputOn;

    public int currentPlayer = 0;
    private int totalMoves = 0;

    public bool isAgainstAI = false;
    private AIBot aiBot;

    public GameObject playerIndicatorPrefab;
    private GameObject playerIndicator;
    private float indicatorHeight = 0.03f;

    private bool canTakeTurn = true;

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
        if (isGameOver || !isInputOn)
            return;

        if (currentPlayer == 1 && isAgainstAI && canTakeTurn)
        {
            canTakeTurn = false;
            aiBot.TakeTurn();
            return;
        }

        if (!isAgainstAI || currentPlayer == 0)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            bool isMouseAboveBoard = (hit.collider != null && hit.collider.CompareTag("cell"));
            playerIndicator.SetActive(isMouseAboveBoard);

            if (isMouseAboveBoard && isInputOn)
            {
                UpdateIndicatorPosition(mousePosition);
            }

            if (Input.GetMouseButtonDown(0) && isMouseAboveBoard)
            {
                if (hit.collider != null)
                {
                    PlaceToken(hit.collider.gameObject);
                    canTakeTurn = true;
                }
            }
        }
    }

    public void PlaceToken(GameObject cell)
    {
        Vector2Int cellIndex = GetCellIndex(cell);

        if (cellIndex.x != -1 && cellIndex.y != -1)
        {
            int column = cellIndex.x;

            for (int row = 0; row < gridManager.rows; row++)
            {
                GameObject targetCell = gridManager.GetCell(column, row);

                if (targetCell != null && targetCell.transform.childCount == 0)
                {
                    Vector3 startPosition = new Vector3(targetCell.transform.position.x, gridManager.rows, targetCell.transform.position.z);
                    GameObject token = Instantiate(playerTokens[currentPlayer], startPosition, Quaternion.identity, targetCell.transform);
                    tokenList.Add(token);

                    MoveTokenWithDOTween(token, targetCell.transform.position);
                    SoundManager.inst.PlaySound(SoundName.Ball);

                    winChecker.UpdateGrid(column, row, currentPlayer + 1);
                    totalMoves++;

                    if (winChecker.CheckForWinFromCell(column, row, currentPlayer + 1))
                    {
                        HighlightWinningCells(winChecker.GetWinningCells(), currentPlayer + 1);
                        StartCoroutine(HandleWin(currentPlayer));
                        isGameOver = true;
                    }
                    else if (totalMoves >= gridManager.columns * gridManager.rows)
                    {
                        StartCoroutine(HandleDraw());
                        isGameOver = true;
                    }
                    else
                    {
                        currentPlayer = (currentPlayer + 1) % playerTokens.Length;
                        HighlightCurrentPlayer();
                        UpdatePlayerIndicator();
                    }
                    break;
                }
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
    }
}

