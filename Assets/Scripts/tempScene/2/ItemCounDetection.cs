using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCounDetection : MonoBehaviour
{
    public int itemCount;
    Controller controller;
    void Start()
    {
        controller = Global.Component.GetController();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
