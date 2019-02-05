using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocationCache : MonoBehaviour
{

    //This allows the character to spawn in the right place in the previous scene
    //If they return to that scene
    [SerializeField] public string previousSceneName;
    [SerializeField] public Vector3 playerTransformInPreviousScene;
    [SerializeField] public Quaternion playerRotationInPreviousScene;

    //Used for actually loading the previous location data when returning to the scene
    //Necessary because by the time the new scene loads the variables above are used for the newly previous scene
    public bool useCacheOnSceneLoad;
    public Vector3 playerTransformToLoad;
    public Quaternion playerRotationToLoad;

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
        useCacheOnSceneLoad = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SavePreviousSceneData(string currentSceneName, string nextSceneName, Vector3 location, Quaternion rotation)
    {
        print("NextScene: " + nextSceneName);
        print("Previous Scene Name: " + previousSceneName);
        print("");
        if(nextSceneName == previousSceneName)
        {
            print("GOT HERE!!!!");
            useCacheOnSceneLoad = true;
            playerTransformToLoad = playerTransformInPreviousScene;
            playerRotationToLoad = playerRotationInPreviousScene;
        }
        else
        {
            useCacheOnSceneLoad = false;
        }

        previousSceneName = currentSceneName;
        playerTransformInPreviousScene = location;
        playerRotationInPreviousScene = rotation;
    }
}
