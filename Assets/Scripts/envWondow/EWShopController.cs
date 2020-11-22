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
    public void Init(GameObject window, GameObject envObj)
    {
        BaseInit(window, envObj);

        accauntController = Global.Component.GetAccauntController();
        savedItems = envObj.GetComponent<VendingController>().savedItems;

        if (savedItems.Count == 0) 
        {
            emtyShop.gameObject.SetActive(true);
            emtyShop.text = SetTextColor("Shop is empty", TextColor.Red);
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
                        status.text = SetTextColor("Ready to pay?", TextColor.Green);
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
        itemContentCpy.transform.GetChild(2).GetComponent<Text>().text = item.itemPrice.ToString();
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
        itemViewer.GetChild(3).GetComponent<Text>().text = choosedItem.itemPrice.ToString();

    }

    void MoveToItemViewer() 
    {
        contentViewer.gameObject.SetActive(false);
        itemViewer.gameObject.SetActive(true);
    }

    void MoveToContentViewer() 
    {
        itemViewer.gameObject.SetActive(false);
        contentViewer.gameObject.SetActive(true);
        payBtn.gameObject.SetActive(false);
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
            //RemoveShopItem(itemToPay);

            Item itemToPayClone = Instantiate(itemToPay);
            prefToSpawn.GetComponent<ItemCell>().item = itemToPayClone;
            Instantiate(prefToSpawn, vendorPosition, Quaternion.identity);
            prefToSpawn.name = Global.DROPED_ITEM_PREFIX + prefToSpawn.name;

            MoveToContentViewer();

            CleanItemsInShop();
            FillItemsInShop();

            CleanItemList();
            FillItemList();

            itemToPay = null;
        }
        else 
        {
            status.text = SetTextColor("not enough money", TextColor.Red);
        }
    }

    void CleanItemList() 
    {
        foreach (Transform child in itemList.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    void RemoveShopItem(Item item) 
    {
        if (itemsInShop[item] > 1)
        {
            itemsInShop[item]--;
        }
        else 
        {
            itemsInShop.Remove(item);
        }
    }
}
