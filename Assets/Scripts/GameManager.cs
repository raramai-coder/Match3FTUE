using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int moves = 10;
    public bool canMove = true;
    public int score = 0;
    public bool level2 = false;

    private int goalScore = 50;

    [SerializeField]  private Slider progressSlider;
    [SerializeField] private Text movesText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text requiredScoreText;
    [SerializeField] private GameObject scorePanel;
    [SerializeField] private GameObject resultsPanel;
    [SerializeField] private Text resultsText;

    // Start is called before the first frame update
    void Start()
    {
        scorePanel.SetActive(false);
        resultsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        movesText.text = moves.ToString();
        scoreText.text = score.ToString();
        requiredScoreText.text ="/"+ goalScore;
        progressSlider.value = (float)score / goalScore;

        /*if (canMove)
        {
            CheckGameState();
        }*/
        
    }

    public void CheckGameState()
    {
        if (moves <= 0)
        {
            if (score >= goalScore)
            {
                //Debug.Log("Success");
                WinState();
            }
            else
            {
                //Debug.Log("Fail");
                FailState();
            }
            return;
        }

        if (score >= goalScore)
        {
            ///Debug.Log("Success");
            WinState();
            return;
        }
    }

    private void FailState()
    {
        canMove = false;
        scorePanel.SetActive(true);
        resultsPanel.SetActive(true);
        resultsText.text = "You LOst :(";
    }

    private void WinState()
    {
        canMove = false;
        scorePanel.SetActive(true);
        resultsPanel.SetActive(true);
        resultsText.text = "You're a Winner ! <3";
    }
}
