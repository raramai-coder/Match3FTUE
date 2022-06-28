using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMatches : MonoBehaviour
{
    private Board board;
    private DestroyMatches destroyMatches;
    private GameManager gameManager;

    private bool matchedColorBomb = false;

    [SerializeField] public List<Tile> currentmatches;
    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        destroyMatches = FindObjectOfType<DestroyMatches>();
        gameManager = FindObjectOfType<GameManager>();
    }


    public void CallFindMatches()
    {
        StartCoroutine(CheckMatches());
    }

    private IEnumerator CheckMatches()
    {
        yield return new WaitForSeconds(0.5f);
        foreach(Tile currentTile in board.gameTiles)
        {
            if (currentTile != null)
            {
                if (currentTile.row < board.height - 1 && currentTile.row > 0)
                {
                    if (board.gameTiles[currentTile.column, currentTile.row + 1] != null && board.gameTiles[currentTile.column, currentTile.row - 1] != null)
                    {
                        CheckNeighbours(board.gameTiles[currentTile.column, currentTile.row - 1], board.gameTiles[currentTile.column, currentTile.row + 1], currentTile);

                    }
                }

                if (currentTile.column < board.width - 1 && currentTile.column > 0)
                {
                    if (board.gameTiles[currentTile.column - 1, currentTile.row] != null && board.gameTiles[currentTile.column + 1, currentTile.row])
                    {
                        CheckNeighbours(board.gameTiles[currentTile.column - 1, currentTile.row], board.gameTiles[currentTile.column + 1, currentTile.row], currentTile);
                    }
                }
            }
            


        }

        /*if (board.currentTileSwapped != null)
        {
            if (board.currentTileSwapped.isColorBomb && board.currentTileSwapped.canMatchBomb)
            {
                ColorBombSwap(board.currentTileSwapped,true);
            }
            else if (board.currentTileSwapped.dotToSwipeWith!=null &&board.currentTileSwapped.dotToSwipeWith.isColorBomb && board.currentTileSwapped.dotToSwipeWith.canMatchBomb)
            {
                ColorBombSwap(board.currentTileSwapped.dotToSwipeWith,false);
            }
        }*/

        if (board.currentTileSwapped != null)
        {
            if (board.currentTileSwapped.isColorBomb && board.currentTileSwapped.canMatchBomb)
            {
                ColorBombSwap(board.currentTileSwapped, true);
                board.currentTileSwapped.isMatched = true;
                //matchedColorBomb = true;

            }
            else if (board.currentTileSwapped.dotToSwipeWith != null && board.currentTileSwapped.dotToSwipeWith.isColorBomb && board.currentTileSwapped.dotToSwipeWith.canMatchBomb)
            {
                ColorBombSwap(board.currentTileSwapped, false);
               // matchedColorBomb = true;
            }

            
        }


        //gameManager.score += currentmatches.Count;

        if (currentmatches.Count > 3)
        {
            CheckBombs();
            //matchedColorBomb = false;
        }
        

        destroyMatches.DestroyAllMatches();
    }

    private void RCBomb(Tile tile1, Tile tile2, Tile tile3, bool column)
    {

        if (column)
        {
            if (tile1.isColumbBomb && tile1.canMatchBomb)
            {
                MatchColumnPieces(tile1.column);
            }

            if (tile2.isColumbBomb && tile2.canMatchBomb)
            {
                MatchColumnPieces(tile2.column);
            }

            if (tile3.isColumbBomb && tile3.canMatchBomb)
            {
                MatchColumnPieces(tile3.column);
            }
        }
        else
        {
            if (tile1.isRowBomb && tile1.canMatchBomb)
            {
                MatchRowPieces(tile1.row );
            }
            if (tile2.isRowBomb && tile2.canMatchBomb)
            {
                MatchRowPieces(tile2.row);
            }
            if (tile3.isRowBomb && tile3.canMatchBomb)
            {
                MatchRowPieces(tile3.row);
            }
        }
    }

    private void MatchColumnPieces(int column)
    {

        for (int i = 0; i < board.height; i++)
        {
            if (board.gameTiles[column, i] != null)
            {
                AddToListAndMatch(board.gameTiles[column, i]);
            }
        }

    }

    private void MatchRowPieces(int row)
    {

        for (int i = 0; i < board.width; i++)
        {
            if (board.gameTiles[i, row] != null)
            {
                AddToListAndMatch(board.gameTiles[i, row]);
            }
        }

    }


    private void ColorBombSwap(Tile tile, bool colorBombSwapped)
    {

        if (tile.dotToSwipeWith != null && colorBombSwapped)
        {
            foreach (Tile t in board.gameTiles)
            {
                if (t != null && t.gameObject.CompareTag(tile.dotToSwipeWith.gameObject.tag))
                {
                    AddToListAndMatch(t);
                }
            }

            
        }
        else if(!colorBombSwapped)
        {
            foreach (Tile t in board.gameTiles)
            {
                if (t != null && t.gameObject.CompareTag(tile.gameObject.tag))
                {
                    AddToListAndMatch(t);
                }
            }
        }

        tile.isMatched = true;
        tile.dotToSwipeWith.isMatched = true;

    }
    

    
    private void CheckNeighbours(Tile tile1, Tile tile2, Tile currentTile)
    {
        if (currentTile.CompareTag(tile1.gameObject.tag)&& currentTile.CompareTag(tile2.gameObject.tag))
        {
            //ColorBomb(tile1, tile2,currentTile);
            RCBomb(tile1, tile2, currentTile,true);
            RCBomb(tile1, tile2, currentTile, false);
            AddToListAndMatch(tile1);
            AddToListAndMatch(tile2);
            AddToListAndMatch(currentTile);
            
        }
    }

   
    private void AddToListAndMatch(Tile tile)
    {
        if (!currentmatches.Contains(tile))
        {
            currentmatches.Add(tile);
            tile.isMatched = true;
        }
        
    }

    private void CheckBombs()
    {
        //Debug.Log("checked boms");
        /*if (currentmatches.Count == 4 || currentmatches.Count == 7)
        {
            //Debug.Log("in if loop");
            MakeRCBombs();
        }

        if (currentmatches.Count == 5 || currentmatches.Count == 8)
        {

            MakeColorBombs();
        }*/


        if (currentmatches.Count > 3)
        {
            var matchedTiles = new Dictionary<string, int>();
            foreach (Tile t in currentmatches)
            {
                string color = "";
                int numberOfTiles = 0;
                foreach (Tile t2 in currentmatches)
                {
                    color = t.gameObject.tag;
                    if (t.gameObject.CompareTag(t2.gameObject.tag))
                    {
                        ++numberOfTiles;
                    }
                }

                if (numberOfTiles > 3 && !matchedTiles.ContainsKey(color))
                {
                    matchedTiles.Add(color, numberOfTiles);
                }

            }

            foreach (KeyValuePair<string, int> entry in matchedTiles)
            {
                if (entry.Value == 4 || entry.Value == 7)
                {
                    MakeRCBombs();
                }
                else if ((entry.Value == 5 || entry.Value == 8) )
                {
                    MakeColorBombs();
                }
            }
        }


    }

    private void MakeColorBombs()
    {
        if(board.currentTileSwapped!=null && board.currentTileSwapped.isMatched)
        {
            board.currentTileSwapped.isMatched = false;
            board.currentTileSwapped.MakeColorBomb();
        }else if(board.currentTileSwapped.dotToSwipeWith != null && board.currentTileSwapped.dotToSwipeWith.isMatched)
        {
            board.currentTileSwapped.dotToSwipeWith.isMatched = false;
            board.currentTileSwapped.dotToSwipeWith.MakeColorBomb();
        }
    }
    private void MakeRCBombs()
    {
        if (board.currentTileSwapped != null && board.currentTileSwapped.isMatched)
        {
            board.currentTileSwapped.isMatched = false;

            board.currentTileSwapped.MakeRCBomb();
        }
        else if(board.currentTileSwapped.dotToSwipeWith!=null && board.currentTileSwapped.dotToSwipeWith.isMatched)
        {
            board.currentTileSwapped.dotToSwipeWith.isMatched = false;
            board.currentTileSwapped.dotToSwipeWith.MakeRCBomb();
        }
    }

    
    /// <summary>
    /// Function to check if a tile will make a match if placed at a specific position on the board.
    /// Helps to ensure that no (as few as possible) matches drop down on the board.
    /// </summary>
    /// <param name="column"></param>
    /// <param name="row"></param>
    /// <param name="currentTile"></param>
    /// <returns></returns>
    /*public bool MatchesAtPosition(int column,int row,Tile currentTile)
    {
        if (column > 1 && row > 1)
        {
            if(currentTile.gameObject.CompareTag(board.gameTiles[column-1,row].gameObject.tag) && currentTile.gameObject.CompareTag(board.gameTiles[column - 2, row].gameObject.tag))
            {
                Debug.Log(board.gameTiles[column - 1, row].gameObject.tag);
                Debug.Log(board.gameTiles[column - 2, row].gameObject.tag);
                Debug.Log(currentTile.gameObject.tag);
                return true;
            }

            if (currentTile.gameObject.CompareTag(board.gameTiles[column , row-1].gameObject.tag) && currentTile.gameObject.CompareTag(board.gameTiles[column , row-2].gameObject.tag))
            {
                Debug.Log(board.gameTiles[column - 1, row].gameObject.tag);
                Debug.Log(board.gameTiles[column - 2, row].gameObject.tag);
                Debug.Log(currentTile.gameObject.tag);
                return true;
            }
        }else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (currentTile.gameObject.CompareTag(board.gameTiles[column, row - 1].gameObject.tag) && currentTile.CompareTag(board.gameTiles[column, row - 2].gameObject.tag))
                {
                    return true;
                }
            }

            if (column > 1)
            {
                if (currentTile.gameObject.CompareTag(board.gameTiles[column - 1, row].gameObject.tag) && currentTile.CompareTag(board.gameTiles[column - 2, row ].gameObject.tag))
                {
                    return true;
                }
            }
        }

        return false;
    }*/

    public bool MatchesAtPosition(int column, int row, string currentTile)
    {
        if (column > 1 && row > 1)
        {
            if (board.gameTiles[column - 1, row].gameObject.CompareTag(currentTile) && board.gameTiles[column - 2, row].gameObject.CompareTag(currentTile))
            {
              
                return true;
            }

            if (board.gameTiles[column, row - 1].gameObject.CompareTag(currentTile) && board.gameTiles[column, row - 2].gameObject.CompareTag(currentTile))
            {
                return true;
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (board.gameTiles[column, row - 1].gameObject.CompareTag(currentTile) && board.gameTiles[column, row - 2].gameObject.CompareTag(currentTile))
                {
                    return true;
                }
            }

            if (column > 1)
            {
                if (board.gameTiles[column - 1, row].gameObject.CompareTag(currentTile) && board.gameTiles[column - 2, row].gameObject.CompareTag(currentTile))
                {
                    return true;
                }
            }
        }

        return false;
    }

}
