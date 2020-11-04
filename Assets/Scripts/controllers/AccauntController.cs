using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccauntController : MonoBehaviour
{
    [SerializeField]
    int money = 0;

    public int GetAccautBalance() 
    {
        return money;
    }

    public void SetAccauntBalance(int money) 
    {
        this.money = money;
    }

    public int Add(int money) 
    {
        this.money += money;
        return this.money;
    }

    public int Remove(int money) 
    {
        this.money -= money;
        return this.money;
    }
}
