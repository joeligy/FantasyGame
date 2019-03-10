using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour
{

    AICharacterControl aiCharacterControl;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        aiCharacterControl = GetComponent<AICharacterControl>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        aiCharacterControl.target = player.transform;
    }
}
