using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CasePanelController : MonoBehaviour
{
    public GameObject staticItemPanel;

    // иниц событием
    CaseItem caseItem;
    Transform casePosition;

    public Controller controller;
    public bool caseIsOpen;

    public void ActivateStaticItemPanel(CaseItem caseItem, Transform casePosition) 
    {
        this.caseItem = caseItem;
        this.casePosition = casePosition;

        if (caseItem.isLocked && !OpenWithKey()) 
        {
            Debug.Log("No item to unlock case");
            return;
        }

        controller.CloseOpenContainer(staticItemPanel, ref caseIsOpen);
        controller.ContainerContentInit(caseItem.items, staticItemPanel);
    }

    void Update()
    {
        if (caseIsOpen && !IsInActionRadius())
        {
            controller.CloseOpenContainer(staticItemPanel, ref caseIsOpen);
        }
    }

    public void OnCasePanelClick(string caseCellIndex) 
    {
        GameObject caseCellGo = GameObject.FindGameObjectWithTag(caseCellIndex);
        Button caseCellBtn = caseCellGo.GetComponent<Button>();

        Item caseCellItem = caseCellGo.GetComponent<ItemCell>().item;
        Item handItem = controller.currentHand.GetComponent<ItemCell>().item;
        List<Item> innerItems = caseItem.items;

        if (controller.IsEmpty(controller.currentHand) && !controller.IsEmpty(caseCellBtn))
        {
            innerItems.Remove(caseCellItem);
            controller.DressCell(controller.currentHand, caseCellItem);
            controller.SetDefaultItem(caseCellBtn);
        }
        else if (!controller.IsEmpty(controller.currentHand) && !controller.IsEmpty(caseCellBtn)) //TODO: проверить если достаточно места в сумке 
        {
            // если достаточно места в сумке для свапа
            if (caseItem.CountInnerCapacity() - caseCellItem.GetItemSize() + handItem.GetItemSize() 
                <= caseItem.caseCapacity) 
            { 
                innerItems.Add(handItem);
                innerItems.Remove(caseCellItem);
                controller.DressCell(controller.currentHand, caseCellItem);
                controller.DressCell(caseCellBtn, handItem);
            
            }
        }
        else if (!controller.IsEmpty(controller.currentHand) && controller.IsEmpty(caseCellBtn))
        {
            // если достаточно места в сумке для добавления
            if (caseItem.CountInnerCapacity() + handItem.GetItemSize() <= caseItem.caseCapacity)
            {
                innerItems.Add(handItem);
                controller.DressCell(caseCellBtn, handItem);
                controller.SetDefaultItem(controller.currentHand);
            }
        }
    }

    public bool IsInActionRadius() 
    {
        return Vector2.Distance(casePosition.position, controller.player.position) < controller.actioPlayerRadius;
    }

    public bool OpenWithKey() 
    {
        Item handItem = controller.currentHand.GetComponent<ItemCell>().item;

        foreach (var item in caseItem.itemsToUnlockCase)
        {
            if (handItem.IsSameItems(item)) 
            {
                return true;
            }
        }

        return false;
    }
}
