using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffController : MonoBehaviour
{
    //List<Image> buffImages = new List<Image>();
    List<GameObject> buffCells = new List<GameObject>();
    List<GameObject> debuffCells = new List<GameObject>();

    EventController eventController;
    void Awake()
    {
        eventController = Global.Component.GetEventController();

        GameObject buffPanelGo = Global.UIElement.GetBuffPanel();
        GameObject debuffPanelGo = Global.UIElement.GetDeBuffPanel();

        foreach (Transform tr in buffPanelGo.transform)
        {
            //buffImages.Add(tr.gameObject.GetComponent<Image>());
            buffCells.Add(tr.gameObject);
        }

        foreach (Transform tr in debuffPanelGo.transform)
        {
            debuffCells.Add(tr.gameObject);
        }
    }

    void Start()
    {
        eventController.OnAddBuffEvent.AddListener(AddBuff);
        eventController.OnRemoveBuffEvent.AddListener(RemoveBuff);
    }

    void AddBuff(Item item) 
    {
        List<GameObject> buffs = (item.itemBuff.buff.buffMode == Buff.BuffMode.BUFF) ? buffCells : debuffCells;

        // remove debuff if  buff contains list 
        if (item.itemBuff.buff.buffMode == Buff.BuffMode.BUFF) 
        {
            Buff debuffToRemove = item.itemBuff.buff.debuffToRemove;

            if (debuffToRemove != null) 
            {
                foreach (var cell in debuffCells)
                {
                    BuffCell activeBuffCell = cell.GetComponent<BuffCell>();
                    
                    if (activeBuffCell.buffType == debuffToRemove.buffType) 
                    {
                        RemoveBuff(cell, debuffToRemove);
                        return;
                    }
                }
            }
        }
        
        //find if buff exist
        foreach (var cell in buffs)
        {
            BuffCell activeBuffCell = cell.GetComponent<BuffCell>();
            
            if (activeBuffCell.buffType == item.itemBuff.buff.buffType) 
            {
                if (activeBuffCell.GetBuffTimeLeft() < item.itemBuff.buffTime 
                    || item.itemBuff.buffTime <= Global.CONSTANT_BUFF_TIME) 
                { 
                    activeBuffCell.RefreshBuff(cell, item);
                }

                return;
            }
        }

        //if not exist
        GameObject freeCell = FindFreeBuffCell(buffs);
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
        cell.GetComponent<BuffCell>().SetBuffActive(false);
    }

    void RemoveBuff(GameObject cell, Buff buff) 
    {
        buff.BuffDiactivate();

        cell.GetComponent<Image>().sprite = null;
        cell.SetActive(false);
        cell.GetComponent<BuffCell>().buffType = Buff.BuffType.None;
        cell.GetComponent<BuffCell>().SetBuffActive(false);
    }

    GameObject FindFreeBuffCell(List<GameObject> cells) 
    {
        GameObject cellToreturn = null;

        foreach (var cell in cells)
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

    public BuffCell TryGetBuffCell(Item item)
    {

        foreach (var cell in buffCells)
        {
            BuffCell currentCell = cell.GetComponent<BuffCell>();

            if (currentCell.buffType == item.itemBuff.buff.buffType)
            {
                return currentCell;
            }
        }

        return null;
    }

    public void ActivateBuff(Item item) 
    {
        if (item.itemBuff.buff != null)
        {

            BuffCell buffCell = TryGetBuffCell(item);

            if (buffCell != null)
            {
                buffCell.SetBuffActive(false);
            }

            item.itemBuff.buff.BuffActivate(item);
        }
    }

    public void DiactivateBuff(Item item) 
    {
        if (item.itemBuff.buff != null)
        {
            BuffCell buffCell = TryGetBuffCell(item);

            if (buffCell != null)
            {
                buffCell.SetBuffActive(false);
            }

            item.itemBuff.buff.BuffDiactivate();
        }
    }
}
