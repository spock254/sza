using System.Collections.Generic;
using UnityEngine;
public enum CraftType { None, Cooking };
public enum CraftTable { None, Hands, Table, Microwave, Oven };
public enum CraftComplexety { Simple, Complex };

[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Recept")]
public class ItemCraftData : ScriptableObject
{
    [SerializeField]
    public Recept recept;
    public CraftComplexety craftComplexety;
    public CraftType craftType;
    public CraftTable craftTable;
    public int craftMinLVL;
    public bool removeTool;
    public float expReward;
}
