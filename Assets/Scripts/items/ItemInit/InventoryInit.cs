using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryInit : MonoBehaviour
{
    public Dictionary<string, Item> inventoryDefaultDB { get;  set; }

    [SerializeField] Sprite head = null;
    [SerializeField] Sprite face = null;
    [SerializeField] Sprite body = null;
    [SerializeField] Sprite arm = null;
    [SerializeField] Sprite lags = null;
    [SerializeField] Sprite bag = null;
    [SerializeField] Sprite left_hand = null;
    [SerializeField] Sprite right_hand = null;
    [SerializeField] Sprite left_pack = null;
    [SerializeField] Sprite right_pack = null;
    [SerializeField] Sprite card = null;
    [SerializeField] Sprite bagCell = null;

    void Awake()
    {
        inventoryDefaultDB = new Dictionary<string, Item>();

        inventoryDefaultDB.Add("head", new Item("head", new ItemUseData(new UseHead()), head));
        inventoryDefaultDB.Add("body", new Item("body", new ItemUseData(new UseBody()), body));
        inventoryDefaultDB.Add("face", new Item("face", new ItemUseData(new UseFace()), face));
        inventoryDefaultDB.Add("arm", new Item("arm", new ItemUseData(new UseArm()), arm));
        inventoryDefaultDB.Add("lags", new Item("lags", new ItemUseData(new UseLags()), lags));
        inventoryDefaultDB.Add("bag", new Item("bag", new ItemUseData(new UseBag()), bag));
        inventoryDefaultDB.Add("left_hand", new Item("left_hand", new ItemUseData(new UseLeftHand()), left_hand));
        inventoryDefaultDB.Add("right_hand", new Item("right_hand", new ItemUseData(new UseRightHand()), right_hand));
        inventoryDefaultDB.Add("packet_left", new Item("packet_left", new ItemUseData(new UseLeftPack()), left_pack));
        inventoryDefaultDB.Add("packet_right", new Item("packet_right", new ItemUseData(new UseRightPack()), right_pack));
        inventoryDefaultDB.Add("card", new Item("card", new ItemUseData(new UseCard()), card));

        // инит для слотов сумки
        for (int i = 1; i < 11; i++)
        {
            inventoryDefaultDB.Add(i.ToString(), new Item(i.ToString(), new ItemUseData(new UseBagCell()), bagCell));
        }

        for (int i = 1; i < 11; i++)
        {
            inventoryDefaultDB.Add(i.ToString() + "c", new Item(i.ToString() + "c", new ItemUseData(new UseBagCell()), bagCell));
        }
    }
}
