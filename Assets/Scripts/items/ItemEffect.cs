using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemEffect
{
    public enum EffectUsePlace { hands }
    public GameObject effect;

    [SerializeReference]
    public List<Button> effectCells;

    [HideInInspector]
    public Button currentCell;

}
