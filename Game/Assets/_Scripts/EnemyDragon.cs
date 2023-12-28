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
    private GameObject Player;
    public int boltsPerShot = 1;
    public float spread = 20;
    public bool isMainDragon = true;
    public float dragonOffset = 0f;

    public bool autoAIM = false;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DropEgg", 5f);
        Invoke("Shoot", 2f);
    }

    void Shoot(){
        Vector3 myVector = new Vector3(0.0f, 5.0f, 0.25f);
        float shootAngle = 0;
        float angleChange = 0;
        if (boltsPerShot > 1){
            shootAngle = -spread / 2;
            angleChange = spread / (boltsPerShot - 1);
        }

        for (int i = 0; i < boltsPerShot; i++)
        {
            GameObject bolt = Instantiate<GameObject>(fireboltPrefab);
            bolt.transform.position = transform.position + myVector;

            if (autoAIM)
            {
                bolt.transform.LookAt(Player.transform.position);

            }
            bolt.transform.localEulerAngles += new Vector3(shootAngle, 0, 0);
            shootAngle += angleChange;
        }
        
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
        if (!Player)
            Player = GameObject.FindGameObjectWithTag("Player");
        if (health <= 0){
            if (isMainDragon)
                SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1)%6);
            else
                Destroy(this.gameObject);
        }

        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        if (pos.x < -(leftRightDistance + dragonOffset)){
            speed = Mathf.Abs(speed);
        }
        else if (pos.x > leftRightDistance - dragonOffset){
            speed = -Mathf.Abs(speed);
        }

        
    }

    private void FixedUpdate() {
        if (Random.value < chanceDirection){
            speed *= -1;
        }
    }
}
