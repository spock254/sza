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

    PlayerInfo playerInfo;

    const string idNotPresent = "ID not present";
    const string formNotPresent = "Form not present";
    const string idPresent = "ID present";
    const string formPresent = "Form present";
    const string correctStatus = "Valid form";

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
                        if (!savedItems.Contains(form))
                        {
                            formData = itemInHand.itemOptionData.text;
                            isGranted = itemInHand.itemOptionData.isModified;

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
        playerInfo = Global.Component.GetPlayerInfo();

        InitStatus();
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


            Item formClone = Instantiate(form);

            formClone.itemOptionData.text = formData;
            formClone.itemOptionData.isModified = isGranted;

            prefToSpawn.GetComponent<ItemCell>().item = formClone;
            Instantiate(prefToSpawn, vendorPosition, Quaternion.identity);
            prefToSpawn.name = Global.DROPED_ITEM_PREFIX + prefToSpawn.name;

            isGranted = false;
            formData = string.Empty;
        }
    }


    public void OnCheckClick() 
    {
        if (savedItems.Contains(form) && savedItems.Contains(id))
        {
            string checkStatus = CheckForm();

            if (checkStatus.Contains(correctStatus)) 
            {
                isGranted = true;
            }

            statusText.text = checkStatus;
        }
        else if (savedItems.Contains(form) && !savedItems.Contains(id))
        {
            statusText.text = SetTextColor("Please put id", TextColor.Red);
        }
        else if (!savedItems.Contains(form) && savedItems.Contains(id))
        {
            statusText.text = SetTextColor("Please put form", TextColor.Red);
        }
        else 
        {
            statusText.text = SetTextColor("Please put id and form", TextColor.Red);
        }
    }

    void InitStatus() 
    {
        if (savedItems.Contains(form) && savedItems.Contains(id)) 
        {
            statusText.text = SetTextColor("Ready to validate", TextColor.Green);
        }
        else if (savedItems.Contains(form) && !savedItems.Contains(id))
        {
            statusText.text = SetTextColor("Please put id", TextColor.Red);
        }
        else if (!savedItems.Contains(form) && savedItems.Contains(id))
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
        formText.text = (savedItems.Contains(form)) ? formPresent : formNotPresent;
    }

    string CheckForm() 
    {
        List<string> lines = formData.Split('\n').ToList();

        string ssNumber = lines[0].Split('_')[0];
        string playerName = lines[1].Split('_')[0];
        string age = lines[2].Split('_')[0];
        string id = lines[3].Split('_')[0];
        string sign = lines[4].Split('_')[0];
        string race = lines[5].Split('_')[0];
        string planet = lines[6].Split('_')[0];
        string ocupation = lines[7].Split('_')[0];

        if (ssNumber != "213") 
        {
            return SetTextColor("Space station #" + ssNumber + " invalid", TextColor.Red);
        }

        if (playerName.ToLower().Trim() != playerInfo.playerName.ToLower()) 
        {
            return SetTextColor("Invalid name", TextColor.Red);
        }

        if (id != playerInfo.ID) 
        {
            return SetTextColor("Invalid ID", TextColor.Red);
        }

        // TODO
        if (age != "23") 
        {
            return SetTextColor("Invalid age", TextColor.Red);
        }

        if (race.ToLower() != "human") 
        {
            return SetTextColor("Invalid race", TextColor.Red);
        }

        if (planet.ToLower() != playerInfo.planetOfOrigin.ToLower())
        {
            return SetTextColor("Invalid planet of origin", TextColor.Red);
        }

        if (ocupation.ToLower() != "service") 
        {
            return SetTextColor("No free ocupation", TextColor.Red);
        }

        if (sign.ToLower().Trim() != playerInfo.signature)
        {
            return SetTextColor("Invalid signature", TextColor.Red);
        }

        return SetTextColor(correctStatus, TextColor.Green); ;
    }
}
