using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private Vector2 intendedPosition;
    private float swipeResistance = 1f;
    private float swipeAngle = 0f;
    private float targetX;
    private float targetY;
    private float xOffset = 0.6f;
    private float yOffset = 0.3f;

    public int column;
    public int row;
    public bool isMatched;
    public bool isColumbBomb = false;
    public bool isRowBomb = false;
    public bool isColorBomb = false;
    public bool matchRC = false;
    public bool inHint = false;
    public Animator anim;
    public bool playSecondAnim = false;

    private Board board;
    private FindMatches findMatches;
    public Tile dotToSwipeWith;
    private GameManager gameManager;
    private HintsManager hintManager;
    

    [SerializeField] private bool placedAtStart = false;
    [SerializeField] public GameObject particleEffect;
    [SerializeField] private GameObject rowArrow;
    [SerializeField] private GameObject columnArrow;
    [SerializeField] private GameObject colorBomb;
    [SerializeField] private string animationTitle;
    [SerializeField] private string secondAnimationTitle;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        findMatches = FindObjectOfType<FindMatches>();
        gameManager = FindObjectOfType<GameManager>();
        hintManager = FindObjectOfType<HintsManager>();

        /*if (inHint)
        {
            anim = GetComponent<Animator>();
        }*/
        
        
        if (placedAtStart) //add to array in board if placed at start of game by developer and not by game
        {
            board.gameTiles[column, row] = this;
        }

        isMatched = false;

        if (gameManager.level2)
        {
            xOffset = 0.75f;
            //yOffset = 0.3f;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        MoveTile();

        if (inHint)
        {
            anim = GetComponent<Animator>();
            anim.enabled = true;

            if(this.gameObject.name == "green-hint1" &&playSecondAnim)
            {
                anim.Play(secondAnimationTitle);
            }
            else
            {
                anim.Play(animationTitle);
            }
            
        }
        
        
        
    }

    public void MakeRCBomb()
    {
        //Debug.Log(swipeAngle);
        if ((swipeAngle >= -45 && swipeAngle <= 45) || (swipeAngle <= -135 || swipeAngle >= 135)) 
        {
            MakeRowBomb();
        }
        else
        {
            MakeColumnBomb();
        }
    }

    private void MakeRowBomb()
    {
        isRowBomb = true;
        rowArrow.SetActive(true);
    }

    private void MakeColumnBomb()
    {
        isColumbBomb = true;
        columnArrow.SetActive(true);
    }

    public void MakeColorBomb()
    {
        isColorBomb = true;
        colorBomb.SetActive(true);
    }

    /// <summary>
    /// Function that physically moves tile on the game board
    /// </summary>
    private void MoveTile()
    {
        
        
        targetX = column + (column * xOffset);
        targetY = row + (row * yOffset);

        if (Mathf.Abs(targetX - transform.position.x) > 0.1)
        {
            intendedPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, intendedPosition, 0.6f);
            findMatches.CallFindMatches();
        }
        else
        {
            intendedPosition = new Vector2(targetX, transform.position.y);
            transform.position = intendedPosition;
        }

        if (Mathf.Abs(targetY - transform.position.y) > 0.1)
        {
            intendedPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, intendedPosition, 0.6f);
            findMatches.CallFindMatches();
        }
        else
        {
            intendedPosition = new Vector2(transform.position.x, targetY);
            transform.position = intendedPosition;
        }

        if (board.gameTiles[column, row] != this)
        {
            board.gameTiles[column, row] = this;
        }

        
        //findMatches.CallFindMatches();
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
    }

    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (gameManager.canMove || inHint)
        {
            gameManager.canMove = false;

            if (inHint)
            {
                hintManager.movedHintTile = true;
               
            }
            CalculateSwipeAngle();

        }

    }

    private void CalculateSwipeAngle()
    {
        if(Mathf.Abs(finalTouchPosition.y-firstTouchPosition.y)>swipeResistance || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResistance)
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            board.currentTileSwapped = this;
            SetTilePosition();
            
            /*if (!hintManager.stillHinting)
            {
                gameManager.moves -= 1;
            }*/
            
        }
        else
        {
            gameManager.canMove = true;
        }
    }

    private void SetTilePosition()
    {
        if(swipeAngle>-45 && swipeAngle<=45 && column < board.width - 1)
        {
            //right swipe
            dotToSwipeWith = board.gameTiles[column + 1, row];
            dotToSwipeWith.column -= 1;
            column += 1;
        }else if (swipeAngle > 45 && swipeAngle <= 135 && row< board.height - 1)
        {
            //up swipe
            dotToSwipeWith = board.gameTiles[column , row+1];
            dotToSwipeWith.row-= 1;
            row+= 1;
        }else if ((swipeAngle > 135 || swipeAngle <= -135) && column >0)
        {
            //left swipe
            dotToSwipeWith = board.gameTiles[column - 1, row];
            dotToSwipeWith.column += 1;
            column -= 1;
        } else if (swipeAngle >= -135 && swipeAngle < -45 && row >0)
        {
            //down swipe
            dotToSwipeWith = board.gameTiles[column , row-1];
            dotToSwipeWith.row += 1;
            row -= 1;
        }
    }

 
    
}
