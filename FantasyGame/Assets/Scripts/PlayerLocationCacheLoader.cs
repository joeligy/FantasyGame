using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocationCacheLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        PlayerLocationCache cache = FindObjectOfType<PlayerLocationCache>();
        if(cache.useCacheOnSceneLoad)
        {
            transform.position = new Vector3(0, 0, 0);
            transform.rotation = cache.playerRotationToLoad;
        }
        transform.position = new Vector3(0, 0, 0);
        print(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        print(transform.position);
    }
}
