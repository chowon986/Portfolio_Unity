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
    public Canvas clearCanvas;
    public Canvas starCanvas;
    private SoundManager soundManager;

    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        length = board.scoreGoals.Length;
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
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

        if (score > board.scoreGoals[length - 1])
        {
            foreach (Transform transform in clearCanvas.transform)
            {
                transform.gameObject.SetActive(true);
            }

            StartCoroutine(OpenStarCo());
        }
    }

    private IEnumerator OpenStarCo()
    {
        yield return new WaitForSeconds(1.0f);

        foreach(Transform transform in starCanvas.transform)
        {
            if(soundManager != null)
            {
                soundManager.PlayRandomDestroyNoise();
            }
            transform.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.4f);
        }
    }
}
