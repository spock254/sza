using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrinterController : MonoBehaviour
{
    public Item paper;
    public Item itemToPrint;
    public GameObject itemPref;

    CaseItem caseItem;

    void Start()
    {
        caseItem = GetComponent<CaseItem>();    
    }

    public void OnPrinterClick() 
    {

        if (itemToPrint && isPaperInside()) 
        {
            Debug.Log("print");

            caseItem.items.Remove(paper);
            itemPref.GetComponent<ItemCell>().item = itemToPrint;
            itemPref.GetComponent<SpriteRenderer>().sprite = itemToPrint.itemSprite;
            itemPref.name = "item_" + itemPref.name;

            Instantiate(itemPref, transform.position, Quaternion.identity);

            //itemToPrint = null;
        }
    }

    public bool isPaperInside() 
    {
        return caseItem.items.Contains(paper);
    }
}
