using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintsManager : MonoBehaviour
{

    [SerializeField] private Tile greenTile1;
    [SerializeField] private Tile blueTile1;
    [SerializeField] private Tile redTile2;
    [SerializeField] private Text instructionText;
    [SerializeField] private GameObject scorePanel, movesPanel,nextButton;

    [SerializeField] private Tile TGreenHint1;
    [SerializeField] private Tile TBlueHint1;
    [SerializeField] private Tile TYellowHint2;
    [SerializeField] private Tile TBlueHint2;

    public bool movedHintTile = false;
    public bool stillHinting = false;

    private GameManager gameManager;
    private bool nextTapped;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        stillHinting = true;
        if (gameManager.level2)
        {
            StartCoroutine(Hinting2());
        }
        else
        {
            StartCoroutine(Hinting());
        }
        
        nextButton.SetActive(false);
    }

    // Update is called once per frame
    /*void Update()
    {
        
    }*/

    private IEnumerator Hinting2()
    {
        ShowHint(TBlueHint1, TGreenHint1);
        instructionText.text = "Drag tiles to swap them.";
        yield return new WaitUntil(() => movedHintTile);
        ResetForNewHint(TBlueHint1, TGreenHint1);
        yield return new WaitForSeconds(0.5f);
        instructionText.text = "Now swap the two tiles to make a COMBO match.";
        ShowHint(TBlueHint2, TYellowHint2);
        yield return new WaitUntil(() => movedHintTile);
        ResetForNewHint(TBlueHint2, TYellowHint2);
        nextButton.SetActive(true);
        ShowPoints();
        yield return new WaitUntil(() => nextTapped);
        instructionText.text = "Create COMBOS to beat the level. Good Luck!";
        FinishHinting(TBlueHint2, TYellowHint2);
    }
    
    private IEnumerator Hinting()
    {
        ShowHint(greenTile1, blueTile1);
        instructionText.text = "Drag tiles to swap them. This is a No-Obligation Match move, it does not lead to a match.";
        yield return new WaitUntil(()=>movedHintTile);
        ResetForNewHint(greenTile1, blueTile1);
        yield return new WaitForSeconds(0.5f);
        instructionText.text = "Now swap the two tiles to make a match. No-Obligation Match moves can help you create matches!";
        ShowHint(greenTile1, redTile2);
        yield return new WaitUntil(() => movedHintTile);
        //instructionText.text = "Excellent! Keep creating matches to beat the level!";
        ResetForNewHint(greenTile1, redTile2);
        nextButton.SetActive(true);
        ShowPoints();
        yield return new WaitUntil(()=>nextTapped);
        nextTapped = false;
        ShowMoves();
        yield return new WaitUntil(() => nextTapped);
        instructionText.text = "Create matches to beat the level. Good Luck!";
        FinishHinting(greenTile1, redTile2);

    }

    
    private void ShowPoints()
    {
        scorePanel.SetActive(true);
        if (gameManager.level2)
        {
            instructionText.text = "Well Done, matching 4/5 tiles creates a special combo! Create Combo Matches to beat this level!";
        }
        else
        {
            instructionText.text = "Excellent! Keep creating matches to collect points and beat the level!";
        }
        
    }

    private void ShowMoves()
    {
        
        movesPanel.SetActive(true);
        instructionText.text = "You can see the moves you have remaining here." ;
    }

    public void TapNext()
    {
        scorePanel.SetActive(false);
        movesPanel.SetActive(false);
        nextTapped = true;
    }

    private void FinishHinting(Tile tile1, Tile tile2)
    {
        movedHintTile = false;
        gameManager.canMove = true;
        stillHinting = false;
        nextTapped = false;

        tile1.inHint = false;
        tile2.inHint = false;

        //tile1.anim.enabled = false;
        //tile2.anim.enabled = false;

        nextButton.SetActive(false);
    }

    private void ResetForNewHint(Tile tile1, Tile tile2)
    {

        movedHintTile = false;
        gameManager.canMove = false;
        
        tile1.inHint = false;
        tile2.inHint = false;

        tile1.anim.enabled = false;
        tile2.anim.enabled = false;

        if (!gameManager.level2)
        {
            tile1.playSecondAnim = true;
        } 
    }

    private void ShowHint(Tile tile1, Tile tile2)
    {
        tile1.inHint = true;
        tile2.inHint = true;
    }
}
