using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PrinterController : MonoBehaviour
{
    public Item paper;
    public Item itemToPrint;
    public GameObject itemPref;

    CaseItem caseItem;

    public List<Tile> printsTiles = null;
    public float printsWorkUnit = 0.1f;

    void Start()
    {
        caseItem = GetComponent<CaseItem>();    
    }

    public void OnPrinterClick() 
    {

        if (itemToPrint && isPaperInside()) 
        {
            StartCoroutine(Print());
            //Debug.Log("print");

            //caseItem.items.Remove(paper);
            //itemPref.GetComponent<ItemCell>().item = itemToPrint;
            //itemPref.GetComponent<SpriteRenderer>().sprite = itemToPrint.itemSprite;
            //itemPref.name = "item_" + itemPref.name;

            //Instantiate(itemPref, transform.position, Quaternion.identity);

            ////itemToPrint = null;
        }
    }

    public bool isPaperInside() 
    {
        return caseItem.items.Contains(paper);
    }

    public IEnumerator Print() 
    {
        Tilemap tilemap = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER);
        Vector3Int cell = tilemap.WorldToCell(transform.position);

        foreach (var tile in printsTiles)
        {
            tilemap.SetTile(cell, tile);

            yield return new WaitForSeconds(printsWorkUnit);
        }

        caseItem.items.Remove(paper);
        itemPref.GetComponent<ItemCell>().item = itemToPrint;
        itemPref.GetComponent<SpriteRenderer>().sprite = itemToPrint.itemSprite;
        itemPref.name = "item_" + itemPref.name;

        Instantiate(itemPref, transform.position, Quaternion.identity);

    }
}
