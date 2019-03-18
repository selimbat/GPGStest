using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    private int score = 0;
    private float gameDuration = 20;
    private float EndGameTime;
    private bool IsPlaying = true;
    public Camera mainCamera;
    public Text scoreText;
    public Text timerText;
    public static List<int> Highscores = new List<int>();

	void Start ()
    {
        scoreText.text = score.ToString("0");

        EndGameTime = Time.time + gameDuration;
        timerText.text = gameDuration.ToString("0");
    }

	// Update is called once per frame
	void Update () {

        timerText.text = Math.Max(0, EndGameTime - Time.time).ToString("0");

        if (Input.touchCount > 0 && IsPlaying){

            Touch playerTouch = Input.touches[0];

            if (playerTouch.phase == TouchPhase.Began){
                score++;
                scoreText.text = score.ToString("0");
            }

            if (Time.time > EndGameTime)
            {
                GoogleServices.instance.AddScoreToLeaderboard(score, GPGSIds.leaderboard_learderboard);
                IsPlaying = false;
            }

        }
    }
}
