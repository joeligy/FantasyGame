using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocationCache : MonoBehaviour
{

    //This allows the character to spawn in the right place in the previous scene
    //If they return to that scene
    [SerializeField] string previousSceneName;
    [SerializeField] Vector3 playerLocationInPreviousScene;

    private void Awake()
    {
        int numPlayerLocationCaches = FindObjectsOfType<PlayerLocationCache>().Length;
        if (numPlayerLocationCaches > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SavePreviousSceneData(string scene, Vector3 location)
    {
        previousSceneName = scene;
        playerLocationInPreviousScene = location;
    }
}
