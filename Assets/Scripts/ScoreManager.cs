using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public TMP_Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = "Score: " + score.ToString();
    }

    public void addToScore(int addition) {
        score += addition;
        // Debug.Log(score);
        scoreText.text = "Score: " + score.ToString();
    }
}
