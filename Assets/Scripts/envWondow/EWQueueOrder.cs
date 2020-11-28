using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EWQueueOrder : EWBase, IEWInit
{
    [SerializeField]
    Text idText = null;

    [SerializeField]
    Text formText = null;

    [SerializeField]
    Text statusText = null;

    List<Item> savedItems = null;

    public GameObject prefToSpawn;

    public Item id = null;
    public Item form = null;
    public Item queueTicket = null;

    const string idNotPresent = "ID not present";
    const string formNotPresent = "Form not present";
    const string idPresent = "ID present";
    const string formPresent = "Form present";
    //const string correctStatus = "Valid form";

    string formData = null;
    bool isGranted = false;

    void Update()
    {
        if (IsPlayerInEWindowRadius() == false)
        {
            Close();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            foreach (var hit in hits)
            {
                if (hit.collider.tag == "envObj")
                {
                    Item itemInHand = controller.GetItemInHand(controller.currentHand);

                    if (id.IsSameItems(itemInHand))
                    {
                        if (!savedItems.Contains(id))
                        {
                            savedItems.Add(id);
                            idText.text = idPresent;
                            controller.SetDefaultItem(controller.currentHand);
                        }
                    }
                    else if (form.IsSameItems(itemInHand))
                    {
                        if (!savedItems.Find(f => f.IsSameItems(form)))
                        {
                            formData = itemInHand.itemOptionData.text;
                            isGranted = itemInHand.itemOptionData.isModified;

                            Item formCpy = Instantiate(form);
                            formCpy.itemOptionData.text = itemInHand.itemOptionData.text;
                            formCpy.itemOptionData.isModified = itemInHand.itemOptionData.isModified;
                            savedItems.Add(formCpy);

                            formText.text = formPresent;
                            controller.SetDefaultItem(controller.currentHand);
                        }

                    }

                    InitStatus();

                    return;
                }
            }
        }
    }

    public void Init(GameObject window, GameObject envObj)
    {
        BaseInit(window, envObj);

        savedItems = envObj.GetComponent<VendingController>().savedItems;
        InitWindow(savedItems);

        InitStatus();
    }

    public void OnPullOutClick(bool isId)
    {
        Item to_remove = null;

        if (isId && savedItems.Contains(id))
        {
            savedItems.Remove(id);
            idText.text = idNotPresent;

            Item idClone = Instantiate(id);
            prefToSpawn.GetComponent<ItemCell>().item = idClone;
            Instantiate(prefToSpawn, vendorPosition, Quaternion.identity);
            prefToSpawn.name = Global.DROPED_ITEM_PREFIX + idClone.name;
        }
        else if (!isId && (to_remove = savedItems.Find(f => f.IsSameItems(form))))
        {
            savedItems.Remove(to_remove);


            formText.text = formNotPresent;


            Item formClone = Instantiate(form);

            formClone.itemOptionData.text = formData;
            formClone.itemOptionData.isModified = isGranted;

            prefToSpawn.GetComponent<ItemCell>().item = formClone;
            Instantiate(prefToSpawn, vendorPosition, Quaternion.identity);
            prefToSpawn.name = Global.DROPED_ITEM_PREFIX + formClone.name;

            isGranted = false;
            formData = string.Empty;
        }

        InitStatus();
    }

    public void OnOrderClick() 
    {
        if (savedItems.Count == 2) 
        {
            if (IsGranded())
            {
                Item queueTicketClone = Instantiate(queueTicket);

                prefToSpawn.GetComponent<ItemCell>().item = queueTicketClone;
                Instantiate(prefToSpawn, vendorPosition, Quaternion.identity);
                prefToSpawn.name = Global.DROPED_ITEM_PREFIX + queueTicketClone.name;

                statusText.text = SetTextColor("Your order in queue #13", TextColor.Green);
            }
            else 
            {
                statusText.text = SetTextColor("Invalid form", TextColor.Red);
            }
        }
        else 
        { 
            InitStatus();
        }
    }

    void InitStatus()
    {
        if (savedItems.Find(f => f.IsSameItems(form)) && savedItems.Contains(id))
        {
            statusText.text = SetTextColor("Ready to order queue", TextColor.Green);
        }
        else if (savedItems.Find(f => f.IsSameItems(form)) && !savedItems.Contains(id))
        {
            statusText.text = SetTextColor("Please put id", TextColor.Red);
        }
        else if (!savedItems.Find(f => f.IsSameItems(form)) && savedItems.Contains(id))
        {
            statusText.text = SetTextColor("Please put form", TextColor.Red);
        }
        else
        {
            statusText.text = SetTextColor("Please put id and form", TextColor.Red);
        }
    }

    void InitWindow(List<Item> savedItems)
    {
        idText.text = (savedItems.Contains(id)) ? idPresent : idNotPresent;
        formText.text = (savedItems.Find(f => f.IsSameItems(form))) ? formPresent : formNotPresent;
    }

    bool IsGranded()
    {
        Item savedForm = savedItems.Find(f => f.IsSameItems(form));

        if (savedForm == null) 
        {
            return false;
        }

        return savedForm.itemOptionData.isModified;
    }
}
