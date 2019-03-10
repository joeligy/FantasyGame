using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float stoppingDistanceFromPlayer = 5f;

    NavMeshAgent nav;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        nav.destination = player.transform.position;
    }
}
