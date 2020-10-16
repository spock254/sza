using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TabWaterController : MonoBehaviour
{
    List<Item> vessels;

    void Awake()
    {
        vessels = Resources.LoadAll<Item>(Global.Path.VESSELS).ToList();
    }

    private void Start()
    {
        
    }

    public void OnWaterTap_Click(Button hand) 
    {
        if (hand == null) 
        {
            Debug.Log("No vessel");
            return;
        }

        Item itemInHand = hand.GetComponent<ItemCell>().item;
        List<Item> pair = new List<Item>();

        foreach (var item in vessels)
        {
            if (GetItemPairPrefix(item.itemName).Equals(GetItemPairPrefix(itemInHand.itemName)))
            {
                pair.Add(item);
            }
        }

        if (pair.Count == 0) 
        {
            Debug.Log("cant fill");
            return;
        }

        Item filledVessel = isFillVessel(pair[0]) ? pair[0] : pair[1];

        hand.GetComponent<ItemCell>().item = filledVessel;
        hand.GetComponent<Image>().sprite = filledVessel.itemSprite;
    }

    //пара пустой полный в конце 00 или 01 тд
    // пара мижет состоять только из двух одинаковых префиксов один из которых e00 другой f00
    string GetItemPairPrefix(string itemName) 
    {
        return itemName.Substring(itemName.Length - 2);
    }

    bool isFillVessel(Item item) 
    {
        return item.itemName[item.itemName.Length - 3] == 'f';
    }
}
