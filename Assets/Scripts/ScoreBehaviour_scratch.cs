using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBehaviour_scratch : MonoBehaviour
{
    public ScoreManager scoreManager;
    private bool scratched = false;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    public void scratchScore() {
        if (scratched == false) {
            scoreManager.addToScore(50);
            scratched = true;
        }
    }
}
