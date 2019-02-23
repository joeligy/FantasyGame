using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{

    [Tooltip("The scene this door leads to")] [SerializeField] string sceneName;
    [Tooltip("The spawn point to start at (optional)")] [SerializeField] string spawnPointName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetSceneName()
    {
        return sceneName;
    }

    public string GetSpawnPointName()
    {
        return spawnPointName;
    }
}
