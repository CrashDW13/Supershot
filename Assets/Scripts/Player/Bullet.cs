using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject bulletExplosionPrefab;

    [SerializeField]
    private float speed = 3.0f;
    private float lifetime = 20.0f; 

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        lifetime -= 0.1f;
        if (lifetime <= 0)
        {
            Destroy(gameObject); 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        speed = 0;
        GameObject explosion = Instantiate(bulletExplosionPrefab);
        explosion.transform.position = transform.position; 
        Destroy(gameObject); 
    }
}
