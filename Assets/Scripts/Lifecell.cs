using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lifecell : MonoBehaviour
{
    public Stats stats;
    public StatInit statInit;
    public ExpInit exp;
    public FightStatsInit fightStatsInit;

    UnityEvent lvlUp;
    UnityEvent dead;

    private float hungerDamage = 0.5f;
    private float dehydrationDamage = 0.5f;
    private float sleepDamage = 0.5f;
    private float happinessDamage = 0.5f;

    private float baseHealthDuration = 0;
    private bool isGettingDamage = false;
    private void Start()
    {
        stats = statInit.stats;
        baseHealthDuration = stats.Health.Duration;

        if (lvlUp == null) 
        {
            lvlUp = new UnityEvent();
        }

        if (dead == null)
        {
            dead = new UnityEvent();
        }

        lvlUp.AddListener(FightStatsModify);

        dead.AddListener(Dead);

        //StartCoroutine(UpdateStats());
    }

    IEnumerator UpdateStats() 
    {
        while (true) 
        {
            timeModify(stats.Age);
            timeModify(stats.Health);
            timeModify(stats.Hunger);
            timeModify(stats.Dehydration);
            timeModify(stats.Sleep);
            timeModify(stats.Happiness);

            // !!! BEFORE LvlCalculation !!!
            if (exp.exp.isLvlUp())
            {
                lvlUp.Invoke();
            }

            LvlCalculation();

            if ((stats.Hunger.IsEmpty || stats.Sleep.IsEmpty || stats.Happiness.IsEmpty || stats.Dehydration.IsEmpty) && !isGettingDamage)
            {
                stats.Health.Duration = Stats.healthDuration;
                isGettingDamage = true;
            }
            else if (!stats.Hunger.IsEmpty && !stats.Sleep.IsEmpty && !stats.Happiness.IsEmpty && !stats.Dehydration.IsEmpty)
            {
                stats.Health.Duration = float.MaxValue;
                isGettingDamage = false;
            }

            StatModify.HealthControll(stats.Health, stats.Hunger, hungerDamage);
            StatModify.HealthControll(stats.Health, stats.Dehydration, dehydrationDamage);
            StatModify.HealthControll(stats.Health, stats.Sleep, sleepDamage);
            StatModify.HealthControll(stats.Health, stats.Happiness, happinessDamage);


            if (!isAlife(statInit))
            {
                dead.Invoke();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    void Update()
    {
        timeModify(stats.Age);
        timeModify(stats.Health);
        timeModify(stats.Hunger);
        timeModify(stats.Dehydration);
        timeModify(stats.Sleep);
        timeModify(stats.Happiness);

        // !!! BEFORE LvlCalculation !!!
        if (exp.exp.isLvlUp())
        {
            lvlUp.Invoke();
        }

        LvlCalculation();

        if ((stats.Hunger.IsEmpty || stats.Sleep.IsEmpty || stats.Happiness.IsEmpty || stats.Dehydration.IsEmpty) && !isGettingDamage)
        {
            stats.Health.Duration = Stats.healthDuration;
            isGettingDamage = true;
        }
        else if (!stats.Hunger.IsEmpty && !stats.Sleep.IsEmpty && !stats.Happiness.IsEmpty && !stats.Dehydration.IsEmpty)
        {
            stats.Health.Duration = float.MaxValue;
            isGettingDamage = false;
        }

        StatModify.HealthControll(stats.Health, stats.Hunger, hungerDamage);
        StatModify.HealthControll(stats.Health, stats.Dehydration, dehydrationDamage);
        StatModify.HealthControll(stats.Health, stats.Sleep, sleepDamage);
        StatModify.HealthControll(stats.Health, stats.Happiness, happinessDamage);


        if (!isAlife(statInit))
        {
            dead.Invoke();
        }

    }

    public void timeModify(StatsField statsField)
    {
       // float oldDuration = statsField.duration;
        if (statsField.Value < 0) 
        {
            statsField.Value = 0;
        }

        if (statsField.BuffTime > 0)
        {
            statsField.Value -= Time.deltaTime / statsField.TempDuration;
            statsField.BuffTime -= Time.deltaTime;
            return;
        }

        statsField.Value -= Time.deltaTime / statsField.Duration;
    }

    private void FightStatsModify() 
    {
        Debug.Log("lvl up");
    }

    public void LvlCalculation() 
    {
        if (exp.exp.Current_exp >= exp.exp.NextExpAmount())
        {
            exp.exp.Lvl++;
        }
    }

    private bool isAlife(StatInit stat) 
    {
        return stat.stats.Age.Value < stat.stats.Age.MAX_VALUE
            && stat.stats.Health.Value > 0;
    }

    private void Dead() 
    {
        Debug.Log("player dead");
    }
}
