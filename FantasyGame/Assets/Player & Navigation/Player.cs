using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth = 100f;

    [SerializeField] float maxMagic = 100f;
    [SerializeField] float currentMagic = 100f;
    [SerializeField] float magicRegenerationPerSecond = 5f;

    PlayerHealthBar playerHealthBar;
    PlayerMagicBar playerMagicBar;

    void Start()
    {
        playerHealthBar = FindObjectOfType<PlayerHealthBar>();
        playerMagicBar = FindObjectOfType<PlayerMagicBar>();
        InvokeRepeating("RegenerateMagic", 0f, 1f);
    }

    public float GetHealthAsPercentage()
    {
        return currentHealth / maxHealth;
    }

    public float GetMagicAsPercentage()
    {
        return currentMagic / maxMagic;
    }

    public bool HasEnoughMagic(float magicRequired)
    {
        return currentMagic >= magicRequired;
    }

    public void ReduceMagic(float magicRequired)
    {
        currentMagic -= magicRequired;
        playerMagicBar.UpdateMagicBar();
    }

    void RegenerateMagic()
    {
        if (currentMagic <= maxMagic)
        {
            currentMagic = Mathf.Clamp(currentMagic + magicRegenerationPerSecond, 0f, maxMagic);
            playerMagicBar.UpdateMagicBar();
        }
    }

    void Update()
    {
        
    }
}
