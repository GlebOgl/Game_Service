using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using TMPro;

public class EnergyShield : MonoBehaviour
{
    public TextMeshProUGUI scoreGT;
    public TextMeshProUGUI healthGT;
    public AudioSource audioSource;

    public int health = 100;

    void Start() {
        GameObject scoreGO = GameObject.Find("Score");
        scoreGT = scoreGO.GetComponent<TextMeshProUGUI>();
        scoreGT.text = "0";
        GameObject healthGO = GameObject.Find("Health");
        healthGT = healthGO.GetComponent<TextMeshProUGUI>();
        healthGT.text = "Здровье - " + health;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 pos = this.transform.position;
        if (mousePos3D.x > 22f || mousePos3D.x < -22f) return;
        pos.x = mousePos3D.x;
        //pos.y = mousePos3D.y;
        this.transform.position = pos;
    }

    private void OnCollisionEnter(Collision coll) {
        GameObject Collided = coll.gameObject;
        if (Collided.tag == "Dragon Egg"){
            Destroy(Collided);
            int score = int.Parse(scoreGT.text);
            score += 1;
            scoreGT.text = score.ToString();

            audioSource = GetComponent<AudioSource>();
            audioSource.Play();
        }
        if (Collided.tag == "Firebolt"){
            
            Bullet firebolt = Collided.gameObject.transform.parent.GetComponent<Bullet>();
            //урон который нанесется игроку
            int damage =  firebolt.damage;
            health -= damage;

            healthGT.text = "Здровье - " + health;

            if (health <= 0)
                GameManager.Instance.SaveAfterDeath();
        }
    }
}
