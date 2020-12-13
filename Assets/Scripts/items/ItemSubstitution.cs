using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class ItemSubstitution
{
    public GameObject prefToSubstitut;

    public List<Item> itemsToUse;

    public bool IsItemToUseExist(Item item) 
    {
        foreach (var itemToUse in itemsToUse)
        {
            if (itemToUse.IsSameItems(item)) 
            {
                return true;
            }
        }

        return false;
    }

    public bool IsSubstituted()
    {
        return prefToSubstitut != null;
    }

    public bool IsUsable(Item item) 
    {
        return IsSubstituted() && IsItemToUseExist(item);
    }

    public GameObject Substitute(GameObject itemGo) 
    {
        GameObject substitudeClone = GameObject.Instantiate(prefToSubstitut, itemGo.transform.position, Quaternion.identity);
        GameObject.Destroy(itemGo);

        return substitudeClone;
    }
}
