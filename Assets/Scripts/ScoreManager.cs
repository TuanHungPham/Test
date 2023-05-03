using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Facebook.Unity;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance { get => instance; set => instance = value; }

    public int score;
    public int bestScore;
    public TMP_Text scoreText;


    private void Awake()
    {
        instance = this;

        scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
    }

    private void Reset()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
    }

    public void AddScore()
    {
        score++;
    }

    private void Update()
    {
        ShowScore();
    }

    private void ShowScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }
}