using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Board board;
    public Text scoreText;
    public int score;
    public Image scoreBar;
    private int length;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        length = board.scoreGoals.Length;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "" + score;
    }

    public void IncreaseScore(int amountToIncrease)
    {
        score += amountToIncrease;
        if(board != null && scoreBar != null)
        {
            scoreBar.fillAmount = (float)score / (float)board.scoreGoals[length - 1];
        }

        if(score > board.scoreGoals[length - 1])
        {
            Debug.Log("GameOver");
        }
    }
}
