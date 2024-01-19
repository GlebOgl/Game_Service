using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using TMPro;
using static UnityEngine.EventSystems.EventTrigger;

public class EnergyShield : MonoBehaviour
{
    public TextMeshProUGUI healthGT;
    public AudioSource audioSource;

    public int health = 100;

    public float energy = 0;
    public float energyInEgg = 1;
    public float energyPerShot = 2;

    public GameObject projectilePrefab;


    void Start() {
        healthGT = GameObject.Find("Health").GetComponent<TextMeshProUGUI>();

        health = UpgradeManager.Instance.PlayerHealth;
        energyPerShot = UpgradeManager.Instance.EnergyPerShot;
        energyInEgg = UpgradeManager.Instance.EnergyPerEgg;

        UpdateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        Vector3 pos = transform.position;
        if (mousePos3D.x > 22f || mousePos3D.x < -22f) return;
        pos.x = mousePos3D.x;
        //pos.y = mousePos3D.y;
        transform.position = pos;


        if (Input.GetMouseButtonDown(0))
            TryFire();
    }

    private void OnCollisionEnter(Collision coll) {
        GameObject Collided = coll.gameObject;
        if (Collided.tag == "Dragon Egg"){
            Destroy(Collided);
            GameManager.Instance.score += 1;
            energy += energyInEgg;

            audioSource = GetComponent<AudioSource>();
            audioSource.Play();
        } else if (Collided.tag == "Firebolt") {
            
            var firebolt = Collided.gameObject.transform.parent.GetComponent<Bullet>();
            
            health -= firebolt.damage;
            UpdateHealth();

            if (health <= 0)
            {
                GameManager.Instance.PlayerDied();
                Destroy(gameObject);
            }
        }
    }

    private void UpdateHealth()
    {
        healthGT.text = "Здровье - " + health;
    }

    private void TryFire()
    {
        if (energy < energyPerShot)
            return;

        energy -= energyPerShot;

        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }
}
