using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{

    [SerializeField] float damageCaused = 10f;
    [SerializeField] float maxDistance = 100f;
    [SerializeField] GameObject fireBallExplosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>()) return;

        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy && enemy.isAlive)
        {
            enemy.TakeDamage(damageCaused);
        }
        Explode();
    }

    void Explode()
    {
        Instantiate(fireBallExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject, .25f);
    }
}
