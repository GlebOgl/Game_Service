using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float lifetime = 5f;
    
    public float speed = 1f;
    public Vector3 movementDirection = Vector3.up;

    public int damage = 10;

    public ParticleSystem explosion;

    void Start()
    {
        movementDirection.Normalize();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Aaaaa", other);
        var dragon = other.GetComponentInParent<EnemyDragon>();
        if (!dragon)
            return;

        dragon.health -= damage;
        
        Destroy(gameObject, 2f);
        var particles = GetComponentsInChildren<ParticleSystem>();
        foreach (var sys in particles)
            sys.Stop();

        explosion.Play();
    }

    void FixedUpdate()
    {
        transform.position += movementDirection * speed * Time.fixedDeltaTime;
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
            Destroy(gameObject);
    }
}
