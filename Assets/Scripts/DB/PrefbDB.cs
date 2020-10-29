using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefbDB : MonoBehaviour
{
    [SerializeField]
    GameObject itemPrefab;

    public GameObject GetItemPrefab() 
    {
        this.itemPrefab.name = Global.DROPED_ITEM_PREFIX + this.itemPrefab.name;
        return this.itemPrefab;
    }

    public void InstantiateItemPref(Vector3 position) 
    {
        Instantiate(this.itemPrefab, position, Quaternion.identity);
    }
}
