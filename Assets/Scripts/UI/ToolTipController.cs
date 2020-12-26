using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipController : MonoBehaviour
{
    static ToolTipController instance;

    EventController eventController = null;

    [SerializeField]
    Camera uiCamera = null;

    [SerializeField]
    GameObject toolTip = null;

    [SerializeField]
    float textPaddinSize = 0f;

    [SerializeField]
    float actionDistanceCof = 1f;

    Image bgImage;
    Text textItemName;
    Text textInteraction;
    RectTransform bgRectTransform;
    RectTransform UIRectTransform;

    Transform playerTransform;
    Controller controller;

    bool isTooltipOpen = false;
    float preferredHeight = 0;
    Vector2 tooltipPosition;

    [SerializeField]
    GameObject reviewWindow = null;
    [SerializeField]
    GameObject itemDescription = null;

    RectTransform rtItemDescription = null;
    [SerializeField]
    RectTransform rtImage = null;
    [SerializeField]
    Text descriptionText = null;

    Vector2 pos = Vector2.zero;

    GameObject staticItemPanel = null;

    bool isdetected = false;
    void Start()
    {
        bgImage = toolTip.transform.GetChild(0).GetComponent<Image>();
        textItemName = bgImage.transform.GetChild(0).GetComponent<Text>();
        textInteraction = bgImage.transform.GetChild(1).GetComponent<Text>();
        bgRectTransform = toolTip.GetComponent<RectTransform>();
        UIRectTransform = Global.UIElement.GetUI().GetComponent<RectTransform>();
        playerTransform = Global.Obj.GetPlayerGameObject().GetComponent<Transform>();
        controller = Global.Component.GetController();

        instance = this;

        preferredHeight = textItemName.preferredHeight;

        HideToolTip();

        pos = itemDescription.GetComponent<RectTransform>().anchoredPosition;
        rtItemDescription = itemDescription.GetComponent<RectTransform>();
        
        eventController = Global.Component.GetEventController();
        eventController.OnCaseCloseEvent.AddListener(HideItemReview);
    }

    private void LateUpdate()
    {
        UpdateItemDescriptionPosition();
    }

    void FixedUpdate()
    {
        if (IsInActionRadius() == true)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            if (hits.Length == 0)
            {
                isdetected = false;
            }

            foreach (var hit in hits)
            {
                if (hit.collider.tag == "substitudeItem")
                {
                    BaseConection baseConection = hit.collider.GetComponent<BaseConection>();
                    NPC_Info npcInfo = hit.collider.GetComponent<NPC_Info>();
                    PCController pcController = baseConection.FindPcInRadius();

                    string interactionStr = Global.Tooltip.LM_INTERACT + " / " + Global.Tooltip.RM_TURN_OFF;

                    if (pcController != null)
                    {
                        bool isConnected = pcController.peripherals.Contains(hit.collider.gameObject);
                        interactionStr = ((isConnected == true) ? Global.Tooltip.LM_DISCONNECT : Global.Tooltip.LM_CONNECT) + " / " + Global.Tooltip.RM_TURN_OFF;
                    }

                    isdetected = true;
                    tooltipPosition = Camera.main.WorldToScreenPoint(hit.transform.position);
                    ShowToolTip(npcInfo == null ? "obj" : npcInfo.npcName, interactionStr);
                    TooltipLocate(tooltipPosition);
                    return;

                }
                else if (hit.collider.tag == "table")
                {
                    TableController tableController = hit.collider.GetComponent<TableController>();

                    isdetected = true;
                    tooltipPosition = Camera.main.WorldToScreenPoint(hit.collider.transform.position);

                    if (IsCurrentHandEmpty() && IsObjectExist(hits, Global.DROPED_ITEM_PREFIX, false) == false)
                    {
                        ShowToolTip((tableController.tableName == string.Empty) ? "table" : tableController.tableName,
                                PrintRed(Global.Tooltip.NO_ACTIONS));
                    }
                    else if (IsCurrentHandEmpty() && IsObjectExist(hits, Global.DROPED_ITEM_PREFIX, false) == true)
                    {
                        Item itemOnTable = FindRaycast(hits, Global.DROPED_ITEM_PREFIX, false)?.collider
                                                                         .GetComponent<ItemCell>().item;

                        ShowToolTip((tableController.tableName == string.Empty) ? "table" + " (" + itemOnTable.itemName + ")"
                                                              : tableController.tableName + " (" + itemOnTable.itemName + ")",
                                                                Global.Tooltip.LM_PICK_UP);
                    }
                    else if (IsCurrentHandEmpty() == false && IsObjectExist(hits, Global.DROPED_ITEM_PREFIX, false) == true)
                    {
                        Item tool = controller.GetItemInHand(controller.currentHand);
                        RaycastHit2D? itemOnTableHit = FindRaycast(hits, Global.DROPED_ITEM_PREFIX, false);
                        Item temOnTable = itemOnTableHit.GetValueOrDefault().collider.GetComponent<ItemCell>().item;

                        ItemCraftData craftData = CraftController.FindRecept_Static(tool, temOnTable,
                                                                                    CraftType.Cooking, /* TODO */
                                                                                    CraftTable.Table);

                        ShowToolTip((tableController.tableName == string.Empty) ? "table" + " (" + temOnTable.itemName + ")"
                                                              : tableController.tableName + " (" + temOnTable.itemName + ")",
                                Global.Tooltip.LM_PUT + ((tableController.isCraftTable) ? ((craftData != null) ?
                                " / " + Global.Tooltip.RM_CRAFT + " " + craftData.recept.craftResult.itemName : string.Empty)
                                : string.Empty));
                    }
                    else
                    {
                        ShowToolTip((tableController.tableName == string.Empty) ? "table" : tableController.tableName,
                                Global.Tooltip.LM_PUT);
                    }

                    TooltipLocate(tooltipPosition);

                    return;
                }
                else if (hit.collider.name.Contains(Global.DROPED_ITEM_PREFIX))
                {
                    bool onTable = false;

                    foreach (var hitTable in hits)
                    {
                        if (hitTable.collider.tag == "table")
                        {
                            onTable = true;
                        }
                    }

                    if (onTable == false)
                    {

                        Item item = hit.collider.GetComponent<ItemCell>().item;
                        Item itemInHand = controller.currentHand.GetComponent<ItemCell>().item;

                        isdetected = true;
                        tooltipPosition = Camera.main.WorldToScreenPoint(hit.collider.transform.position);

                        string useInteraction = (item.itemSubstitution.IsSubstituted() == true && item.itemSubstitution.IsItemToUseExist(itemInHand)) ? " / " + Global.Tooltip.RM_TURN_ON : string.Empty;

                        ShowToolTip(item.itemName, (controller.IsEmpty(controller.currentHand) == true) ?
                            Global.Tooltip.LM_PICK_UP + useInteraction
                            : ((useInteraction == string.Empty) ? PrintRed(Global.Tooltip.NO_ACTIONS) : useInteraction.Substring(" / ".Length)));
                        TooltipLocate(tooltipPosition);

                        return;
                    }
                }
                else if (hit.collider.tag == "case")
                {
                    CaseController caseController = hit.collider.GetComponent<CaseController>();

                    isdetected = true;
                    tooltipPosition = Camera.main.WorldToScreenPoint(hit.collider.transform.position);

                    ShowToolTip((caseController.caseName == string.Empty) ? "case" : caseController.caseName, (caseController.isOpen == false)
                                ? Global.Tooltip.LM_OPEN : Global.Tooltip.LM_CLOSE);
                    TooltipLocate(tooltipPosition);

                    return;
                }
                else if (hit.collider.tag == "tv")
                {
                    TVController tvController = hit.collider.GetComponent<TVController>();

                    isdetected = true;
                    tooltipPosition = Camera.main.WorldToScreenPoint(hit.collider.transform.position);

                    ShowToolTip("tv", (tvController.IsTvOpen() == false)
                                ? Global.Tooltip.LM_TURN_ON : Global.Tooltip.LM_TURN_OFF + " / " + Global.Tooltip.RM_NEXT_CHANNEL);
                    TooltipLocate(tooltipPosition);

                    return;
                }
                else if (hit.collider.tag == "envObj")
                {
                    VendingController vending = hit.collider.GetComponent<VendingController>();

                    isdetected = true;
                    tooltipPosition = Camera.main.WorldToScreenPoint(hit.collider.transform.position +
                                                                    Global.Tooltip.EnvObjOffset());

                    ShowToolTip((vending.headerTitle == string.Empty) ? "vending" : vending.headerTitle.ToLower(),
                                                                            Global.Tooltip.LM_USE);
                    TooltipLocate(tooltipPosition);

                    return;

                }
                else if (hit.collider.tag == "pc") 
                {
                    Item itemInHand = controller.currentHand.GetComponent<ItemCell>().item;
                    PCController pcController = hit.collider.GetComponent<PCController>();
                    Item disk = pcController.disk;

                    isdetected = true;
                    tooltipPosition = Camera.main.WorldToScreenPoint(hit.collider.transform.position);

                    if (disk != null && controller.IsEmpty(controller.currentHand))
                    {
                        ShowToolTip("pc", Global.Tooltip.LM_INTERACT + " / " + Global.Tooltip.RM_PULL_THE_DISK);
                    }
                    else 
                    { 
                        ShowToolTip("pc", (controller.IsEmpty(controller.currentHand) == false 
                                            && itemInHand.itemName.Contains("disk"))
                                ? Global.Tooltip.LM_INTERACT + " / " + Global.Tooltip.RM_INSERT : Global.Tooltip.LM_INTERACT);
                    }

                    TooltipLocate(tooltipPosition);
                }
                else
                {
                    isdetected = false;
                }
            }

            if (isTooltipOpen == true && isdetected == false)
            {
                HideToolTip();
            }
        }
        else 
        {
            if (isTooltipOpen == true) 
            {
                HideToolTip();
            }
        }
    }

    void ShowToolTip(string itemName, string itemInteraction) 
    {
        toolTip.SetActive(true);
        isTooltipOpen = true;

        textItemName.text = itemName;
        textInteraction.text = itemInteraction;

        Vector2 bgSize = Vector2.zero;

        int itemInteractionLength = itemInteraction.Length;

        // если текст цветной
        if (itemInteraction.Contains(Global.Color.RED_COLOR_PREFIX)) 
        {
            itemInteractionLength -= Global.Color.RED_COLOR_PREFIX.Length + Global.Color.END_COLOR_PREFIX.Length;
        }

        //if (itemName.Length >= itemInteractionLength - 1)
        if (textItemName.preferredWidth >= textInteraction.preferredWidth)
        {
            bgSize = new Vector2(textItemName.preferredWidth + textPaddinSize * 2,
                (textInteraction.preferredHeight * 2) + textPaddinSize * 2);

            //bgSize = new Vector2(textItemName.preferredWidth,
            //    (textInteraction.preferredHeight * 2));

            //  textInteraction.alignment = TextAnchor.MiddleCenter;
        }
        else 
        {
            bgSize = new Vector2(textInteraction.preferredWidth + textPaddinSize * 2,
                (textInteraction.preferredHeight * 2) + textPaddinSize * 2);

            //bgSize = new Vector2(textInteraction.preferredWidth,
            //    (textInteraction.preferredHeight * 2));

            // textItemName.alignment = TextAnchor.MiddleCenter;
        }

        //bgRectTransform.sizeDelta = bgSize;

        bgRectTransform.sizeDelta = new Vector2(bgSize.x, bgSize.y);

        //bgSize.y = bgSize.y + preferredHeight / 2;


        //bgImage.rectTransform.sizeDelta = bgSize;
        bgImage.rectTransform.sizeDelta = bgRectTransform.sizeDelta;
    }

    void HideToolTip() 
    {
      //  textItemName.alignment = TextAnchor.MiddleLeft;
      //  textInteraction.alignment = TextAnchor.MiddleLeft;

        toolTip.SetActive(false);
        isTooltipOpen = false;
    }

    void TooltipLocate(Vector2 pos) 
    {
        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIRectTransform,
            pos, uiCamera, out localPoint);

        localPoint.y = localPoint.y + 32;
        toolTip.transform.localPosition = localPoint;
        //toolTip.transform.localPosition = new Vector3(0,0,0);

    }

    bool IsInActionRadius() 
    {
        return Vector2.Distance(playerTransform.position, 
            Camera.main.ScreenToWorldPoint(Input.mousePosition)) 
            < Controller.GetActioPlayerRadiusStatic() * actionDistanceCof;
    }

    string PrintRed(string str) 
    {
        return Global.Color.RED_COLOR_PREFIX + str + Global.Color.END_COLOR_PREFIX;
    }

    bool IsCurrentHandEmpty() 
    {
        return controller.IsEmpty(controller.currentHand);
    }

    bool IsObjectExist(RaycastHit2D[] hits, string objId, bool isTag) 
    {
        if (isTag == true)
        {
            foreach (var hit in hits)
            {
                if (hit.collider.tag == objId)
                {
                    return true;
                }
            }
        }
        else 
        {
            foreach (var hit in hits)
            {
                if (hit.collider.name.Contains(objId))
                {
                    return true;
                }
            }
        }

        return false;
    }

    RaycastHit2D? FindRaycast(RaycastHit2D[] hits, string objId, bool isTag) 
    {
        if (isTag == true)
        {
            foreach (var hit in hits)
            {
                if (hit.collider.tag == objId)
                {
                    return hit;
                }
            }
        }
        else
        {
            foreach (var hit in hits)
            {
                if (hit.collider.name.Contains(objId))
                {
                    return hit;
                }
            }
        }

        return null;
    }

    void UpdateItemDescriptionPosition() 
    {
        if (controller.isBagOpen == true && staticItemPanel == null)
        {
            rtItemDescription.anchoredPosition = new Vector2(pos.x, pos.y + 32);
        }
        else if (controller.isBagOpen == true && staticItemPanel != null)
        {
            rtItemDescription.anchoredPosition = new Vector2(pos.x, pos.y + 64);
        }
        else
        {
            rtItemDescription.anchoredPosition = pos;
        }
    }

    public void ShowItemReview(string button_tag) 
    {
        Button cell = GameObject.FindGameObjectWithTag(button_tag).GetComponent<Button>();
        Item item = cell.GetComponent<ItemCell>().item;

        staticItemPanel = Global.UIElement.GetStaticItemPanel();

        if (item != null)
        {

            if (controller.IsEmpty(cell) == false)
            {
                //StartCoroutine(SetItemDescription(item));
                itemDescription.SetActive(true);
                descriptionText.text = item.itemName;
                Vector2 contentSize = new Vector2(descriptionText.preferredWidth, rtItemDescription.rect.height);
                
                descriptionText.rectTransform.sizeDelta = contentSize;
                rtImage.sizeDelta = new Vector2(contentSize.x + textPaddinSize * 2, contentSize.y);
            }
        }

        StartCoroutine(UpdateItemDescription(cell));
    }

    IEnumerator UpdateItemDescription(Button cell)
    {
        while (true) 
        { 
            yield return new WaitForSeconds(0.1f);

            Item item = cell.GetComponent<ItemCell>().item;

            if (item != null)
            {

                if (controller.IsEmpty(cell) == false)
                {
                    //StartCoroutine(SetItemDescription(item));
                    itemDescription.SetActive(true);
                    descriptionText.text = item.itemName;
                }
                else 
                {
                    descriptionText.text = string.Empty;
                    itemDescription.SetActive(false);
                }
            }
        }
    }

    public void HideItemReview() 
    {
        StopAllCoroutines();

        if (reviewWindow.activeInHierarchy == true) 
        {
            foreach (Transform child in reviewWindow.transform)
            {
                Destroy(child.gameObject);
            }

            reviewWindow.SetActive(false);
        }

        if (itemDescription.activeInHierarchy == true) 
        {
            itemDescription.SetActive(false);
        }
    }
    public static void Show(string itemName, string itemInteraction) 
    {
        instance.ShowToolTip(itemName, itemInteraction);
    }

    public static void Hide()
    {
        instance.HideToolTip();
    }

}
