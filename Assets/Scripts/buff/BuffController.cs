using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffController : MonoBehaviour
{
    List<Image> buffImages = new List<Image>();
    List<GameObject> buffCells = new List<GameObject>();

    EventController eventController;
    void Awake()
    {
        eventController = Global.Component.GetEventController();

        GameObject buffPanelGo = Global.UIElement.GetBuffPanel();

        foreach (Transform tr in buffPanelGo.transform)
        {
            buffImages.Add(tr.gameObject.GetComponent<Image>());
            buffCells.Add(tr.gameObject);
        }

    }

    void Start()
    {
        eventController.OnAddBuffEvent.AddListener(AddBuff);
        eventController.OnRemoveBuffEvent.AddListener(RemoveBuff);
    }

    void AddBuff(Item item) 
    {
        //find if buff exist
        foreach (var cell in buffCells)
        {
            BuffCell activeBuffCell = cell.GetComponent<BuffCell>();
            
            if (activeBuffCell.buffType == item.itemBuff.buff.buffType) 
            {
                activeBuffCell.RefreshBuff(cell, item);

                return;
            }
        }

        //if not exist
        GameObject freeCell = FindFreeBuffCell();
        BuffCell buffCell = null;
        // когда все ячейки для баффа заняты
        if (freeCell == null) 
        {
            return;
        }

        buffCell = freeCell.GetComponent<BuffCell>();
        
        freeCell.SetActive(true);
        freeCell.GetComponent<Image>().sprite = item.itemBuff.buff.buffSprite;
        buffCell.buffType = item.itemBuff.buff.buffType;

        buffCell.InitBuff(freeCell, item);
        //StartCoroutine(buffCell.BuffLifeTime(freeCell, item));
    }

    void RemoveBuff(GameObject cell, Item item) 
    {
        item.itemBuff.buff.BuffDiactivate();

        cell.GetComponent<Image>().sprite = null;
        cell.SetActive(false);
        cell.GetComponent<BuffCell>().buffType = Buff.BuffType.None;
    }

    GameObject FindFreeBuffCell() 
    {
        GameObject cellToreturn = null;

        foreach (var cell in buffCells)
        {
            if (cell.activeInHierarchy == false) 
            {
                return cell;
            }
        }

        return cellToreturn;
    }

    public bool IsBuffExist(Item item) 
    {
        foreach (var cell in buffCells)
        {
            BuffCell activeBuffCell = cell.GetComponent<BuffCell>();
            
            if (activeBuffCell.buffType == item.itemBuff.buff.buffType)
            {
                return true;
            }
        }

        return false;
    }
}
