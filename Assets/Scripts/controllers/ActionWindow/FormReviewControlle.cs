using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormReviewControlle : MonoBehaviour
{
    const string INPUT_PREFIX = "_input";
    const string DDOWN_PREFIX = "_ddown";

    public List<Text> texts;
    public Image granted;
    ActionWindowController actionWindow;
    void Awake()
    {
        actionWindow = Global.Component.GetActionWindowController();
    }

    public void Init(Item item)
    {
        // добавляет печать на форму если форма проваледированна
        granted.gameObject.SetActive(item.itemOptionData.isModified);
        // если открыть чистый бланк, сначало надо удалить инфу с заполненого 
        CleanForm();

        FillFormFromItemData(SplitString(item.itemOptionData.text));
    }

    void CleanForm() 
    {
        foreach (var text in texts)
        {
            text.text = string.Empty;
        }
    }

    public void OnClose()
    {
        actionWindow.CloseActionWindow(this.gameObject.tag);
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

                input.Add(origin);

            }
        }

        for (int i = 0; i < texts.Count; i++)
        {
            texts[i].text = input[i];
        }
    }
}
