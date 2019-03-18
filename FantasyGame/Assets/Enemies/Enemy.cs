using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public bool isAlive = true;

    [SerializeField] float health = 100f;

    NavMeshAgent nav;
    Player player;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;

        nav.destination = player.transform.position;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isAlive = false;
        nav.isStopped = true;
        animator.SetTrigger("Death");
    }
}
