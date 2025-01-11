using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ScoreBehaviour : MonoBehaviour
{
    public ScoreManager scoreManager;
    private bool fallen = false;
    public AudioSource metal_pipe;
    // Start is called before the first frame update
    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        metal_pipe = gameObject.AddComponent<AudioSource>();
        metal_pipe.clip = Resources.Load<AudioClip>("metal-pipe");
    }

    void OnCollisionEnter(Collision collision) {
        if (fallen == false) {
            if (collision.gameObject.tag == "Floor") {
                scoreManager.addToScore(40);
                if (gameObject.tag == "breakable") {
                    scoreManager.addToScore(20);
                }
                metal_pipe.Play();
                fallen = true;
            }
            // if (collision.gameObject.tag == "Counter") {
            //     scoreManager.addToScore(20);
            //     fallen = true;
            // }
        }
    }
}
