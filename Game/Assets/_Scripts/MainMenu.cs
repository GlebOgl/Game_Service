using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI coinsText;

    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Application.Quit();
    }

    void Start()
    {
        UpgradeManager.Instance.DataUpdated += UpdateCoins;
        UpdateCoins();
    }

    void OnDestroy()
    {
        UpgradeManager.Instance.DataUpdated -= UpdateCoins;
    }

    private void UpdateCoins()
    {
        coinsText.text = "Монеты: " + UpgradeManager.Instance.Coins;
    }
}
