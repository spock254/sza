using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EWFormCheckerController : EWBase, IEWInit
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

    const string idNotPresent = "ID not present";
    const string formNotPresent = "Form not present";

    const string idPresent = "ID present";
    const string formPresent = "Form present";

    string formData = null;

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
                        if (!savedItems.Contains(form))
                        {
                            formData = itemInHand.itemOptionData.text;

                            savedItems.Add(form);
                            formText.text = formPresent;
                            controller.SetDefaultItem(controller.currentHand);
                        }

                    }
                    
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
    }

    public void OnPullOutClick(bool isId) 
    {
        if (isId && savedItems.Contains(id))
        {
            savedItems.Remove(id);
            idText.text = idNotPresent;

            Item idClone = Instantiate(id);
            prefToSpawn.GetComponent<ItemCell>().item = idClone;
            Instantiate(prefToSpawn, vendorPosition, Quaternion.identity);
            prefToSpawn.name = Global.DROPED_ITEM_PREFIX + prefToSpawn.name;
        }
        else if (!isId && savedItems.Contains(form)) 
        {
            savedItems.Remove(form);
            formText.text = formNotPresent;

            // без потерь полей
            //form.itemOptionData.text = formData;
            Item formClone = Instantiate(form);
            formClone.itemOptionData.text = formData;
            prefToSpawn.GetComponent<ItemCell>().item = formClone;
            Instantiate(prefToSpawn, vendorPosition, Quaternion.identity);
            prefToSpawn.name = Global.DROPED_ITEM_PREFIX + prefToSpawn.name;
        }
    }

    public void OnCheckClick() 
    {
        if (savedItems.Contains(form) && savedItems.Contains(id))
        {
            Debug.Log(form.itemOptionData.text);
        }
        else if (savedItems.Contains(form) && !savedItems.Contains(id))
        {
            statusText.text = "please put id";
        }
        else if (!savedItems.Contains(form) && savedItems.Contains(id))
        {
            statusText.text = "please put form";
        }
        else 
        {
            statusText.text = "please put id and form";
        }
    }

    void InitWindow(List<Item> savedItems) 
    {
        idText.text = (savedItems.Contains(id)) ? idPresent : idNotPresent;
        formText.text = (savedItems.Contains(form)) ? formPresent : formNotPresent;
    }
}
