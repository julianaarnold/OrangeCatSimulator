using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBehaviour_bite : MonoBehaviour
{
    public ScoreManager scoreManager;
    private bool eaten;
    private int biteCount;
    public int biteLimit = 2;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        biteCount = 0;
        eaten = false;
    }

    public void biteCounter() {
        biteCount += 1;
        if (biteCount == biteLimit) {
            biteScore();
        }
    }

    private void biteScore() {
        if (eaten == false) {
            scoreManager.addToScore(20);
            eaten = true;
        }
    }
}
