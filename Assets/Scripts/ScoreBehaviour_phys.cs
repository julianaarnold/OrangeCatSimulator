using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBehaviour : MonoBehaviour
{
    public ScoreManager scoreManager;
    private bool fallen = false;
    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void OnCollisionEnter(Collision collision) {
        if (fallen == false) {
            if (collision.gameObject.tag == "Floor") {
                scoreManager.addToScore(40);
                if (gameObject.tag == "breakable") {
                    scoreManager.addToScore(20);
                }
                fallen = true;
            }
            // if (collision.gameObject.tag == "Counter") {
            //     scoreManager.addToScore(20);
            //     fallen = true;
            // }
        }
    }
}
