using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FormController : MonoBehaviour
{
    const string INPUT_PREFIX = "_input";
    const string DDOWN_PREFIX = "_ddown";

    public List<InputField> inputFields;
    public List<Dropdown> dropdowns;

    ActionWindowController actionWindow;

    GameObject goOnTable;
    Item item;
    Item resultItem;

    bool isOpen = false;

    void Awake()
    {
        actionWindow = Global.Component.GetActionWindowController();
    }

    public void OnClose()
    {
        if (!IsFormFill())
        {
            goOnTable.GetComponent<ItemCell>().item = item;
            goOnTable.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        }
        else
        {
            goOnTable.GetComponent<ItemCell>().item = resultItem;
            goOnTable.GetComponent<SpriteRenderer>().sprite = resultItem.itemSprite;
        }

        goOnTable.GetComponent<ItemCell>().item.itemOptionData.text = ParseToString();

        actionWindow.CloseActionWindow(this.gameObject.tag);
        isOpen = false;

        CleanFields();
    }

    bool IsFormFill() 
    {

        foreach (var item in inputFields)
        {
            if (item.text == string.Empty)
            {
                return false;
            }
        }

        foreach (var item in dropdowns)
        {
            if (item.captionText.text == string.Empty) 
            {
                return false;
            }
        }

        return true;
    }

    void CleanFields() 
    {
        foreach (var item in inputFields)
        {
            item.text = string.Empty;
        }

        foreach (var item in dropdowns)
        {
            item.captionText.text = string.Empty;
        }
    }

    string ParseToString() 
    {
        string toReturn = string.Empty;

        foreach (var item in inputFields)
        {
            toReturn += item.text + INPUT_PREFIX + "\n";
        
        }

        foreach (var item in dropdowns)
        {
            toReturn += item.captionText.text + DDOWN_PREFIX + "\n";
        
        }

        return toReturn;
    }

    List<string> SplitString(string data) 
    {
        if (data == string.Empty) 
        {
            return null;
        }

        return data.Split('\n').ToList();
    }

    void FillFormFromItemData(List<string> data) 
    {
        if (data == null) 
        {
            return;
        }

        List<string> input = new List<string>();
        List<string> ddown = new List<string>();

        foreach (var item in data)
        {
            if (item.EndsWith(INPUT_PREFIX))
            {
                string origin = item.Substring(0, item.Length - INPUT_PREFIX.Length);

                input.Add(origin);

            }
            else if (item.EndsWith(DDOWN_PREFIX)) 
            {
                string origin = item.Substring(0, item.Length - DDOWN_PREFIX.Length);

                 ddown.Add(origin);

            }
        }

        

        for (int i = 0; i < inputFields.Count; i++)
        {
            inputFields[i].text = input[i];
        }

        for (int i = 0; i < dropdowns.Count; i++)
        {
            dropdowns[i].captionText.text = ddown[i];
        }
    }

    public void Init(GameObject goOnTable, Item item, Item resultItem)
    {

        this.goOnTable = goOnTable;
        this.item = item;
        this.resultItem = resultItem;

        FillFormFromItemData(SplitString(item.itemOptionData.text));

        isOpen = true;
    }
}
