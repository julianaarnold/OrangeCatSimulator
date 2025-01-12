using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBehaviour_scratch : MonoBehaviour
{
    public ScoreManager scoreManager;
    private bool scratched;
    private int scratchCount;
    public int scratchLimit = 3;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        scratchCount = 0;
        scratched = false;
    }
    public void scratchCounter() {
        scratchCount += 1;
        if (scratchCount == scratchLimit) {
            scratchScore();
        }
    }

    private void scratchScore() {
        if (scratched == false) {
            scoreManager.addToScore(50);
            scratched = true;
            if (audioSource != null) {
                audioSource.Play();
            }
        }
    }
}
