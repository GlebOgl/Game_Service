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

    void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        
    }

    void OnEnable() {
        YandexGame.GetDataEvent += GetLoadSave;
        SceneManager.activeSceneChanged += OnNewSceneLoaded;
    }

    void OnDisable() {
        YandexGame.GetDataEvent -= GetLoadSave;
        SceneManager.activeSceneChanged -= OnNewSceneLoaded;
    }

    private void OnNewSceneLoaded(Scene oldScene, Scene newScene)
    {
        if (YandexGame.SDKEnabled)
        {
            GetLoadSave();
        }
    }

    public void OnDragonKilled(EnemyDragon killed)
    {
        if (killed.isMainDragon)
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % 6);
    }

    public void GetLoadSave()
    {
        Debug.Log(YandexGame.savesData.score);
        GameObject playerNamePrefabGUI = GameObject.Find("PlayerName");
        var playerName = playerNamePrefabGUI.GetComponent<TextMeshProUGUI>();
        playerName.text = YandexGame.playerName;
    }

    public void UserSave(int currentScore, int currentBestScore, string[] currentAchiv)
    {
        YandexGame.savesData.score = currentScore;
        if (currentScore > currentBestScore)
        {
            YandexGame.savesData.bestScore = currentScore;
        }
        YandexGame.savesData.achivment = currentAchiv;
        YandexGame.SaveProgress();
    }

    public void SaveAfterDeath()
    {
        GameObject scoreGO = GameObject.Find("Score");
        var scoreGT = scoreGO.GetComponent<TextMeshProUGUI>();

        string[] achivList;
        achivList = YandexGame.savesData.achivment;
        achivList[0] = "Береги щиты!!!";

        UserSave(int.Parse(scoreGT.text), YandexGame.savesData.bestScore, achivList);


        YandexGame.NewLeaderboardScores("TOPPlayerScore", int.Parse(scoreGT.text));

        YandexGame.RewVideoShow(0);

        SceneManager.LoadScene("_0Scene");
        GetLoadSave();
    }
}
