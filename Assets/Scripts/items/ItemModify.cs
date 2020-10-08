using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class ItemModify
{
    //public static void RemoveItem(List<InventoryCell> inventory, InventoryCell item) 
    //{
    //    inventory.Remove(item);
    //}

    public static void UseShopItem(Item item, Gold gold) 
    {
        if (gold.GoldAmount >= item.itemPrice) 
        {
            Debug.Log("Buy");
            gold.GoldAmount -= item.itemPrice;
        }
        else 
        {
            Debug.Log("Not enough gold");
        }
    }
    public static void UseItem(Item item, Stats stats) 
    {
        if (item == null) 
        {
            Debug.LogError("UseItem item == null");
            return;
        }
        //item.stats.duration
        if (item.stats.playerStats == PlayerStats.HUNGER) 
        {
            stats.Hunger.Value += item.stats.value;
            
            if (stats.Hunger.TempDuration + item.stats.duration > stats.Hunger.MAX_TEMP_DURATION) 
            {
                stats.Hunger.TempDuration = stats.Hunger.MAX_TEMP_DURATION;
            }
            else 
            { 
                stats.Hunger.TempDuration += item.stats.duration;
            }

            stats.Hunger.BuffTime = item.stats.time;
        }
        else if (item.stats.playerStats == PlayerStats.SLEEP)
        {
            stats.Sleep.Value += item.stats.value;
            
            if (stats.Sleep.TempDuration + item.stats.duration > stats.Sleep.MAX_TEMP_DURATION)
            {
                stats.Sleep.TempDuration = stats.Sleep.MAX_TEMP_DURATION;
            }
            else
            {
                stats.Sleep.TempDuration += item.stats.duration;
            }

            stats.Sleep.BuffTime = item.stats.time;
        }
        else if (item.stats.playerStats == PlayerStats.HAPPINESS)
        {
            stats.Happiness.Value += item.stats.value;

            if (stats.Happiness.TempDuration + item.stats.duration > stats.Happiness.MAX_TEMP_DURATION)
            {
                stats.Happiness.TempDuration = stats.Happiness.MAX_TEMP_DURATION;
            }
            else
            {
                stats.Happiness.TempDuration += item.stats.duration;
            }

            stats.Happiness.BuffTime = item.stats.time;
        }
        else if (item.stats.playerStats == PlayerStats.HEALTH)
        {
            stats.Health.Value += item.stats.value;

            if (stats.Health.TempDuration + item.stats.duration > stats.Health.MAX_TEMP_DURATION)
            {
                stats.Health.TempDuration = stats.Health.MAX_TEMP_DURATION;
            }
            else
            {
                stats.Health.TempDuration += item.stats.duration;
            }

            stats.Health.BuffTime = item.stats.time;
        }

    }
}
