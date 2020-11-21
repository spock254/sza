using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ItemOptionData
{
    public GameObject awPrefab;
    public string text;
    public string actionWindowTag;
    public bool isModified = false;

    public ItemOptionData(GameObject awPrefab, string text, string actionWindowTag, bool isModified)
    {
        this.awPrefab = awPrefab;
        this.text = text;
        this.actionWindowTag = actionWindowTag;
        this.isModified = isModified;
    }
}
