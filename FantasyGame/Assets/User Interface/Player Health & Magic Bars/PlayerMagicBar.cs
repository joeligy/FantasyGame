using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicBar : MonoBehaviour
{
    ProgressBarPro progressBarPro;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        progressBarPro = GetComponent<ProgressBarPro>();
        player = FindObjectOfType<Player>();
        progressBarPro.SetValue(player.GetMagicAsPercentage());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMagicBar()
    {
        progressBarPro.SetValue(player.GetMagicAsPercentage());
    }
}
