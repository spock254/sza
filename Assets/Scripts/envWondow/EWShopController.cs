using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EWShopController : EWBase, IEWInit
{
    public Item card;
    public GameObject itemContentPref;
    public GameObject prefToSpawn;

    public Transform itemList;
    public Transform payBtn;
    public Transform contentViewer;
    public Transform itemViewer;
    public Text status;
    public Text balance;
    public Text emtyShop;

    Dictionary<Item, int> itemsInShop = new Dictionary<Item, int>();
    AccauntController accauntController;
    Item itemToPay = null;
    List<Item> savedItems;

    const string emptyShopStr = "Shop is empty";
    const string noMoney = "Not enough money";
    const string readyToPay = "Ready to pay?";

    public void Init(GameObject window, GameObject envObj)
    {
        BaseInit(window, envObj);

        accauntController = Global.Component.GetAccauntController();
        savedItems = envObj.GetComponent<VendingController>().savedItems;

        if (savedItems.Count == 0) 
        {
            emtyShop.gameObject.SetActive(true);
            emtyShop.text = SetTextColor(emptyShopStr, TextColor.Red);
            return;
        }

        emtyShop.gameObject.SetActive(false);

        FillItemsInShop();

        FillItemList();
    }


    // Update is called once per frame
    void Update()
    {
        if (IsPlayerInEWindowRadius() == false)
        {
            Close();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            foreach (var hit in hits)
            {
                if (hit.collider.tag == "envObj")
                {
                    if (isItemViewrActive() && card.IsSameItems(controller.GetItemInHand(controller.currentHand))) 
                    {
                        balance.text = accauntController.GetAccautBalance().ToString() + " " + Global.MONEY_SIGN;
                        status.text = SetTextColor(readyToPay, TextColor.Green);
                        payBtn.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    void FillItemsInShop() 
    {
        foreach (var item in savedItems)
        {
            if (itemsInShop.ContainsKey(item))
            {
                itemsInShop[item]++;
            }
            else
            {
                itemsInShop.Add(item, 1);
            }
        }
    }

    void CleanItemsInShop() 
    {
        itemsInShop.Clear();
    }
    void FillItemList() 
    {
        foreach (var itemPair in itemsInShop)
        {
            GameObject itemContentCpy = Instantiate(itemContentPref, itemList);
            FillItemContent(itemContentCpy, itemPair.Key, itemPair.Value);
        }
    }

    void FillItemContent(GameObject itemContentCpy, Item item, int itemsCount) 
    {
        // image
        itemContentCpy.transform.GetChild(0).GetComponent<Image>().sprite = item.itemSprite;
        // item name
        itemContentCpy.transform.GetChild(1).GetComponent<Text>().text = item.itemName;
        // item price
        itemContentCpy.transform.GetChild(2).GetComponent<Text>().text = item.itemPrice.ToString() + Global.MONEY_SIGN;
        // item count
        itemContentCpy.transform.GetChild(3).GetComponent<Text>().text = itemsCount.ToString();
        // button
        itemContentCpy.transform.GetChild(4).GetComponent<Button>()
            .onClick.AddListener(() => ClickOnChoose(item));
    }

    void ClickOnChoose(Item choosedItem) 
    {
        itemToPay = choosedItem;
        MoveToItemViewer();

        itemViewer.GetChild(1).GetComponent<Text>().text = choosedItem.itemName;
        itemViewer.GetChild(2).GetComponent<Image>().sprite = choosedItem.itemSprite;
        itemViewer.GetChild(3).GetComponent<Text>().text = choosedItem.itemPrice.ToString() + Global.MONEY_SIGN;

    }

    void MoveToItemViewer() 
    {
        contentViewer.gameObject.SetActive(false);
        itemViewer.gameObject.SetActive(true);
        status.text = string.Empty;
        balance.text = string.Empty;
    }

    void MoveToContentViewer() 
    {
        itemViewer.gameObject.SetActive(false);
        contentViewer.gameObject.SetActive(true);
        payBtn.gameObject.SetActive(false);

        if (savedItems.Count == 0)
        {
            emtyShop.gameObject.SetActive(true);
            emtyShop.text = SetTextColor(emptyShopStr, TextColor.Red);
        }
    }

    bool isItemViewrActive() 
    {
        return itemViewer.gameObject.activeInHierarchy;
    }

    public void OnPayBtnClick() 
    {
        if (itemToPay.itemPrice <= accauntController.GetAccautBalance())
        {
            accauntController.Remove(itemToPay.itemPrice);
            savedItems.Remove(itemToPay);

            Item itemToPayClone = Instantiate(itemToPay);
            prefToSpawn.GetComponent<ItemCell>().item = itemToPayClone;
            GameObject spawnItem = Instantiate(prefToSpawn, vendorPosition, Quaternion.identity);
            spawnItem.name = Global.DROPED_ITEM_PREFIX + itemToPay.itemName;

            CleanItemsInShop();
            FillItemsInShop();

            CleanItemList();
            FillItemList();

            MoveToContentViewer();

            itemToPay = null;
        }
        else 
        {
            status.text = SetTextColor(noMoney, TextColor.Red);
        }
    }

    void CleanItemList() 
    {
        foreach (Transform child in itemList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
