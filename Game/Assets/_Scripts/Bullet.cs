using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f;
    public int damage = 10;
    public GameObject explosion;
    private ParticleSystem boltPartSys;
    private SphereCollider boltCollider;
    private float timeLeft = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        var boltPart = transform.GetChild(0);
        boltPartSys = boltPart.GetComponent<ParticleSystem>();
        boltPartSys.startSpeed = speed;

        var boltColl = transform.GetChild(1);
        boltCollider = boltColl.GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if ( timeLeft < 0 )
        {
            boltCollider.enabled = true;
        }

        transform.position = transform.position + transform.forward * speed * Time.deltaTime; //new Vector3(0, -speed * Time.deltaTime, 0);

        if (transform.position.y < -20.2)
        {
            var b = GameObject.FindGameObjectWithTag("Firebolt");
            Destroy(b);
        }
        
    }
}
