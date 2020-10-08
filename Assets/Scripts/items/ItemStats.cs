using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemStats
{
    public PlayerStats playerStats;
    public float value;
    public float duration;
    public float time;

    public ItemStats(PlayerStats playerStats, 
        float value, 
        float duration, float time)
    {
        this.playerStats = playerStats;
        this.value = value;
        this.duration = duration;
        this.time = time;
    }
}
