using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMatches : MonoBehaviour
{
    private Board board;
    private FindMatches findMatches;
    private GameManager gameManager;

    [SerializeField] private AudioClip singlePop;
    [SerializeField] private AudioClip multiplePops;

    private static AudioSource audioData;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        gameManager = FindObjectOfType<GameManager>();

        audioData = GetComponent<AudioSource>();
    }

    public void DestroyAllMatches()
    {
       
        foreach(Tile currentTile in board.gameTiles)
        {
            if (currentTile != null)
            {
                if (currentTile.isMatched)
                {
                    //particle effect for destroying tile
                    ParticleEffect(currentTile);

                    //play sound for effect
                    PlaySound();
                    
                    //destroy tile and remove from arrays
                    board.gameTiles[currentTile.column, currentTile.row] = null;
                    //findMatches.currentmatches.Remove(currentTile);
                    Destroy(currentTile.gameObject, 0.6f);

                    /* if (currentTile.gameObject.CompareTag("Green"))
                     {
                         gameManager.score += 1;
                     }*/

                    //gameManager.score += 1;
                    //++gameManager.score;
                }
            }
           
        }
        /*//gameManager.score += findMatches.currentmatches.Count;
        findMatches.currentmatches.Clear();*/
        //findMatches.currentmatches.Clear();
        StartCoroutine(DecreaseRow());
    }

    IEnumerator DecreaseRow()
    {
        yield return new WaitForSeconds(0.6f);

        int emptyRowsBelow = 0;

        for (int i = 0; i < board.width; i++)
        {
            for(int j = 0; j < board.height; j++)
            {
                if (board.gameTiles[i, j] == null)
                {
                    ++emptyRowsBelow;
                }else if (emptyRowsBelow > 0)
                {
                    board.gameTiles[i, j].row -= emptyRowsBelow;
                    board.gameTiles[i, j] = null;
                }
            }
            emptyRowsBelow = 0;
        }

        board.CallFillBoard();
    }

    private void ParticleEffect(Tile currentTile)
    {
        currentTile.particleEffect.SetActive(true);
        SpriteRenderer mySprite = currentTile.gameObject.GetComponent<SpriteRenderer>();
        mySprite.color = new Color(0f, 0f, 0f, 0.6f);
    }

    private void PlaySound()
    {
        if (findMatches.currentmatches.Count > 3)
        {
            audioData.clip = multiplePops;
            audioData.time = 4f;
        }
        else
        {
            audioData.clip = singlePop;
            audioData.time = 0.575f;
        }
        
        audioData.Play();
    }

    
}
