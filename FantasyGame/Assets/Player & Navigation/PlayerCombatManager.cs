using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerCombatManager : MonoBehaviour
{

    [SerializeField] Rigidbody fireBallSpell;
    [SerializeField] float fireBallSpeed = 10f;

    [SerializeField] float health = 100f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Cast Spell"))
        {
            Vector3 spellOriginationPoint = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
            Rigidbody fireball = Instantiate(fireBallSpell, spellOriginationPoint, transform.rotation, transform);
            fireball.velocity = transform.forward * fireBallSpeed;
        }
    }
}
