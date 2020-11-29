using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TS2LaggaadgeCheck : MonoBehaviour, IAction
{
    bool isPlayerWithLaggadge;

    Controller controller;

    private void Start()
    {
        controller = Global.Component.GetController();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "player") 
        {

            foreach (var cell in controller.cellList)
            {
                Item itemInCell = cell.GetComponent<ItemCell>().item;

                if (itemInCell != null && itemInCell.itemUseData.itemTypes.Contains(ItemUseData.ItemType.Openable)
                    && itemInCell.itemUseData.itemSize == ItemUseData.ItemSize.Middle) 
                {
                    isPlayerWithLaggadge = true;
                    return;
                }
            }
        }
    }

    public bool IsInAction()
    {
        return isPlayerWithLaggadge;
    }
    public void Action()
    {
        
    }
}
