using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
    }

    public void addToScore(int addition) {
        score += addition;
        Debug.Log(score);
    }
}
