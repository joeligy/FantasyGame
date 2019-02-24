using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnToPlayer()
    {
        transform.LookAt(Camera.main.transform);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}
