﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionWindowController : MonoBehaviour
{
    public bool isOpen = false;

    public void OpenActionWindow(string tag) 
    {
        GameObject actionWindow = GameObject.FindGameObjectWithTag(tag);

        for (int i = 0; i < actionWindow.transform.childCount; i++)
        {
            actionWindow.transform.GetChild(i).gameObject.SetActive(true);
        }

        isOpen = true;
    }

    public void InitActioWindow(string tag, Item item) 
    {
            
        if (tag == "awPaper")
        {
            Global.Component.GetPaperController().Init(item);
        }
    }

    public void CloseActionWindow(string tag) 
    {
        GameObject actionWindow = GameObject.FindGameObjectWithTag(tag);

        for (int i = 0; i < actionWindow.transform.childCount; i++)
        {
            actionWindow.transform.GetChild(i).gameObject.SetActive(false);
        }

        isOpen = false;
    }
}