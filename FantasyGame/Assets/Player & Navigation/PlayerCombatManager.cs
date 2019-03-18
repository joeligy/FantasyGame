using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerCombatManager : MonoBehaviour
{

    [SerializeField] Rigidbody fireBallSpell;
    [SerializeField] float fireBallSpeed = 1f;
    [SerializeField] float fireBallMagicRequired = 30f;

    [SerializeField] float health = 100f;

    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Cast Spell") && player.HasEnoughMagic(fireBallMagicRequired))
        {
            CastFireBall();
        }
    }

    private void CastFireBall()
    {
        InstantiateAndShootFireBallParticle();
        player.ReduceMagic(fireBallMagicRequired);
    }

    private void InstantiateAndShootFireBallParticle()
    {
        Vector3 spellOriginationPoint = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
        Quaternion spellOriginationRotation = Quaternion.Euler(Camera.main.gameObject.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        Rigidbody fireball = Instantiate(fireBallSpell, spellOriginationPoint, spellOriginationRotation);
        fireball.velocity = fireball.transform.forward * fireBallSpeed;
    }
}
