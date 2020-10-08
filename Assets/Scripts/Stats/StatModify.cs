using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatModify
{
    public static void AddValue(StatsField statsField, float value) 
    {
        statsField.Value += value;

        if (statsField.Value > statsField.MAX_VALUE) 
        {
            statsField.Value = statsField.MAX_VALUE;
        }
    }

    public static void ChangeDuration(StatsField statsField, float value, float time) 
    {
        statsField.TempDuration = value;
        statsField.BuffTime = time;
    }

    public static void HealthControll(StatsField health, StatsField atribute, float damage) 
    {
        if (atribute.Value <= 0 && !atribute.IsEmpty)
        {
            health.Duration -= damage;
            atribute.IsEmpty = true;
        }
        else if (atribute.IsEmpty && atribute.Value > 0)
        {
            health.Duration += damage; //TODO: 
            atribute.IsEmpty = false;
        }

        health.IsEmpty = health.Value <= 0;
    }
}
