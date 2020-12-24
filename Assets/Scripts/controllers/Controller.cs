using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class Controller : MonoBehaviour //, IPointerClickHandler
{
    static Controller instance;

    [Header("Init")]
    public StatInit statInit;
    public FightStatsInit fightStatsInit;

    public InventoryInit inventoryInit;
    public ItemInInventoryInit itemInInventoryInit;

    [Header("Eq cells")]
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

    [Header("Inv cells")]
    public Button invCell1;
    public Button invCell2;
    public Button invCell3;
    public Button invCell4;
    public Button invCell5;
    public Button invCell6;
    public Button invCell7;
    public Button invCell8;
    public Button invCell9;
    public Button invCell10;

    public List<Button> bagCellList = new List<Button>();
    public List<Button> invCellList = new List<Button>();
    public List<Button> cellList = new List<Button>();

    public GameObject bag_panel;
    
    public bool isBagOpen = false;
    public bool isBagOpenWithTab = false;

    bool isLeftHand = true;
    
    public Button currentHand;

    [Header("Player data")]
    [SerializeField]
    float actioPlayerRadius = 0;
    public Transform player;
    PlayerMovement playerMovement;

    [HideInInspector]
    public Vector3 mousePos;
    [HideInInspector]
    public Vector3 mousePosRight;

    [Header("controllers")]
    public EventController eventController;
    public CraftController craftController;

    ActionWindowController actionWindow;
    DialogueManager dialogWindow;
    ActionPanelController actionPanel;
    TerminalController terminalController; // не детектить нажатие пробела когда терминал открыт
    void Start()
    {
        instance = this;
        //DontDestroyOnLoad(transform.gameObject);
        // еслт какое-то окно активно, запретить управление
        actionWindow = Global.Component.GetActionWindowController();
        dialogWindow = Global.Component.GetDialogueManager();
        actionPanel = Global.Component.GetActionPanelController();
        playerMovement = Global.Obj.GetPlayerGameObject().GetComponent<PlayerMovement>();
        terminalController = Global.Component.GetTerminalController();

        InitCells();
        InitItemInInventory();
        currentHand = left_hand_btn;
        
        SetHandColor();
        // отресовка всех одетых вещей
        UpdateAllEqupment();

        SetBagCellList();
        SetSellList();
        SetInvCellList();

    }

    void SetBagCellList() 
    {
        bagCellList.Add(bagCell1);
        bagCellList.Add(bagCell2);
        bagCellList.Add(bagCell3);
        bagCellList.Add(bagCell4);
        bagCellList.Add(bagCell5);
        bagCellList.Add(bagCell6);
        bagCellList.Add(bagCell7);
        bagCellList.Add(bagCell8);
        bagCellList.Add(bagCell9);
        bagCellList.Add(bagCell10);
    }
    void SetInvCellList()
    {
        invCellList.Add(invCell1);
        invCellList.Add(invCell2);
        invCellList.Add(invCell3);
        invCellList.Add(invCell4);
        invCellList.Add(invCell5);
        invCellList.Add(invCell6);
        invCellList.Add(invCell7);
        invCellList.Add(invCell8);
        invCellList.Add(invCell9);
        invCellList.Add(invCell10);
    }
    void SetSellList() 
    {
        cellList.Add(head_btn);
        cellList.Add(face_btn);
        cellList.Add(body_btn);
        cellList.Add(arm_btn);
        cellList.Add(lags_btn);
        cellList.Add(bag_btn);
        cellList.Add(left_hand_btn);
        cellList.Add(right_hand_btn);
        cellList.Add(left_pack_btn);
        cellList.Add(right_pack_btn);
        cellList.Add(card_btn);
    }
    void SetHandColor() 
    {
        currentHand.GetComponent<Image>().color = Color.green;
        GetAnotherHand().GetComponent<Image>().color = Color.white;
    }

    bool isOptMouseInput = false;
    void Update()
    {
        if (dialogWindow.isOpen == false) 
        {
            if (Input.mouseScrollDelta.y != 0)      //switch hands
            {
                currentHand = SwapActiveHand();
                SetHandColor();
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                actionPanel.OnDropClick();
            }
            //else if (Input.GetKeyDown(KeyCode.Q))   //switch to left hand
            //{
            //    currentHand = left_hand_btn;
            //    SetHandColor();
            //}
            //else if (Input.GetKeyDown(KeyCode.E))   //switch to right hand
            //{
            //    currentHand = right_hand_btn;
            //    SetHandColor();
            //}
            else if (Input.GetKeyDown(KeyCode.Tab)) // open bag
            {
                if (isBagOpen == true)
                {

                    CloseOpenContainer(bag_panel, ref isBagOpen);
                    return;
                }

                if (IsEmpty(bag_btn) == true)
                {
                    return;
                }

                Item bag = bag_btn.GetComponent<ItemCell>().item;

                CloseOpenContainer(bag_panel, ref isBagOpen);

                if (isBagOpen == true)
                {
                    isBagOpenWithTab = true;
                    ContainerContentInit(bag.innerItems, bag_panel);
                }
            }

            if (Input.GetMouseButtonDown(1)) 
            {
                eventController.OnMouseClickEvent.Invoke();

                mousePosRight = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePosRight.x, mousePosRight.y);
                RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

                foreach (var hit in hits)
                {
                    if (hit.collider.name.Contains(Global.DROPED_ITEM_PREFIX) && IsInActionRadius(hit.transform.position, player.position, actioPlayerRadius)) 
                    {
                        Item itemInWorld = hit.collider.GetComponent<ItemCell>().item;
                        if (itemInWorld.itemSubstitution.IsUsable(GetItemInHand(currentHand))) 
                        {
                            itemInWorld.itemSubstitution.Substitute(hit.collider.gameObject);

                            return;
                        }
                            
                    }

                    if (hit.collider.tag == "substitudeItem") 
                    {
                        Item itemToDrop = hit.collider.GetComponent<SubstitudeCell>().item;
                        
                        actionPanel.SpawnItem(hit.transform.position, itemToDrop);
                        Destroy(hit.collider.gameObject);
                        
                        return;
                    }

                    if (hit.collider.gameObject.tag == "table" && hit.collider.GetComponent<TableController>().isCraftTable) 
                    {
                        bool removeTool = craftController.Craft_Table(hits, GetItemInHand(currentHand), 
                                                                            CraftType.Cooking, 
                                                                            CraftTable.Table);

                        if (removeTool) 
                        {
                            SetDefaultItem(currentHand);
                        }

                        return;
                    }

                    if (hit.collider.gameObject.tag == "microwave")
                    {
                        MicrowaveController microwave = hit.collider.GetComponent<MicrowaveController>();

                        craftController.Craft_Microwave(microwave, GetItemInHand(currentHand), 
                                                                   CraftType.Cooking, 
                                                                   CraftTable.Microwave);

                        eventController.OnEnvChangeShapeEvent.Invoke();

                        return;
                    }
                
                    if (hit.collider.gameObject.tag == "oven")
                    {
                        MicrowaveController microwave = hit.collider.GetComponent<MicrowaveController>();

                        craftController.Craft_Microwave(microwave, GetItemInHand(currentHand), 
                                                                   CraftType.Cooking, 
                                                                   CraftTable.Oven);

                        eventController.OnEnvChangeShapeEvent.Invoke();

                        return;
                    }

                    if (hit.collider.gameObject.tag == "printer") 
                    { 
                        PrinterController printerController = hit.collider.GetComponent<PrinterController>();

                        printerController.OnPrinterClick();
                    }

                    if (hit.collider.gameObject.tag == "pc") 
                    { 
                        PCController pcController = hit.collider.GetComponent<PCController>();

                        pcController.OnPc_Disck(currentHand);
                    }

                    if (hit.collider.gameObject.tag == "tv") 
                    {
                        TVController tvController = hit.collider.GetComponent<TVController>();

                        tvController.NextChanel();
                    }

                    if (hit.collider.gameObject.name.Contains(Global.ITEM_SWITCH_PREFIX)) 
                    {
                        ItemSwitchController itemSwitchController = hit.collider.GetComponent<ItemSwitchController>();

                        itemSwitchController.SwitchItem(GetItemInHand(currentHand), currentHand);
                    }

                    /*                  */
                    /*      QUESTS      */
                    /*                  */

                    if (hit.collider.gameObject.name.Contains("bus_spawn"))
                    {
                        Debug.Log("bus");
                        BusController bus = hit.collider.GetComponent<BusController>();
                        bus.GiveTicket(currentHand);
                        return;
                    }
                }

                eventController.OnRightButtonClickEvent.Invoke(hits, mousePos2D);

            }

            if (Input.GetMouseButtonDown(0) || ((isOptMouseInput = Input.GetKeyDown(KeyCode.E)) 
                                                     && terminalController.isOpen == false))
            {
                eventController.OnMouseClickEvent.Invoke();
                // не детектить нажатие пробела вовремя работы в терминале
                //isOptMouseInput = !terminalController.isOpen;   

                if (isOptMouseInput == false)
                {
                    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                else
                {
                    mousePos = player.position + (playerMovement.GetTurnSide() / 2);
                }

                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

                //isSpacePressed = false;

                if (IsInActionRadius(mousePos, player.position, actioPlayerRadius)) 
                {
                    Item itemInHand = GetItemInHand(currentHand);
                    itemInHand.itemUseData.use.Use_On_Env(hits, mousePos, currentHand, GetAnotherHand());
                }

                foreach (var hit in hits)
                {

                    if (hit.collider != null && IsInActionRadius(mousePos, player.position, actioPlayerRadius))
                    {

                        // ели на полу айтем и в руках не чего нет
                        if (hit.collider.name.Contains(Global.DROPED_ITEM_PREFIX)
                        && IsEmpty(currentHand))
                        {
                            GameObject itemGo = hit.collider.gameObject;
                            ItemPickUp(itemGo);
                            return; // приоритет что бы не взять айтем и не положить его потом на стол если он был уже на столе
                        }

                        if (hit.collider.gameObject.tag == "player")
                        {
                            Item item = currentHand.GetComponent<ItemCell>().item;
                            item.itemUseData.use.Use_On_Player(statInit.stats, item);

                            if (item.isDestroyOnPlayerUse) 
                            { 
                                SetDefaultItem(currentHand);
                            }
                            else 
                            {
                                if (item.afterOnPlayerUseItem != null) 
                                {
                                    Item afterUseItemClone = Instantiate(item.afterOnPlayerUseItem);
                                    DressCell(currentHand, afterUseItemClone);
                                }
                            }
                        }
                       
                        if (hit.collider.tag == "substitudeItem")
                        {
                            BaseConection baseConection = hit.collider.GetComponent<BaseConection>();

                            if (baseConection != null)
                            {
                                PCController pcController = baseConection.FindPcInRadius();

                                if (pcController != null) 
                                { 
                                    baseConection.ProcessConection(pcController);
                                
                                }

                                return;
                            }
                        }

                        if (hit.collider.gameObject.tag == "tapWater")
                        {
                            TabWaterController tabWaterController = hit.collider.GetComponent<TabWaterController>();
                            Button btn_itemInHand = IsEmpty(currentHand) ? null : currentHand;
                            tabWaterController.OnWaterTap_Click(btn_itemInHand);
                        }

                        if (hit.collider.gameObject.tag == "envObj") 
                        { 
                            BaseActionWindowConntroller baseAction = hit.collider.GetComponent<BaseActionWindowConntroller>();
                            baseAction.Open(hit.collider.gameObject);
                        }

                        if (hit.collider.gameObject.tag == "pc") 
                        {
                            PCController pcController = hit.collider.GetComponent<PCController>();
                            Item itemInHand = IsEmpty(currentHand) ? null : GetItemInHand(currentHand);
                            pcController.OnPc_ClicK(itemInHand, mousePos);
                        }

                        if (hit.collider.gameObject.tag == "tv") 
                        { 
                            TVController tVController = hit.collider.GetComponent<TVController>();
                            tVController.OnTvClick();
                        }

                        if (hit.collider.gameObject.tag == "microwave" || hit.collider.gameObject.tag == "oven") 
                        {
                            Item itemInHand = IsEmpty(currentHand) ? null : GetItemInHand(currentHand);
                            MicrowaveController microwaveController = hit.collider.GetComponent<MicrowaveController>();

                            MicrowaveController.MicrowaveStatus status = microwaveController.OnMicrowaveClick(itemInHand, mousePos);

                            if (status == MicrowaveController.MicrowaveStatus.PutItem) 
                            {
                                SetDefaultItem(currentHand);
                            }
                            else if (status == MicrowaveController.MicrowaveStatus.TakeItem) 
                            {
                                DressCell(currentHand, microwaveController.itemInside);
                                microwaveController.itemInside = null;
                            }

                            eventController.OnEnvChangeShapeEvent.Invoke();
                        }

                        if (hit.collider.gameObject.tag == "door") 
                        {
                            Item itemInHand = GetItemInHand(currentHand);
                            // использовать айтем как ключ
                            //eventController.OnDoorEvent.Invoke(itemInHand, mousePos, hit.collider, hit.collider.GetComponent<DoorController>().isLocked);
                            hit.collider.GetComponent<DoorController>().OnDoorClick(itemInHand, mousePos, hit.collider, hit.collider.GetComponent<DoorController>().isLocked);
                            itemInHand.itemUseData.use.Use_To_Open(statInit.stats, itemInHand);
                        }

                        if (hit.collider.gameObject.tag == "case" || hit.collider.gameObject.tag == "printer")
                        {
                            CaseItem caseItem = hit.collider.GetComponent<CaseItem>();
                            Transform casePosition = hit.collider.transform;

                            // важно соблюдать очередность, сначало открывается сундук потом панэль
                            hit.collider.GetComponent<CaseController>().OnCaseCloseOpen(casePosition.position);
                            
                            eventController.OnStaticCaseItemEvent.Invoke(caseItem, casePosition);
                            eventController.OnEnvChangeShapeEvent.Invoke();
                        }

                        if (hit.collider.gameObject.tag == "table") 
                        {
                            hit.collider.GetComponent<TableController>().OnTableClick(hit.transform.position,
                                IsEmpty(currentHand) ? null : GetItemInHand(currentHand));
                        }

                        /*                  */
                        /*      QUESTS      */
                        /*                  */
                        if (hit.collider.gameObject.name.Contains("bus_spawn")) 
                        {
                            Debug.Log("bus");
                            BusController bus = hit.collider.GetComponent<BusController>();
                            bus.Activate();
                            return;
                        }
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
        Item bag = (isBagOpenWithTab == true) ? bag_btn.GetComponent<ItemCell>().item 
                                              : GetAnotherHand().GetComponent<ItemCell>().item;

        // избежание добавления сумки в ту же сумку
        if (IsEmpty(GetAnotherHand()) == true && isBagOpenWithTab == false)
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

            if (currentHand == cell) 
            { 
                craftController.Craft_OneHand(cell, itemInHand);
            }

            SwapItems(cell, itemType);

            if (IsEmpty(cell))  //если не чего не надето
            {
                // если сумка открыта, тогда закрыть
                foreach (var item_types in itemInHand.itemUseData.itemTypes)
                {
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

            // если две руки заняты попробовать скрафтить
            // и если заимод только с айтемом во второй руке
            Item itemInCell = cell.GetComponent<ItemCell>().item;

            if (!IsEmpty(GetAnotherHand()) && itemInCell == GetItemInHand(GetAnotherHand())) 
            {
                foreach (var item_type in itemInHand.itemUseData.itemTypes) 
                {
                    if (item_type == ItemUseData.ItemType.HandCraftable) 
                    {
                        craftController.Craft_Hands(currentHand.gameObject,
                                                    GetAnotherHand().gameObject);

                        return;
                    }
                }
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
                        else if (item_type == ItemUseData.ItemType.HandCraftable)
                        {
                            Debug.Log("CRAFT");
                            craftController.Craft_Hands(GetAnotherHand().gameObject,
                                                             currentHand.gameObject);

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

                            if (isBagOpen == true)
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

    /*                                  */
    /*              ACTIONS             */
    /*                                  */
    public void ItemPickUp(GameObject itemGo) 
    {
        Item item = itemGo.GetComponent<ItemCell>().item;
        
        DressCell(currentHand, Instantiate(item));

        Destroy(itemGo);
    }

    void SwapItems(Button cellToSwap, string itemType) 
    {
        Item itemInHand = GetItemInHand(currentHand);
        
        if (IsItemHasType(itemInHand, itemType) && !IsEmpty(cellToSwap)) 
        { 
            Item itemInCell = cellToSwap.GetComponent<ItemCell>().item;

            DressCell(currentHand, itemInCell);
            DressCell(cellToSwap, itemInHand);
        }

        UpdateAllEqupment();
        //Debug.Log(IsItemHasType(itemInHand, itemType) + " " + itemType);
    }

    bool IsItemHasType(Item item, string itemType) 
    {
        foreach (var type in item.itemUseData.itemTypes)
        {
            if (isSameTypes(type.ToString(), itemType)) 
            {
                return true;
            }
        }

        return false;
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

    void InitItemInInventory() 
    {
        if (itemInInventoryInit.head != null) 
        {
            DressCell(head_btn, Instantiate(itemInInventoryInit.head));
        }

        if (itemInInventoryInit.arm != null)
        {
            DressCell(arm_btn, Instantiate(itemInInventoryInit.arm));
        }

        if (itemInInventoryInit.face != null)
        {
            DressCell(face_btn, Instantiate(itemInInventoryInit.face));
        }

        if (itemInInventoryInit.lags != null)
        {
            DressCell(lags_btn, Instantiate(itemInInventoryInit.lags));
        }

        if (itemInInventoryInit.bag != null)
        {
            DressCell(bag_btn, Instantiate(itemInInventoryInit.bag));
        }

        if (itemInInventoryInit.body != null)
        {
            DressCell(body_btn, Instantiate(itemInInventoryInit.body));
        }

        if (itemInInventoryInit.left_hand != null)
        {
            DressCell(left_hand_btn, Instantiate(itemInInventoryInit.left_hand));
        }

        if (itemInInventoryInit.right_hand != null)
        {
            DressCell(right_hand_btn, Instantiate(itemInInventoryInit.right_hand));
        }

        if (itemInInventoryInit.left_pack != null)
        {
            DressCell(left_pack_btn, Instantiate(itemInInventoryInit.left_pack));
        }

        if (itemInInventoryInit.right_pack != null)
        {
            DressCell(right_pack_btn, Instantiate(itemInInventoryInit.right_pack));
        }

        if (itemInInventoryInit.card != null)
        {
            DressCell(card_btn, Instantiate(itemInInventoryInit.card));
        }
    }

    public List<Item> GetInnerItems()
    {
        return GetAnotherHand().GetComponent<ItemCell>().item.innerItems;
    }

    public void DressCell(Button cellToDress, Item item) 
    {
        cellToDress.GetComponent<ItemCell>().item = item;
        cellToDress.GetComponent<Image>().sprite = item.itemSprite;
        item.itemUseData.use.Use_DressedUp(cellToDress, item);
        item.itemEffect.currentCell = cellToDress;
        //AnimateItem(cellToDress, item);
    }

    void UpdateAllEqupment() 
    {
        List<Button> equpItems = new List<Button>();

        equpItems.Add(head_btn);
        equpItems.Add(face_btn);
        equpItems.Add(body_btn);
        equpItems.Add(arm_btn);
        equpItems.Add(lags_btn);
        equpItems.Add(bag_btn);

        foreach (var item in equpItems)
        {
            string _tag = GetCleanTag(item);
            string _itemName = item.GetComponent<ItemCell>().item.itemSprite.name;

            if (Enum.IsDefined(typeof(PlayerAnimation.SpritePart), _tag))
            {
                eventController.OnChangeSpriteEvent.Invoke(_itemName, ParseStringToPrt(_tag));
            }
        }
    }

    string GetCleanTag(Button part) 
    {
        return part.gameObject.tag.Remove(part.gameObject.tag.Length - "_cell".Length);
    }

    void DressOrTakeOff(Button dressOn, Button takeOff, Item item, bool isDressing) 
    {
        DressCell(dressOn, item);

        SetDefaultItem(takeOff);

        if (isDressing)
        {
            //TODO
            string bodyPart = GetCleanTag(dressOn);
            if (Enum.IsDefined(typeof(PlayerAnimation.SpritePart), bodyPart)) 
            {
                eventController.OnChangeSpriteEvent.Invoke(item.itemSprite.name, ParseStringToPrt(bodyPart));
            }

            item.itemUseData.use.Use_To_Ware(fightStatsInit.fightStats, statInit.stats, item);
        }
        else 
        {
            string bodyPart = GetCleanTag(takeOff);
            
            if (Enum.IsDefined(typeof(PlayerAnimation.SpritePart), bodyPart))
            {
                eventController.OnChangeSpriteEvent.Invoke(string.Empty, ParseStringToPrt(bodyPart));
            }

            item.itemUseData.use.Use_To_TakeOff(fightStatsInit.fightStats, statInit.stats, item);
        }
    }

    PlayerAnimation.SpritePart ParseStringToPrt(string part) 
    {
        if (part == PlayerAnimation.SpritePart.body.ToString()) 
        {
            return PlayerAnimation.SpritePart.body;
        }
        if (part == PlayerAnimation.SpritePart.lags.ToString()) 
        {
            return PlayerAnimation.SpritePart.lags;
        }
        if (part == PlayerAnimation.SpritePart.face.ToString()) 
        {
            return PlayerAnimation.SpritePart.face;
        }
        if (part == PlayerAnimation.SpritePart.head.ToString()) 
        {
            return PlayerAnimation.SpritePart.head;
        }
        if (part == PlayerAnimation.SpritePart.bag.ToString())
        {
            return PlayerAnimation.SpritePart.bag;
        }
        if (part == PlayerAnimation.SpritePart.arm.ToString())
        {
            return PlayerAnimation.SpritePart.arm;
        }

        return PlayerAnimation.SpritePart.None;
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

        if (isOpen == false) 
        {
            isBagOpenWithTab = false;
        }

        panel.SetActive(isOpen);
    }

    public void CloseContainer() 
    {
        isBagOpen = false;
        bag_panel.SetActive(false);
    }

    Button SwapActiveHand() 
    {
        isLeftHand = !isLeftHand;
        //currentHand.GetComponentInChildren<Text>().text = " ";
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

    public float GetActioPlayerRadius() 
    {
        return actioPlayerRadius;
    }

    IEnumerator AnimateCells(List<Button> cells) 
    {
        while (true) 
        {

            for (int i = 0; i < cells.Count; i++)
            {
                Item itemToAnimate = cells[i].GetComponent<ItemCell>().item;
                Image cellImg = cells[i].GetComponent<Image>();

                if (itemToAnimate == null) 
                {
                    continue;
                }

                if (itemToAnimate.itemAnimationData.itemSpriteFrames.Count == 0) 
                {
                    cellImg.sprite = itemToAnimate.itemSprite;
                }
                else 
                {
                    cellImg.sprite = itemToAnimate.itemAnimationData.GetNextFrameSprite();
                }
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    /* STATIC */
    public static float GetActioPlayerRadiusStatic() 
    {
        return instance.GetActioPlayerRadius();
    }

}
