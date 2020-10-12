using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftController : MonoBehaviour
{
    public ItemCraftData itemCraftData;
    public void Craft(RaycastHit2D[] hits, Item tool) 
    {
        GameObject GameObjOnTable = GetGameObjOnTable(hits);


        if (!IsToolInRecept(itemCraftData, tool)) 
        {
            Debug.Log("No tool");
            return;
        }

        // если стол для крафта не пустой
        if (GameObjOnTable != null) 
        {
            Item itemOnTable = GameObjOnTable.GetComponent<ItemCell>().item;

            if (itemCraftData.craftComplexety == CraftComplexety.Simple) 
            {
                if (itemOnTable.IsSameItems(itemCraftData.recept.ingredients[0])) 
                {
                    // подстановка текущуго айтеса на крафт айтем 
                    Item resutItem = itemCraftData.recept.craftResult;
                    GameObjOnTable.GetComponent<ItemCell>().item = resutItem;
                    GameObjOnTable.GetComponent<SpriteRenderer>().sprite = resutItem.itemSprite;
                }
            }
        }
    }

    GameObject GetGameObjOnTable(RaycastHit2D[] hits) 
    {
        foreach (var hit in hits)
        {
            if (hit.collider.name.Contains(Global.DROPED_ITEM_PREFIX)) 
            {
                return hit.collider.gameObject;
            }
        }

        return null;
    }

    bool IsToolInRecept(ItemCraftData itemCraftData, Item toolInHand) 
    {
        foreach (var tool in itemCraftData.recept.craftTool)
        {
            if (tool.IsSameItems(toolInHand)) 
            {
                return true;
            }
        }

        return false;
    }
}
