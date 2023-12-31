using System.Collections;
using System.Collections.Generic;
// using System.Diagnostics.CodeAnalysis;
//using Unity.VisualScripting;
//using UnityEditor.iOS.Xcode;
using UnityEngine;
//using UnityEngine.SceneManagement;

public enum GameState
{
    wait,
    move
}

public enum TileKind
{
    Breakable,
    Blank,
    Normal
}

[System.Serializable]
public class TileType
{
    public int x;
    public int y;
    public TileKind tileKind;
}

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public int offSet;
    public int basePieceValue = 20;
    public int[] scoreGoals;
    public float refillDelay = 0.5f;
    public GameObject tilePrefab;
    public GameObject breakableTilePrefab;
    public GameState currentState = GameState.move;
    public GameObject[] dots;
    public GameObject destroyEffect;
    public GameObject[,] allDots;
    public TileType[] boardLayout;
    public Dot currentDot;

    private bool[,] blankSpaces;
    private int streakValue = 1;
    private FindMatches findeMatches;
    private BackgroundTile[,] breakableTile;
    private ScoreManager scoreManager;
    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        findeMatches = FindObjectOfType<FindMatches>();
        allDots = new GameObject[width, height];
        blankSpaces = new bool[width, height];
        breakableTile = new BackgroundTile[width, height];
        scoreManager = FindObjectOfType<ScoreManager>();
        soundManager = FindObjectOfType<SoundManager>();
        Setup();
    }

    public void GenerateBlankSpaces()
    {
        for (int i = 0; i < boardLayout.Length; i++)
        {
            if (boardLayout[i].tileKind == TileKind.Blank)
            {
                blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
            }
        }
    }

    public void GenerateBreakableTiles()
    {
        // Look at all the tiles in the layout
        for (int i = 0; i < boardLayout.Length; i++)
        {
            // if a tile is a "Breakable" tile
            if (boardLayout[i].tileKind == TileKind.Breakable)
            {
                // create a "Breakable" tile at that position
                Vector2 tempPosition = new Vector2(boardLayout[i].x, boardLayout[i].y);
                GameObject tile = Instantiate(breakableTilePrefab, tempPosition, Quaternion.identity);
                breakableTile[boardLayout[i].x, boardLayout[i].y] = tile.GetComponent<BackgroundTile>();
            }
        }
    }

    private void Setup()
    {
        GenerateBlankSpaces();
        GenerateBreakableTiles();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!blankSpaces[i, j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    Vector2 tilePosition = new Vector2(i, j);
                    GameObject backgroundTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;
                    backgroundTile.transform.parent = this.transform;
                    backgroundTile.name = "( " + i + ", " + j + " )";
                    int dotToUse = Random.Range(0, dots.Length);
                    int maxIterations = 0;

                    while (MatchesAt(i, j, dots[dotToUse]) &&
                           maxIterations < 100)
                    {
                        dotToUse = Random.Range(0, dots.Length);
                        maxIterations++;
                    }

                    maxIterations = 0;

                    GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    dot.GetComponent<Dot>().row = j;
                    dot.GetComponent<Dot>().column = i;
                    dot.transform.parent = this.transform;
                    dot.name = "( " + i + ", " + j + " )";
                    allDots[i, j] = dot;
                }
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
            if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
            {
                if (allDots[column - 1, row].tag == piece.tag &&
                    allDots[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }

            if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
            {
                if (allDots[column, row - 1].tag == piece.tag &&
                allDots[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
                {
                    if (allDots[column, row - 1].tag == piece.tag &&
                    allDots[column, row - 2].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }

            if (column > 1)
            {
                if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
                {
                    if (allDots[column - 1, row].tag == piece.tag &&
                    allDots[column - 2, row].tag == piece.tag)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private bool ColumnOrRow()
    {
        int numberHorizontal = 0;
        int numberVertical = 0;

        Dot firstPiece = findeMatches.currentMatches[0].GetComponent<Dot>();

        if (firstPiece != null)
        {
            foreach (GameObject currentPiece in findeMatches.currentMatches)
            {
                Dot dot = currentPiece.GetComponent<Dot>();
                if (dot != null)
                {
                    if (dot.row == firstPiece.row)
                    {
                        numberHorizontal++;
                    }
                    if (dot.column == firstPiece.column)
                    {
                        numberVertical++;
                    }
                }
            }
        }

        return (numberVertical == 5 || numberHorizontal == 5);
    }

    private void CheckToMakeBombs()
    {
        if (findeMatches.currentMatches.Count == 4 || findeMatches.currentMatches.Count == 7)
        {
            findeMatches.CheckBombs();
        }

        if (findeMatches.currentMatches.Count == 5 || findeMatches.currentMatches.Count == 8)
        {
            if (ColumnOrRow())
            {
                // Make a Color bomb
                // is the current dot matched?
                //Debug.Log("Make a color bomb");
                if (currentDot != null)
                {
                    if (currentDot.isMatched)
                    {
                        if (!currentDot.isColorBomb)
                        {
                            currentDot.isMatched = false;
                            currentDot.MakeColorBomb();
                        }
                    }
                }
                else
                {
                    if (currentDot.otherDot != null)
                    {
                        Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                        if (otherDot.isMatched)
                        {
                            if (!otherDot.isColorBomb)
                            {
                                otherDot.isMatched = false;
                                otherDot.MakeColorBomb();
                            }
                        }
                    }
                }
            }
            else
            {
                // Make a adjacent bomb
                //Debug.Log("Make a adjacent bomb");
                if (currentDot != null)
                {
                    if (currentDot.isMatched)
                    {
                        if (!currentDot.isAdjacentBomb)
                        {
                            currentDot.isMatched = false;
                            currentDot.MakeAdjacentBomb();
                        }
                    }
                }
                else
                {
                    if (currentDot.otherDot != null)
                    {
                        Dot otherDot = currentDot.otherDot.GetComponent<Dot>();
                        if (otherDot.isMatched)
                        {
                            if (!otherDot.isAdjacentBomb)
                            {
                                otherDot.isMatched = false;
                                otherDot.MakeAdjacentBomb();
                            }
                        }
                    }
                }
            }
        }
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().isMatched)
        {
            // How many elements are in the matched pieces list from finadmatches?
            if (findeMatches.currentMatches.Count >= 4)
            {
                CheckToMakeBombs();
            }

            // Does a tile need to break?
            if (breakableTile[column, row] != null)
            {
                breakableTile[column, row].TakeDamage(1);
                if (breakableTile[column, row].hitPoints <= 0)
                {
                    breakableTile[column, row] = null;
                }
            }

            // Does the sound manager exist?
            if(soundManager != null)
            {
                soundManager.PlayRandomDestroyNoise();
            }

            GameObject particle = Instantiate(destroyEffect, allDots[column, row].transform.position, Quaternion.identity);
            Destroy(particle, .5f);
            Destroy(allDots[column, row]);
            scoreManager.IncreaseScore(basePieceValue * streakValue);
            allDots[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }

        findeMatches.currentMatches.Clear();
        StartCoroutine(DecreaseRowCo2());
    }

    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    ++nullCount;
                }
                else if (nullCount > 0)
                {
                    allDots[i, j].GetComponent<Dot>().row -= nullCount;
                    allDots[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(DecreaseRowCo());
    }

    private IEnumerator DecreaseRowCo2()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // if the current spot isn't blank and is empty
                if (!blankSpaces[i, j] && allDots[i, j] == null)
                {
                    // loop from the space above to the top of the column
                    for (int k = j + 1; k < height; k++)
                    {
                        // if a dot is found
                        if (allDots[i, k] != null)
                        {
                            // move that dot to do this empty space
                            allDots[i, k].GetComponent<Dot>().row = j;

                            // set that spot to be null
                            allDots[i, k] = null;

                            // break out of the loop
                            break;
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(refillDelay * 0.5f);
        StartCoroutine(FillBoardCo());
    }


    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null && !blankSpaces[i, j])
                {
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int dotToUse = Random.Range(0, dots.Length);
                    int maxIterations = 0;
                    while(MatchesAt(i,j, dots[dotToUse]) &&
                        maxIterations < 100)
                    {
                        maxIterations++;
                        dotToUse = Random.Range(0, dots.Length);
                    }

                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        yield return new WaitForSeconds(refillDelay);

        while (MatchesOnBoard())
        {
            streakValue++;
            DestroyMatches();
            yield return new WaitForSeconds(2 * refillDelay);
        }
        findeMatches.currentMatches.Clear();
        currentDot = null;

        if (IsDeadlocked())
        {
            StartCoroutine(ShuffleBoard());
            Debug.Log("Deadlocked!!!");
        }
        currentState = GameState.move;
        streakValue = 1;
    }

    private void SwitchPieces(int column, int row, Vector2 direction)
    {
        // Take the second piece and save it in a holder
        GameObject holder = allDots[column + (int)direction.x, row + (int)direction.y] as GameObject;

        // Switching the first dot to be 
        allDots[column + (int)direction.x, row + (int)direction.y] = allDots[column, row];

        // Set the first dot to be the second dot
        allDots[column, row] = holder;
    }

    private bool CheckForMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    // Make sure that one and two to the right are in the board
                    if (i < width - 2)
                    {
                        if (allDots[i + 1, j] != null && allDots[i + 2, j] != null)
                        {
                            if (allDots[i + 1, j].tag == allDots[i, j].tag &&
                                allDots[i + 2, j].tag == allDots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }

                    if (j < height - 2)
                    {
                        // Check if the dots above exist
                        if (allDots[i, j + 1] != null &&
                            allDots[i, j + 2] != null)
                        {
                            if (allDots[i, j + 1].tag == allDots[i, j].tag &&
                                allDots[i, j + 2].tag == allDots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }

        return false;
    }

    public bool SwitchAndCheck(int column, int row, Vector2 direction)
    {
        SwitchPieces(column, row, direction);
        if (CheckForMatches())
        {
            SwitchPieces(column, row, direction);
            return true;
        }

        SwitchPieces(column, row, direction);
        return false;
    }

    private bool IsDeadlocked()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (i < width - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.right))
                        {
                            return false;
                        }
                    }
                    if (j < height - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.up))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    private IEnumerator ShuffleBoard()
    {
        yield return new WaitForSeconds(0.5f);

        // Create a list of game objects
        List<GameObject> newBoard = new List<GameObject>();
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    newBoard.Add(allDots[i, j]);
                }
            }
        }

        yield return new WaitForSeconds(0.5f);

        // for every spot on the board
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height ; j++)
            {
                // if this spot shouldn't be blank
                if (!blankSpaces[i,j])
                {
                    // Pick a random number
                    int pieceToUse = Random.Range(0, newBoard.Count);
                    
                    // Make a container for the piece
                    Dot piece = newBoard[pieceToUse].GetComponent<Dot>();

                    int maxIterations = 0;

                    while (MatchesAt(i, j, newBoard[pieceToUse]) &&
                           maxIterations < 100)
                    {
                        pieceToUse = Random.Range(0, newBoard.Count);
                        maxIterations++;
                        Debug.Log(maxIterations);
                    }
                    maxIterations = 0;

                    // Assign the column to the piece
                    piece.column = i;
                    
                    // Assign the row to the pice
                    piece.row = j;

                    // Fill int the dots array with this new piece
                    allDots[i, j] = newBoard[pieceToUse];

                    // Remove it from the list
                    newBoard.Remove(newBoard[pieceToUse]);
                }
            }
        }

        // Check if it's still deadlocked
        if(IsDeadlocked())
        {
            StartCoroutine(ShuffleBoard());
        }
    }
}