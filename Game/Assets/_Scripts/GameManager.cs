using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int score {
        get => currentScore;
        set {
            currentScore = value;
            UpdateScore();
        }
    }

    private List<string> achievements = new List<string>();
    private int currentScore;

    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (YandexGame.SDKEnabled)
            LoadSave();
    }

    void Update()
    {
        
    }

    void OnEnable() {
        YandexGame.GetDataEvent += LoadSave;
        SceneManager.activeSceneChanged += OnNewSceneLoaded;
    }

    void OnDisable() {
        YandexGame.GetDataEvent -= LoadSave;
        SceneManager.activeSceneChanged -= OnNewSceneLoaded;
    }

    private void OnNewSceneLoaded(Scene oldScene, Scene newScene)
    {
        UpdateScore();
    }

    public void OnDragonKilled(EnemyDragon killed)
    {
        if (!killed.isMainDragon)
            return;

        score += killed.reward;

        var nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        
        if (nextScene >= SceneManager.sceneCountInBuildSettings)
            GameEnded();
        else
            SceneManager.LoadScene(nextScene);
    }

    public void LoadSave()
    {
        var playerName = GameObject.Find("PlayerName")?.GetComponent<TextMeshProUGUI>();
        if (playerName)
            playerName.text = YandexGame.playerName;

        achievements.Clear();
        achievements.AddRange(YandexGame.savesData.achievement ?? new string[0]);
    }

    public void StoreSave()
    {
        YandexGame.savesData.lastScore = currentScore;
        if (currentScore > YandexGame.savesData.bestScore)
            YandexGame.savesData.bestScore = currentScore;
        
        Debug.Log($"StoreSave: currentScore={currentScore}, lastScore={YandexGame.savesData.lastScore}, bestScore={YandexGame.savesData.bestScore}");

        YandexGame.savesData.achievement = achievements.ToArray();
        YandexGame.SaveProgress();
    }

    public void PlayerDied()
    {
        achievements.Add("Береги щиты!!!");

        YandexGame.RewVideoShow(0);
        
        GameEnded();
    }

    public void GameEnded()
    {
        StoreSave();

        YandexGame.NewLeaderboardScores("TOPPlayerScore", currentScore);
        SceneManager.LoadScene("_0Scene");

        LoadSave();

        score = 0;
    }

    private void UpdateScore()
    {
        var scoreDisplay = GameObject.Find("Score")?.GetComponent<TextMeshProUGUI>();
        if (scoreDisplay)
            scoreDisplay.text = "Очки: " + currentScore;
    }
}
