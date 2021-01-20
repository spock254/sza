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
    Sprite bodySprite = null;
    Tile bodyTile = null;
    
    [SerializeField]
    Sprite upperSprite = null;
    Tile upperTile = null;

    [SerializeField]
    TileAnim tileAnim;

    void Awake()
    {
        controller = Global.Component.GetController();

        bodyTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
        bodyTile.sprite = bodySprite;
        upperTile = ScriptableObject.CreateInstance(typeof(Tile)) as Tile;
        upperTile.sprite = upperSprite;

        upper = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER);
        upper2 = Global.TileMaps.GetTileMap(Global.TileMaps.UPPER_2);

        upper.SetTile(upper.WorldToCell(transform.position), bodyTile);
        upper2.SetTile(upper2.WorldToCell(transform.position), upperTile);
        
        tileAnim.Init(this, upper2);
    }

    public void SwitchItem(Item itemToSwitch, Button hand) 
    {
        if (itemToSwitch.IsSameItems(needItem)) 
        {
            controller.SetDefaultItem(hand);
            Item resultItemClone = Instantiate(resultItem);
            
            Object[] args = new Object[1];
            args[0] = resultItemClone;

            tileAnim.StartAnim(FinalAction, args);
            //StartCoroutine(Action(resultItemClone));
        }
    }

    void Test(Object[] args)
    {
        Debug.Log("Finish");
    }
    // IEnumerator Action(Item item)
    // {
    //     foreach (var tile in actionTiles)
    //     {
    //         upper2.SetTile(upper2.WorldToCell(transform.position), tile);
    //         yield return new WaitForSeconds(actionFrameTime);
    //     }

    //     upper2.SetTile(upper2.WorldToCell(transform.position), upperTile);

    //     GameObject prefClone = Instantiate(pref, dropPoint.position, Quaternion.identity);
    //     prefClone.GetComponent<ItemCell>().item = item;
    //     //prefClone.GetComponent<ItemCell>().item.itemBuff.buff = item.itemBuff.buff;
    //     prefClone.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
    //     prefClone.name += Global.DROPED_ITEM_PREFIX;
    // }

    void FinalAction(object[] args)
    {
        Item item = (Item) args[0];

        upper2.SetTile(upper2.WorldToCell(transform.position), upperTile);

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
        return name;
    }
}
