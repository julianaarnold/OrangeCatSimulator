using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBehaviour_destruct : MonoBehaviour
{
    public ScoreManager scoreManager;
    private bool destroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    public void destructScore(int scoreAddition) {
        if (destroyed == false) {
            scoreManager.addToScore(scoreAddition);
            destroyed = true;
        }
    }
}
