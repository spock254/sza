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
        
    }

    void AddBuff(Item item) 
    {
        //find if buff exist
        
        //if not exist
        GameObject freeCell = FindFreeBuffCell();

        // когда все ячейки для баффа заняты
        if (freeCell == null) 
        {
            return;
        }
        
        freeCell.SetActive(true);
        freeCell.GetComponent<Image>().sprite = item.itemBuff.buff.buffSprite;
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

    //IEnumerator 
}
