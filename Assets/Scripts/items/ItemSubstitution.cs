using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class ItemSubstitution
{
    public GameObject prefToSubstitut;
    public StateTypes initState = StateTypes.NPC_STATE_clickWaiting;
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
        Item itemCpy = GameObject.Instantiate(itemGo.GetComponent<ItemCell>().item);
        GameObject substitudeClone = GameObject.Instantiate(prefToSubstitut, itemGo.transform.position, Quaternion.identity);
        substitudeClone.GetComponent<SubstitudeCell>().item = itemCpy;
        GameObject.Destroy(itemGo);

        //if (itemCpy.itemOptionData.text == "upgrated") 
        //{
        //    Debug.Log("CHANGE STATE");
        //    substitudeClone.GetComponent<NPC_StateMashine>().SetInitialState<NPC_STATE_stateTransitionModify>();
        //}

        return substitudeClone;
    }
}
