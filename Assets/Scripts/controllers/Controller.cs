using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Controller : MonoBehaviour //, IPointerClickHandler
{
    [Header("Init")]
    public StatInit statInit;
    public FightStatsInit fightStatsInit;

    public ItemInit itemDB;
    public InventoryInit inventoryInit;
    public FoodInit foodInit;
    public EquipmentInit equipmentInit;
    public BagInit bagInit;

    [Header("Buttons")]
    public Button head_btn;
    public Button face_btn;
    public Button body_btn;
    public Button arm_btn;
    public Button lags_btn;
    public Button bag_btn;
    public Button left_hand_btn;
    public Button right_hand_btn;
    public Button left_pack_btn;
    public Button right_pack_btn;
    public Button card_btn;

    [Header("Bag cells")]
    public Button bagCell1;
    public Button bagCell2;
    public Button bagCell3;
    public Button bagCell4;
    public Button bagCell5;
    public Button bagCell6;
    public Button bagCell7;
    public Button bagCell8;
    public Button bagCell9;
    public Button bagCell10;

    public GameObject bag_panel;
    
    public bool isBagOpen = false;

    bool isLeftHand = true;
    
    public Button currentHand;

    [Header("Player data")]
    public float actioPlayerRadius;
    public Transform player;

    [HideInInspector]
    public Vector3 mousePos;
    [HideInInspector]
    public Vector3 mousePosRight;

    // Events
    public EventController eventController;


    void Start()
    {
        InitCells();

        currentHand = left_hand_btn;
        //DressCell(right_hand_btn, itemDB.items[1]);
        //DressCell(left_pack_btn, itemDB.items[3]);
        //DressCell(bag_btn, itemDB.items[2]);

        DressCell(head_btn, equipmentInit.equipmentDB["head"]);
        DressCell(left_hand_btn, foodInit.foodDB["apple"]);
        DressCell(bag_btn, bagInit.bagDB["bag"]);

        equipmentInit.equipmentDB["head"].itemUseData.use.Use_To_Ware(fightStatsInit.fightStats, statInit.stats, equipmentInit.equipmentDB["head"]);
        bagInit.bagDB["bag"].itemUseData.use.Use_To_Ware(fightStatsInit.fightStats, statInit.stats, bagInit.bagDB["bag"]);
    }


    // Update is called once per frame
    void Update()
    {

        if (Input.mouseScrollDelta.y != 0)
        {
            currentHand = SwapActiveHand();
            currentHand.GetComponentInChildren<Text>().text = "*";
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            mousePosRight = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePosRight.x, mousePosRight.y);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            eventController.OnRightButtonClickEvent.Invoke(hits, mousePos2D);

        }

        if (Input.GetMouseButtonDown(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            foreach (var hit in hits)
            {
                if (hit.collider != null && IsInActionRadius(mousePos, player.position, actioPlayerRadius))
                {
                    if (hit.collider.gameObject.tag == "player")
                    {
                        Item item = currentHand.GetComponent<ItemCell>().item;
                        item.itemUseData.use.Use_On_Player(statInit.stats, item);
                        SetDefaultItem(currentHand);
                    }

                    if (hit.collider.gameObject.tag == "door") 
                    {
                        Item itemInHand = GetItemInHand(currentHand);
                        // использовать айтем как ключ
                        eventController.OnDoorEvent.Invoke(itemInHand, mousePos, hit.collider, hit.collider.GetComponent<DoorController>().isLocked);
                        itemInHand.itemUseData.use.Use_To_Open(statInit.stats, itemInHand);
                    }

                    if (hit.collider.gameObject.tag == "case")
                    {
                        CaseItem caseItem = hit.collider.GetComponent<CaseItem>();
                        Transform casePosition = hit.collider.transform;
                        eventController.OnStaticCaseItemEvent.Invoke(caseItem, casePosition);
                    }

                    // ели на полу айтем и в руках не чего нет
                    if (hit.collider.name.Contains(Global.DROPED_ITEM_PREFIX)
                    && IsEmpty(currentHand))
                    {
                        GameObject itemGo = hit.collider.gameObject;
                        ItemPickUp(itemGo);
                    }
                }
            }
        }
    }

    public void OnBagButtonClick(string bagCellIndex)
    {
        GameObject bagCellGo = GameObject.FindGameObjectWithTag(bagCellIndex);
        Button bagCellBtn = bagCellGo.GetComponent<Button>();

        Item bagItem = bagCellGo.GetComponent<ItemCell>().item;
        Item handItem = currentHand.GetComponent<ItemCell>().item;
        Item bag = GetAnotherHand().GetComponent<ItemCell>().item;

        // избежание добавления сумки в ту же сумку
        if (IsEmpty(GetAnotherHand()))
        {
            return;
        }
        
        // если абгрейдабл айтем можно добавлять только абгрейты
        //в обычную сумку можно добавлять любые предметы
        if (IsItemTypePresent(bag, ItemUseData.ItemType.Openable) 
            || ((IsItemTypePresent(bag, ItemUseData.ItemType.Upgradable) 
            && IsItemTypePresent(handItem, ItemUseData.ItemType.Upgrate)) || IsEmpty(currentHand))) 
        { 
            if (IsEmpty(currentHand) && !IsEmpty(bagCellBtn))
            {
                bag.innerItems.Remove(bagItem);
                DressCell(currentHand, bagItem);
                SetDefaultItem(bagCellBtn);
            }
            else if (!IsEmpty(currentHand) && IsEmpty(bagCellBtn)) //TODO: проверить если достаточно места в сумке 
            {

                // если достаточно места в сумке для добавления
                 if (bag.CountInnerCapacity() + handItem.GetItemSize() <= bag.capacity) 
                 { 
                     bag.innerItems.Add(handItem);
                     DressCell(bagCellBtn, handItem);
                     SetDefaultItem(currentHand);
                }
            }
            else if (!IsEmpty(currentHand) && !IsEmpty(bagCellBtn))
            {
                // если достаточно места в сумке для свапа
                if (bag.CountInnerCapacity() - bagItem.GetItemSize() + handItem.GetItemSize() <= bag.capacity)
                {
                    bag.innerItems.Add(handItem);
                    bag.innerItems.Remove(bagItem);
                    DressCell(currentHand, bagItem);
                    DressCell(bagCellBtn, handItem);
                }
            }
        }

        Debug.Log(bag.CountInnerCapacity() +" / " + bag.capacity);
    }

    public void OnInvButtonClick(string itemType) 
    { 
        GameObject cellGo = GameObject.FindGameObjectWithTag(itemType.ToString()
                                    .ToLower() + "_cell");

        Button cell = cellGo.GetComponent<Button>();

        if (!IsEmpty(currentHand)) //если в руке что то есть
        {
            Item itemInHand = currentHand.GetComponent<ItemCell>().item;

            if (IsEmpty(cell))  //если не чего не надето
            {
                // если сумка открыта, тогда закрыть
                foreach (var item_types in itemInHand.itemUseData.itemTypes)
                {
                    Debug.Log("close");
                    if ((item_types == ItemUseData.ItemType.Openable 
                      || item_types == ItemUseData.ItemType.Upgradable) && isBagOpen)
                    {
                        CloseOpenContainer(bag_panel, ref isBagOpen);
                    }
                }

                foreach (var item_types in itemInHand.itemUseData.itemTypes)
                {
                    if (isSameTypes(itemType, item_types.ToString()))
                    {
                        DressOrTakeOff(cell, currentHand, itemInHand, true);
                        return;
                    }
                }
            }
            else //если одето 
            { 
            
            }
        }
        else //если в руках не чего нет
        {
            Item itemInCell = cell.GetComponent<ItemCell>().item;

            if (cell == GetAnotherHand()) // если взаимодействуем со второй рукой
            {
                if (!IsEmpty(GetAnotherHand())) // если вторая рука занята
                {
                    foreach (var item_type in itemInCell.itemUseData.itemTypes)
                    {
                        if (item_type == ItemUseData.ItemType.HandUsable)
                        {
                            itemInCell.itemUseData.use.Use_In_Hands(statInit.stats, itemInCell);
                            return;
                        }
                        else if (item_type == ItemUseData.ItemType.Openable 
                              || item_type == ItemUseData.ItemType.Upgradable) 
                        {
                            // TODO:  
                            if (!isBagOpen) 
                            { 
                                itemInCell.itemUseData.use.Use_To_Open(statInit.stats, itemInCell);
                            }

                            CloseOpenContainer(bag_panel, ref isBagOpen);

                            if (isBagOpen) 
                            {
                                ContainerContentInit(itemInCell.innerItems, bag_panel);
                                Debug.Log("bag init");
                            }

                            return;
                        }
                    }
                }
                else // если вторая рука пустая
                {

                }
                return;
            }

            if (!IsEmpty(cell)) //если одето
            {
                DressOrTakeOff(currentHand, cell, itemInCell, false);
            }
            else 
            {
                itemInCell.itemUseData.use.Use_When_Ware(fightStatsInit.fightStats, statInit.stats, itemInCell);
            }
        }
    }

    public void ItemPickUp(GameObject itemGo) 
    {
        Item item = itemGo.GetComponent<ItemCell>().item;

        DressCell(currentHand, item);

        Destroy(itemGo);
    }

    //когда не чего не надето
    public void SetDefaultItem(Button cell) 
    {
        Item deffaultItem = inventoryInit.inventoryDefaultDB[cell.name.ToLower()];
        DressCell(cell, deffaultItem);
    }

    void InitCells() 
    {
        head_btn.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["head"];
        face_btn.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["face"];
        body_btn.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["body"];
        lags_btn.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["lags"];
        arm_btn.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["arm"];
        bag_btn.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["bag"];
        left_hand_btn.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["left_hand"];
        right_hand_btn.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["right_hand"];
        left_pack_btn.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["packet_left"];
        right_pack_btn.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["packet_right"];
        card_btn.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["card"];

        bagCell1.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["1"];
        bagCell2.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["2"];
        bagCell3.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["3"];
        bagCell4.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["4"];
        bagCell5.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["5"];
        bagCell6.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["6"];
        bagCell7.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["7"];
        bagCell8.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["8"];
        bagCell9.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["9"];
        bagCell10.GetComponent<ItemCell>().item = inventoryInit.inventoryDefaultDB["10"];
    }

    /*                                  */
    /*              ACTIONS             */
    /*                                  */
    public List<Item> GetInnerItems()
    {
        return GetAnotherHand().GetComponent<ItemCell>().item.innerItems;
    }

    public void DressCell(Button cellToDress, Item item) 
    {
        cellToDress.GetComponent<ItemCell>().item = item;
        cellToDress.GetComponent<Image>().sprite = item.itemSprite;
    }

    void DressOrTakeOff(Button dressOn, Button takeOff, Item item, bool isDressing) 
    {
        DressCell(dressOn, item);

        SetDefaultItem(takeOff);

        if (isDressing)
        {
            item.itemUseData.use.Use_To_Ware(fightStatsInit.fightStats, statInit.stats, item);
        }
        else 
        {
            item.itemUseData.use.Use_To_TakeOff(fightStatsInit.fightStats, statInit.stats, item);
        }
    }
    public bool IsEmpty(Button button) 
    {
        return button.GetComponent<ItemCell>().item == inventoryInit.inventoryDefaultDB[button.name.ToLower()];
    }
    public Button GetAnotherHand() 
    {
        if (currentHand == left_hand_btn) 
        {
            return right_hand_btn;
        }

        return left_hand_btn;
    }

    public bool IsInActionRadius(Vector2 mousePos2D, Vector2 objPosition, float radius) 
    {
        return Vector2.Distance(mousePos2D, player.position) < actioPlayerRadius;
    }

    public bool IsInActionRadius() 
    {
        return Vector2.Distance(mousePos, player.position) < actioPlayerRadius;
    }

    bool isSameTypes(string t1, string t2) 
    {
        return t1.ToLower() == t2.ToLower();
    }

    public void CloseOpenContainer(GameObject panel, ref bool isOpen) 
    {
        isOpen = !isOpen;
        panel.SetActive(isOpen);
    }
    Button SwapActiveHand() 
    {
        isLeftHand = !isLeftHand;
        currentHand.GetComponentInChildren<Text>().text = " ";
        return isLeftHand ? left_hand_btn : right_hand_btn;
    }

    public void ContainerContentInit(List<Item> innerItems, GameObject panel) 
    {
        Button[] cells = panel.GetComponentsInChildren<Button>();
        int i = 0;

        for (; i < innerItems.Count; i++)
        {
            DressCell(cells[i], innerItems[i]);
        }

        //if (innerItems.Count < cells.Length) 
        //{
        //    i++;
        //    SetDefaultItem(cells[i]);
        //}

        for (; i < cells.Length; i++)
        {
            SetDefaultItem(cells[i]);
            //cells[i].gameObject.SetActive(false);
        }
        //RectTransform rt = bag_panel.GetComponent<RectTransform>();
        //rt.sizeDelta = new Vector2(rt.sizeDelta.x, 50 * i);
    }

    bool IsItemTypePresent(Item item, ItemUseData.ItemType type_to_find) 
    {
        foreach (var item_type in item.itemUseData.itemTypes)
        {
            if (item_type == type_to_find)
            {
                return true;
            }
        }

        return false;
    }

    public Item GetItemInHand(Button hand) 
    {
        return hand.GetComponent<ItemCell>().item;
    }
}
