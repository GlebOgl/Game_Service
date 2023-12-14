using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using TMPro;
using System;

public class CheckConnectYG : MonoBehaviour
{
    private void OnEnable() => YandexGame.GetDataEvent += CheckSDK;
    private void OnDisable() => YandexGame.GetDataEvent -= CheckSDK;
    private TextMeshProUGUI scoreBest;
    [SerializeField] private GameObject achiv;
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

        YandexGame.RewVideoShow(0);

        GameObject scoreBO = GameObject.Find("BestScore");
        scoreBest = scoreBO.GetComponent<TextMeshProUGUI>();
        scoreBest.text = "Best Score: " + YandexGame.savesData.bestScore.ToString();
        if (YandexGame.savesData.achivment[0] == null && achiv){

        }
        else{
            
            Debug.Log(achiv);
            TextMeshProUGUI achivment = achiv.GetComponent<TextMeshProUGUI>();
            achivment.text = "";
            foreach (string value in YandexGame.savesData.achivment){
                achivment.text += value +"\n"; 
            }
        }
    }
}
