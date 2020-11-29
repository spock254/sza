using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_DATA_spawnItem : NPC_BaseData
{
    public Transform spawnPoint;
    public float spawnTime = 0;
    public StateTypes whereItemSaved;
    public GameObject itemToSpawnPref;
    public bool isItemPickable = true;

    List<Item> GetSavedItems() 
    {
        NPC_BaseData com = (NPC_BaseData)GetComponent(System.Type.GetType(whereItemSaved.ToString().Replace("STATE", "DATA")));
        return com.savedItems;
    }

    public void SpawnSavedItems() 
    {
        StartCoroutine(DropItems(GetSavedItems()));
    }

    IEnumerator DropItems(List<Item> items)
    {
        foreach (var item in items)
        {
            Item itemToSpawn = Instantiate(item);

            GameObject spawnedItem = Instantiate(itemToSpawnPref, spawnPoint.position, Quaternion.identity);
            spawnedItem.GetComponent<ItemCell>().item = itemToSpawn;
            spawnedItem.GetComponent<ItemCell>().item.itemTimeflowModify.tics = itemToSpawn.itemTimeflowModify.tics;
            spawnedItem.GetComponent<SpriteRenderer>().sprite = itemToSpawn.itemSprite;
            spawnedItem.name = ((isItemPickable == true) ? Global.DROPED_ITEM_PREFIX : Global.UNPICKABLE_ITEM)
                               + itemToSpawn.name; 

            yield return new WaitForSeconds(spawnTime);
        }

    }
}
