using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ItemSwitchController : MonoBehaviour, ISwitchItem
{
    Controller controller = null;

    [SerializeField]
    string objName = string.Empty;

    [SerializeField]
    Item resultItem = null;
    [SerializeField]
    Item needItem = null;

    [SerializeField]
    GameObject pref = null;
    [SerializeField]
    Transform dropPoint = null;

    Tilemap upper;
    Tilemap upper2;

    [SerializeField]
    StaticTiles baseTiles = null;

    [SerializeField]
    TileAnim tileAnim;

    void Awake()
    {
        controller = Global.Component.GetController();

        baseTiles.Init();

        upper = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER);
        upper2 = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER_2);

        upper.SetTile(upper.WorldToCell(transform.position), baseTiles.GetBackTile());
        upper2.SetTile(upper2.WorldToCell(transform.position), baseTiles.GetFrontTile());
        
        tileAnim.Init(this, upper2);
    }

    public void SwitchItem(Item itemToSwitch, Button hand) 
    {
        if (itemToSwitch.IsSameItems(needItem)) 
        {
            controller.SetDefaultItem(hand);
            Item resultItemClone = Instantiate(resultItem);
            
            tileAnim.StartAnim(FinalAction, new Object[]{ resultItem });
        }
    }

    void FinalAction(object[] args)
    {
        Item item = (Item) args[0];

        upper2.SetTile(upper2.WorldToCell(transform.position), baseTiles.GetFrontTile());

        GameObject prefClone = Instantiate(pref, dropPoint.position, Quaternion.identity);
        prefClone.GetComponent<ItemCell>().item = item;
        //prefClone.GetComponent<ItemCell>().item.itemBuff.buff = item.itemBuff.buff;
        prefClone.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        prefClone.name += Global.DROPED_ITEM_PREFIX;
    }

    public Item GetNeedItem() 
    {
        return needItem;
    }

    public string GetISwitchName()
    {
        return objName;
    }
}
