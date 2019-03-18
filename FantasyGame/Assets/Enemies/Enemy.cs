using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public bool isAlive = true;

    [SerializeField] float health = 100f;
    [SerializeField] float attackDamage = 30f;
    [SerializeField] float secondsBetweenAttacks = 5f;

    [SerializeField] float chaseRadius = 20f;
    [SerializeField] float attackRadius = 5f;

    NavMeshAgent nav;
    Player player;
    Animator animator;

    float lastAttackTime = 0f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

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

        ChasePlayerIfClose();
        AttackPlayerIfClose();
    }

    private void ChasePlayerIfClose()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < chaseRadius)
        {
            nav.isStopped = false;
            nav.destination = player.transform.position;
        }
        else
        {
            nav.isStopped = true;
        }
    }

    private void AttackPlayerIfClose()
    {
        if (PlayerInAttackRadius() && (Time.time - lastAttackTime) > secondsBetweenAttacks)
        {
            animator.SetTrigger("Attack3");
            lastAttackTime = Time.time;
            StartCoroutine(GremlinAttack());
        }
    }

    private IEnumerator GremlinAttack()
    {
        nav.isStopped = true;
        yield return new WaitForSeconds(1f);
        if (PlayerInAttackRadius())
        {
            player.ReduceHealth(attackDamage);
        }
        nav.isStopped = false;
    }

    private bool PlayerInAttackRadius()
    {
        return Vector3.Distance(player.transform.position, transform.position) < attackRadius;
    }

    void Die()
    {
        isAlive = false;
        nav.isStopped = true;
        animator.SetTrigger("Death");
        GetComponent<AudioSource>().Play();
    }
}
