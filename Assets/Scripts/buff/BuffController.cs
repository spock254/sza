﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffController : MonoBehaviour
{
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

    public void ActivateBuffInInit(Item item)
    {
        StartCoroutine(LateStart(item));
    }

    IEnumerator LateStart(Item item)
    {
        yield return new WaitForEndOfFrame();

        ActivateBuff(item);
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
                        if (IsBuffExist(item) == false)
                        {
                            if (item.itemBuff.buffTime > Global.Buff.CONSTANT_BUFF_TIME)
                            {
                                RemoveBuff(cell, debuffToRemove);
                                return;
                            }
                            else // если шмотка  
                            {
                                if (IsBuffExistByTypeName(debuffToRemove.buffType, false)) 
                                {
                                    RollBackBuff(debuffToRemove);
                                }
                            }
                        }
                        else
                        {
                            if (item.itemBuff.buffTime > Global.Buff.CONSTANT_BUFF_TIME)
                            {
                                RemoveBuff(cell, debuffToRemove);
                                item.itemBuff.buff.BuffDirty();
                            }
                        }
                    }
                }
            }
        }
        else if (item.itemBuff.buff.buffMode == Buff.BuffMode.DEBUFF) 
        {
            Buff buffToRemove = item.itemBuff.buff.debuffToRemove;

            if (buffToRemove != null)
            {
                foreach (var cell in buffCells)
                {
                    BuffCell activeBuffCell = cell.GetComponent<BuffCell>();

                    if (activeBuffCell.buffType == buffToRemove.buffType)
                    {
                        if (activeBuffCell.GetBuffTimeLeft() != float.MaxValue)
                        {
                            RemoveBuff(cell, buffToRemove);
                            return;
                        }
                        else 
                        {
                            RollBackBuff(item);
                        }
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
                    || item.itemBuff.buffTime <= Global.Buff.CONSTANT_BUFF_TIME) 
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
        freeCell.transform.GetChild(0).GetComponent<Text>().text = item.itemBuff.buff.buffDescription;
        buffCell.buffType = item.itemBuff.buff.buffType;

        buffCell.InitBuff(freeCell, item);
    }

    /// <summary>
    /// Отключает изменения бафа и асайнит ребаф того же бафа
    /// <br>используеть ребаф после окончания бафа</br>
    /// </summary>
    void RollBackBuff(Item item) 
    {
        IBuff debuff = Global.Buff.GetIBuffByType(item.itemBuff.buff.buffType);
        IBuff rebuff = Global.Buff.GetIBuffByType(item.itemBuff.buff.debuffToRemove.buffType);
        item.itemBuff.buff.DeBuffDirty();
        debuff.SetRebuff(rebuff);
    }

    /// <summary>
    /// Отключает изменения бафа и асайнит ребаф того же бафа
    /// <br>используеть ребаф после окончания бафа</br>
    /// </summary>
    void RollBackBuff(Buff buff) 
    {
        IBuff debuff = Global.Buff.GetIBuffByType(buff.buffType);
        IBuff rebuff = Global.Buff.GetIBuffByType(buff.debuffToRemove.buffType);
        buff.DeBuffDirty();
        debuff.SetRebuff(rebuff);
    }

    /// <summary>
    /// Завершает и удаляет баф с баф селл
    /// </summary>
    void RemoveBuff(GameObject cell, Item item) 
    {
        item.itemBuff.buff.BuffDiactivate();

        cell.GetComponent<Image>().sprite = null;
        cell.SetActive(false);
        cell.GetComponent<BuffCell>().buffType = BuffType.None;
        cell.GetComponent<BuffCell>().SetBuffActive(false);
    }

    /// <summary>
    /// Завершает и удаляет баф с баф селл
    /// </summary>
    void RemoveBuff(GameObject cell, Buff buff) 
    {
        buff.BuffDiactivate();

        cell.GetComponent<Image>().sprite = null;
        cell.SetActive(false);
        cell.GetComponent<BuffCell>().buffType = BuffType.None;
        cell.GetComponent<BuffCell>().SetBuffActive(false);
    }

    /// <summary>
    /// Возврощает первую свободную ячейку
    /// </summary>
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

    /// <summary>
    /// Проверяет если баф существует
    /// </summary>
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

    /// <summary>
    /// Возврощает ячейку или null
    /// </summary>
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
    /// <summary>
    /// Проверяет если бафф существует по типу бафа в ячейках баффов или дебаффов
    /// <br>isBuff = true - поиск в листе бафов </br>
    /// <br>false - дебафов</br>
    /// </summary>
    public bool IsBuffExistByTypeName(BuffType buffType, bool isBuff = true) 
    {
        List<GameObject> buffs = (isBuff == true) ? buffCells : debuffCells;

        foreach (var cell in buffs)
        {
            BuffCell bcell = cell.GetComponent<BuffCell>();

            if (bcell.buffType == buffType) 
            {
                return true;
            }
        }

        return false;
    }
}
