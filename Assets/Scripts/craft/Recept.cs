using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Recept
{
    [SerializeReference]
    public List<Item> ingredients;
    [SerializeReference]
    public List<Item> craftTool;
    [SerializeField]
    public Item craftResult;
}
