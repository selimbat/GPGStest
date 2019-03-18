using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

public class GoogleServices : MonoBehaviour {

    public static GoogleServices instance;
    private string SAVE_NAME = "save01";

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        SingIn();

    }

    private void SingIn()
    {
        if (!IsConnectedToGoogleServices)
        {
            Social.localUser.Authenticate((bool success) => { });
        }
    }

    public void AddScoreToLeaderboard(long score, string leaderboardId)
    {
        Social.ReportScore(score, leaderboardId, (bool success) => {  });
    }

    public void ShowLeaderboardUI()
    {
        Social.ShowLeaderboardUI();
    }
    
    //making a string out of game data (highscores...)
    string GameDataToString()
    {
        string result = "";
        foreach (int score in Player.Highscores)
        {
            result += score.ToString() + ",";
        }
        return result;
    }

    //parsing string to game data (stored in Highscores)
    void StringToGameData(string cloudData)
    {
        //if it's the first time that game has been launched after installing it and successfuly logging into Google Play Games
        if (PlayerPrefs.GetInt("IsFirstTime") == 1)
        {
            PlayerPrefs.SetInt("IsFirstTime", 0);
        }
        //if it's not the first time, start comparing
        else
        {
            isCloudDataLoaded = true;
            List<String> listOfScores = cloudData.Split(",");
            foreach(string scoreStr in listOfScores)
            {
                Player.Highscores.Add(int.Parse(scoreStr));
            }
            Player.Highscores = Player.Highscores.GetRange(0, 10);
            SaveData();
        }
    }

    //used for loading data from the cloud
    public void LoadData()
    {
        if (Social.localUser.authenticated)
        {
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithManualConflictResolution(SAVE_NAME,
                DataSource.ReadCacheOrNetwork, true);
        }
    }

    //used for saving data to the cloud
    public void SaveData()
    {
        if (Social.localUser.authenticated)
        {
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithManualConflictResolution(SAVE_NAME,
                DataSource.ReadCacheOrNetwork, true);
        }
    }
}
