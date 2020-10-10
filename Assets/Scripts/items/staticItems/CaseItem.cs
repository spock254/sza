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
