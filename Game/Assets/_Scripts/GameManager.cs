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
    private int coins;

    void Start()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
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
        if (YandexGame.SDKEnabled)
        {
            LoadSave();
        }

        currentScore = 0;
        UpdateScore();
    }

    public void OnDragonKilled(EnemyDragon killed)
    {
        if (killed.isMainDragon)
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % 6);
    }

    public void LoadSave()
    {
        var playerNamePrefabGUI = GameObject.Find("PlayerName");
        var playerName = playerNamePrefabGUI.GetComponent<TextMeshProUGUI>();
        playerName.text = YandexGame.playerName;

        coins = YandexGame.savesData.coins;

        achievements.Clear();
        achievements.AddRange(YandexGame.savesData.achievement);
    }

    public void StoreSave()
    {
        YandexGame.savesData.lastScore = currentScore;
        if (currentScore > YandexGame.savesData.bestScore)
            YandexGame.savesData.bestScore = currentScore;
        
        YandexGame.savesData.coins = coins;
        YandexGame.savesData.achievement = achievements.ToArray();
        YandexGame.SaveProgress();
    }

    public void PlayerDied()
    {
        achievements.Add("Береги щиты!!!");

        StoreSave();
        YandexGame.NewLeaderboardScores("TOPPlayerScore", currentScore);
        YandexGame.RewVideoShow(0);

        SceneManager.LoadScene("_0Scene");
        LoadSave();
    }

    private void UpdateScore()
    {
        var scoreDisplay = GameObject.Find("Score")?.GetComponent<TextMeshProUGUI>();
        if (scoreDisplay)
            scoreDisplay.text = "Очки: " + currentScore;
    }
}
