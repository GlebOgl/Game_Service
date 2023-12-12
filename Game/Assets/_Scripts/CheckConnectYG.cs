using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using TMPro;

public class CheckConnectYG : MonoBehaviour
{
    private void OnEnable() => YandexGame.GetDataEvent += CheckSDK;
    private void OnDisable() => YandexGame.GetDataEvent -= CheckSDK;
    private TextMeshProUGUI scoreBest;
    // Start is called before the first frame update
    void Start()
    {
        if (YandexGame.SDKEnabled){
            CheckSDK();
        }
    }

    public void CheckSDK(){
        if (YandexGame.auth){
            Debug.Log("User authorization ok");
        }
        else{
            Debug.Log("User not authorization");
            YandexGame.AuthDialog();
        }
        GameObject scoreBO = GameObject.Find("BestScore");
        scoreBest = scoreBO.GetComponent<TextMeshProUGUI>();
        scoreBest.text = "Best Score: " + YandexGame.savesData.bestScore.ToString();
    }
}
