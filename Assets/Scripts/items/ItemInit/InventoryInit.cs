using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInit : MonoBehaviour
{
    public Dictionary<string, Item> inventoryDefaultDB { get;  set; }

    [SerializeField] Item head = null;
    [SerializeField] Item face = null;
    [SerializeField] Item body = null;
    [SerializeField] Item arm = null;
    [SerializeField] Item lags = null;
    [SerializeField] Item bag = null;
    [SerializeField] Item left_hand = null;
    [SerializeField] Item right_hand = null;
    [SerializeField] Item left_pack = null;
    [SerializeField] Item right_pack = null;
    [SerializeField] Item card = null;
    [SerializeField] Item bagCell = null;

    void Awake()
    {
        inventoryDefaultDB = new Dictionary<string, Item>();

        inventoryDefaultDB.Add("head", head);
        inventoryDefaultDB.Add("body", body);
        inventoryDefaultDB.Add("face", face);
        inventoryDefaultDB.Add("arm",  arm);
        inventoryDefaultDB.Add("lags",  lags);
        inventoryDefaultDB.Add("bag",  bag);
        inventoryDefaultDB.Add("left_hand", left_hand);
        inventoryDefaultDB.Add("right_hand", right_hand);
        inventoryDefaultDB.Add("packet_left", left_pack);
        inventoryDefaultDB.Add("packet_right", right_pack);
        inventoryDefaultDB.Add("card", card);

        // инит для слотов сумки
        for (int i = 1; i < 11; i++)
        {
            inventoryDefaultDB.Add(i.ToString(), bagCell);
            inventoryDefaultDB[i.ToString()].itemName = i.ToString();
        }

        for (int i = 1; i < 11; i++)
        {
            inventoryDefaultDB.Add(i.ToString() + "c", bagCell);
            inventoryDefaultDB[i.ToString() + "c"].itemName = i.ToString() + "c";
        }
    }
}
