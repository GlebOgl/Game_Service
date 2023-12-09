using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class CheckConnectYG : MonoBehaviour
{
    private void OnEnable() => YandexGame.GetDataEvent += CheckSDK;
    private void OnDisable() => YandexGame.GetDataEvent -= CheckSDK;
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
    }
}
