using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDragon : MonoBehaviour
{
    public GameObject dragonEggPrefab;
    public GameObject fireboltPrefab;
    public float speed = 1f;
    public float timeBetweenShoots = 1f;
    public float timeBetweenEggDrops = 1f;
    public float leftRightDistance = 10f;
    public float chanceDirection = 0.1f;
    public int reward = 10;
    public int health = 100;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DropEgg", 5f);
        Invoke("Shoot", 2f);
    }

    void Shoot(){
        Vector3 myVector = new Vector3(0.0f, 5.0f, 0.25f);
        GameObject bolt = Instantiate<GameObject>(fireboltPrefab);
        bolt.transform.position = transform.position + myVector;
        Invoke("Shoot", timeBetweenShoots);
    }

    void DropEgg(){
        Vector3 myVector = new Vector3(0.0f, 5.0f, 0.0f);
        GameObject egg = Instantiate<GameObject>(dragonEggPrefab);
        egg.transform.position = transform.position + myVector;
        Invoke("DropEgg", timeBetweenEggDrops);
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0){
            SceneManager.LoadScene("_2Scene");
        }

        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        if (pos.x < -leftRightDistance){
            speed = Mathf.Abs(speed);
        }
        else if (pos.x > leftRightDistance){
            speed = -Mathf.Abs(speed);
        }

        
    }

    private void FixedUpdate() {
        if (Random.value < chanceDirection){
            speed *= -1;
        }
    }
}
