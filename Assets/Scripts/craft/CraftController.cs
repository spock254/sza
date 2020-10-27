using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CraftController : MonoBehaviour
{
    List<ItemCraftData> itemCraftData;
    public SkillsInit skillsInit;

    void Awake()
    {
        itemCraftData = Resources.LoadAll<ItemCraftData>(Global.Path.RECEPT).ToList();
        
    }


    public void Craft_Hands(GameObject itemInHand, GameObject toolGO) 
    {
        Item tool = toolGO.GetComponent<ItemCell>().item;
        Item item = itemInHand.GetComponent<ItemCell>().item;

        if (item.itemOptionData.actionWindowTag != string.Empty) 
        {
            string tagWithPrefix = item.itemOptionData.actionWindowTag + "Hand";

            ActionWindowController actionWindow = Global.Component.GetActionWindowController();
            actionWindow.OpenActionWindow(tagWithPrefix);
            actionWindow.InitActioWindow(tagWithPrefix, null, item, null);
        }

        ItemCraftData recept = FindRecept(tool, item, CraftType.Cooking, CraftTable.Hands);

        if (recept == null)
        {
            Debug.Log("no recept");
            return;
        }

        if (!IsSuitableMinLvl(recept))
        {
            Debug.Log("recept lvl bigger then skill lvl");
            return;
        }

        Item craftResult = recept.recept.craftResult;


        itemInHand.GetComponent<ItemCell>().item = craftResult;
        itemInHand.GetComponent<Image>().sprite = craftResult.itemSprite;
    }

    public bool Craft_Table(RaycastHit2D[] hits, Item tool, CraftType craftType, CraftTable craftTable) 
    {
        GameObject GameObjOnTable = GetGameObjOnTable(hits);
        
        if (GameObjOnTable == null) 
        {
            Debug.Log("TableIsEmpty");
            return false;        
        }

        Item itemOnTabe = GameObjOnTable.GetComponent<ItemCell>().item;


        ItemCraftData recept = FindRecept(tool, itemOnTabe, craftType, craftTable);

        if (recept == null) 
        {
            Debug.Log("no recept");
            return false;
        }

        if (!IsSuitableMinLvl(recept)) 
        {
            Debug.Log("recept lvl bigger then skill lvl");
            return false;
        }

        Item craftResult = recept.recept.craftResult;
        
        // для вызова actionWindow
        if (itemOnTabe.itemOptionData.actionWindowTag != string.Empty) 
        {
            Item _itemOnTabe = Instantiate(itemOnTabe);
            Item _craftResult = Instantiate(craftResult);

            ActionWindowController actionWindow = Global.Component.GetActionWindowController();
            actionWindow.OpenActionWindow(itemOnTabe.itemOptionData.actionWindowTag);
            actionWindow.InitActioWindow(itemOnTabe.itemOptionData.actionWindowTag, 
                                         GameObjOnTable, 
                                         _itemOnTabe, 
                                         _craftResult);


            //UpdateGameObjItem(GameObjOnTable, craftResult);

            return recept.removeTool;
        }


        UpdateGameObjItem(GameObjOnTable, craftResult);

        AddExpReward(recept);

        return recept.removeTool;
    }

    public void Craft_Microwave(MicrowaveController microwave, Item hand, CraftType craftType, CraftTable craftTable) 
    {
        if (microwave.isOpen && microwave.itemInside)
        {
            microwave.Close();
        }
        else if (!microwave.isOpen && microwave.itemInside) 
        {
            StartCoroutine(microwave.Work());

            ItemCraftData recept = FindRecept(hand, microwave.itemInside, craftType, craftTable);

            if (recept == null)
            {
                Debug.Log("no recept");
                return;
            }

            Item craftResult = recept.recept.craftResult;
            Debug.Log(craftResult.itemName);
            microwave.itemInside = craftResult;

            AddExpReward(recept);
        }
    }

    GameObject GetGameObjOnTable(RaycastHit2D[] hits) 
    {
        foreach (var hit in hits)
        {
            if (hit.collider.name.Contains(Global.DROPED_ITEM_PREFIX)) 
            {
                return hit.collider.gameObject;
            }
        }

        return null;
    }

    ItemCraftData FindRecept(Item tool, Item originItem, CraftType craftType, CraftTable craftTable) 
    {
        List<ItemCraftData> sameTool = new List<ItemCraftData>();
        
        foreach (var cd in itemCraftData)
        {
            if (cd.recept.craftTool.Contains(tool))
            {
                sameTool.Add(cd);
            }
        }

        List<ItemCraftData> sameType = sameTool
                    .Where(r => r.craftType == craftType)
                    .Where(r => r.craftTable == craftTable).ToList();

        ItemCraftData recept = sameType
                    .Where(r => r.recept.ingredients[0].itemName.Equals(originItem.itemName))
                    .FirstOrDefault();

        return recept;
    }

    void AddExpReward(ItemCraftData itemCraftData) 
    {
        CraftType craftType = itemCraftData.craftType;
        float exp = itemCraftData.expReward;

        if (craftType == CraftType.Cooking) 
        {
            skillsInit.cooking.AddExp(exp);
        }

    }

    bool IsSuitableMinLvl(ItemCraftData itemCraftData) 
    {
        CraftType craftType = itemCraftData.craftType;
        int minCraftLvl = itemCraftData.craftMinLVL;

        if (craftType == CraftType.Cooking 
        && skillsInit.cooking.GetSkillLvl() >= minCraftLvl)
        {
            return true;
        }

        return false;
    }

    void UpdateGameObjItem(GameObject GameObjOnTable, Item craftResult) 
    {
        GameObjOnTable.GetComponent<ItemCell>().item = craftResult;
        GameObjOnTable.GetComponent<SpriteRenderer>().sprite = craftResult.itemSprite;
    }
}
