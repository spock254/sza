using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CaseItem : MonoBehaviour
{
    [SerializeReference]
    public List<Item> items;
    [SerializeReference]
    public List<Item> itemsToUnlockCase;

    public int caseCapacity;
    public bool isLocked = false;

    void Start()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i] = Instantiate(items[i]);
        } 
    }

    public int CountInnerCapacity()
    {
        int innerCapacity = 0;

        foreach (var item in items)
        {
            innerCapacity += item.GetItemSize();
        }

        return innerCapacity;
    }
}
