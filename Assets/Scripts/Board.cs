using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] public int width;
    [SerializeField] public int height;
    [SerializeField] private GameObject[] tilePrefabs;


    public Tile[,] gameTiles;
    public bool tilesDropped = false;
    public Tile currentTileSwapped;
    public List<Tile> specialTiles;

    private float dropOffset = 1.5f;
    private FindMatches findMatches;
    private GameManager gameManager;
    private HintsManager hintManager;
    private Board board;

    // Start is called before the first frame update
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        gameManager = FindObjectOfType<GameManager>();
        hintManager = FindObjectOfType<HintsManager>();
        board = FindObjectOfType<Board>();
    }
    private void Awake()
    {
        gameTiles = new Tile[width, height];
        specialTiles = new List<Tile>();
    }

    public void ActivateSpecialTiles()
    {
        foreach(Tile t in specialTiles)
        {
            t.canMatchBomb = true;
        }
    }

    private void DeactivateSpecialTiles()
    {
        foreach (Tile t in specialTiles)
        {
            t.canMatchBomb = false;
        }
    }

    public void CallFillBoard()
    {
        StartCoroutine(FillBoard1());
    }

    IEnumerator FillBoard1()
    {
        tilesDropped = false;
        yield return new WaitForSeconds(0.4f);
        FillBoard();
       
    }

    public void FillBoard()
    {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (gameTiles[i, j] == null)
                    {
                        Vector2 tempPosition = new Vector2(i, j + dropOffset);
                        int tileToUse = Random.Range(0, tilePrefabs.Length);


                    int maxIterations = 0;

                    while (findMatches.MatchesAtPosition(i,j,tilePrefabs[tileToUse].gameObject.tag)&&maxIterations<100)
                    {
                        tileToUse = Random.Range(0, tilePrefabs.Length);
                        ++maxIterations;
                    }

                    maxIterations = 0;

                        GameObject tile = Instantiate(tilePrefabs[tileToUse], tempPosition, Quaternion.identity);
                        Tile tileTileComponent = tile.GetComponent<Tile>();

                        gameTiles[i, j] = tile.GetComponent<Tile>();
                        //Debug.Log(gameTiles[i, j] == null);
                        tileTileComponent.row = j;
                        tileTileComponent.column = i;

                        tile.transform.parent = this.transform;
                        tile.name = "(" + i + "," + j + ")";
                    }
                }
            }

        if (hintManager.stillHinting)
        {
            gameManager.canMove = false;
            tilesDropped = true;
        }
        else
        {
            gameManager.canMove = true;
            if (gameManager.level2)
            {
                foreach (Tile t in findMatches.currentmatches)
                {
                    if (t.isColorBomb || t.isColumbBomb || t.isRowBomb)
                    {
                        ++gameManager.score;
                    }
                }
            }
            else
            {
                gameManager.score += findMatches.currentmatches.Count;
            }
            //gameManager.score += findMatches.currentmatches.Count;
            //findMatches.currentmatches.Clear();
        }
        //gameManager.canMove = true;
        findMatches.currentmatches.Clear();

        gameManager.CheckGameState();

    }

    
}
