using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStats
{
    AGE,
    HEALTH,
    HUNGER,
    SLEEP,
    HAPPINESS,
    NONE
}

public class Stats
{
    public static readonly float healthDuration = 2;

    private StatsField health;
    private StatsField hunger;
    private StatsField sleep;
    private StatsField happiness;
    private StatsField age;

    // private List<StatsField> statsFieldsList;
    public Stats(StatsField health, StatsField hunger, StatsField sleep, StatsField happiness, StatsField age)
    {
        this.health = health;
        this.hunger = hunger;
        this.sleep = sleep;
        this.happiness = happiness;
        this.Age = age;

    }

    public StatsField Health { get => health; set { if (value.Value <= 0) health.Value = 0; } }
    public StatsField Hunger { get => hunger; set { if (value.Value <= 0) hunger.Value = 0; } }
    public StatsField Sleep { get => sleep; set { if (value.Value <= 0) sleep.Value = 0; } }
    public StatsField Happiness { get => happiness; set { if (value.Value <= 0) happiness.Value = 0; } }
    public StatsField Age { get => age; set => age = value; }
}

public class StatsField
{
    private float value;
    private float buffTime;
    private float duration;
    private float tempDuration;
    public readonly float MAX_TEMP_DURATION;
    private readonly string name;
    private bool isEmpty;
    public readonly float MAX_VALUE;

    public float Value { get => value; 
        set 
        {
            if (value >= this.MAX_VALUE) 
            {
                //Debug.Log(MAX_VALUE);
                this.value = MAX_VALUE;
                return;
            }

            this.value = value; 
        } 
    }
    public float BuffTime { get => buffTime; set => buffTime = value; }
    public float Duration { get => duration; set => duration = value; }
    public float TempDuration { 
        get 
        {
            //if (BuffTime == 0)
            //{
            //    tempDuration = 0;
            //}
            //Debug.Log("wdwdw");
            return tempDuration;   
        }
        set 
        {
            if ((int)BuffTime == 0) 
            {
                this.tempDuration = 0;
            }
            tempDuration = value;
        }  
    }

    public string Name => name;

    public bool IsEmpty { get => isEmpty; set => isEmpty = value; }

    //public float MAX_VALUE => mAX_VALUE;

    public StatsField(float value, float duration, float MAX_TEMP_DURATION, string name, bool isEmpty, float MAX_VALUE)
    {
        this.MAX_VALUE = MAX_VALUE;
        this.MAX_TEMP_DURATION = MAX_TEMP_DURATION;
        this.Value = value;
        this.Duration = duration;
        this.name = name;
        this.IsEmpty = isEmpty;
        TempDuration = 0;
        BuffTime = 0;
    }

    public StatsField(float value, float duration, float MAX_TEMP_DURATION, string name, float MAX_VALUE)
    {
        this.MAX_VALUE = MAX_VALUE;
        this.MAX_TEMP_DURATION = MAX_TEMP_DURATION;
        this.Value = value;
        this.Duration = duration;
        this.name = name;
        this.IsEmpty = value <= 0;
        TempDuration = 0;
        BuffTime = 0;
    }
}