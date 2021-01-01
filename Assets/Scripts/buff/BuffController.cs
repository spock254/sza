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
            if (cell.GetComponent<BuffCell>().buffType == item.itemBuff.buff.buffType) 
            {
                return;
            }
        }

        //if not exist
        GameObject freeCell = FindFreeBuffCell();

        // когда все ячейки для баффа заняты
        if (freeCell == null) 
        {
            return;
        }
        
        freeCell.SetActive(true);
        freeCell.GetComponent<Image>().sprite = item.itemBuff.buff.buffSprite;
        freeCell.GetComponent<BuffCell>().buffType = item.itemBuff.buff.buffType;
        
        StartCoroutine(BuffLifeTime(freeCell, item));
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

    IEnumerator BuffLifeTime(GameObject cell, Item item) 
    {
        yield return new WaitForSeconds(item.itemBuff.buffTime);

        eventController.OnRemoveBuffEvent.Invoke(cell, item);
    }
}
