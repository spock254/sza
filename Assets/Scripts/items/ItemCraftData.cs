using System.Collections.Generic;
using UnityEngine;
public enum CraftType { None, Cooking };
public enum CraftTable { None, Hands, Table };

[System.Serializable]
public class ItemCraftData
{

    [SerializeReference]
    public List<Item> recept;
    [SerializeReference]
    public List<Item> craftTool;
    public CraftType craftType;
    public CraftTable craftTable;
    public int craftMinLVL;
}
