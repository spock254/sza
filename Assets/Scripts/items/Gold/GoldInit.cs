using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldInit : MonoBehaviour
{
    public Gold gold;
    void Start()
    {
        gold = new Gold(100);
    }
}
