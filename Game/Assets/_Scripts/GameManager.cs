using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void OnDragonKilled(EnemyDragon killed)
    {
        if (killed.isMainDragon)
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1) % 6);
    }
}
