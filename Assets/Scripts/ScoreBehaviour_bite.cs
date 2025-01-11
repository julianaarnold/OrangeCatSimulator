using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBehaviour_bite : MonoBehaviour
{
    public ScoreManager scoreManager;
    private bool eaten = false;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    public void scratchScore() {
        if (eaten == false) {
            scoreManager.addToScore(50);
            eaten = true;
        }
    }
}
