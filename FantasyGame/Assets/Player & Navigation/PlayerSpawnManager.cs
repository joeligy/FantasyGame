using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnManager : MonoBehaviour
{

    public string spawnPoint;

    private void Awake()
    {
        int numPlayerSpawnManagers = FindObjectsOfType<PlayerSpawnManager>().Length;
        if (numPlayerSpawnManagers > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
