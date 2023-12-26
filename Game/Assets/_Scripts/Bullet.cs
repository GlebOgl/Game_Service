using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f;
    public int damage = 10;
    public GameObject explosion;
    private ParticleSystem boltPartSys;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        var boltPart = transform.GetChild(0);
        boltPartSys = boltPart.GetComponent<ParticleSystem>();
        boltPartSys.startSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = transform.position + new Vector3(0, -speed * Time.deltaTime, 0);

        if (transform.position.y < -20.2)
        {
            var b = GameObject.FindGameObjectWithTag("Firebolt");
            Destroy(b);
        }
        
    }
}
