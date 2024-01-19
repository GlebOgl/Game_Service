using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private HashSet<string> achievements = new HashSet<string>();
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
            StartCoroutine(LoadSceneWithDelay(nextScene, 2f));
    }

    public void LoadSave()
    {
        var playerName = GameObject.Find("PlayerName")?.GetComponent<TextMeshProUGUI>();
        if (playerName)
            playerName.text = YandexGame.playerName;

        achievements.Clear();
        if (YandexGame.savesData.achievement != null)
            foreach (var s in YandexGame.savesData.achievement)
                achievements.Add(s);
    }

    public void StoreSave()
    {
        if (!YandexGame.SDKEnabled || (YandexGame.savesData == null))
            return;

        YandexGame.savesData.achievement = achievements.ToArray();
        YandexGame.SaveProgress();
    }

    public void PlayerDied()
    {
        achievements.Add("Береги щиты!!!");

        //YandexGame.RewVideoShow(0);
        
        GameEnded();
    }

    public void GameEnded()
    {
        if (YandexGame.SDKEnabled && (YandexGame.savesData != null)) {
            YandexGame.savesData.lastScore = currentScore;
            if (currentScore > YandexGame.savesData.bestScore)
                YandexGame.savesData.bestScore = currentScore;

            YandexGame.NewLeaderboardScores("TOPPlayerScore", currentScore);
        }

        Debug.Log($"GameEnded: currentScore={currentScore}, lastScore={YandexGame.savesData.lastScore}, bestScore={YandexGame.savesData.bestScore}");

        YandexGame.savesData.coins += Mathf.CeilToInt(currentScore / 10f);

        StoreSave();
        
        StartCoroutine(LoadSceneWithDelay("_0Scene", 2f));

        LoadSave();

        score = 0;
    }

    private void UpdateScore()
    {
        var scoreDisplay = GameObject.Find("Score")?.GetComponent<TextMeshProUGUI>();
        if (scoreDisplay)
            scoreDisplay.text = "Очки: " + currentScore;
    }

    public IEnumerator LoadSceneWithDelay(string name, float delay)
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(name);
        Time.timeScale = 1f;
    }

    public IEnumerator LoadSceneWithDelay(int index, float delay)
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(delay);
        SceneManager.LoadScene(index);
        Time.timeScale = 1f;
    }
}
