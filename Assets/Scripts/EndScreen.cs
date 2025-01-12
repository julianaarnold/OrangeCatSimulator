using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public GameObject endScreen;
    public GameObject ingameUI;
    // public TMP_Text ScoreText;
    private bool gameEnded;
    public Button restartGameBtn;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = restartGameBtn.GetComponent<Button>();
        btn.onClick.AddListener(restartGame);
        ingameUI.SetActive(true);
        endScreen.SetActive(false);
        gameEnded = false;
    }

    public void showScreen() {
        ingameUI.SetActive(false);
        endScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        gameEnded = true;
    }

    public void restartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
