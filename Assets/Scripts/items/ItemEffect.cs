using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemEffect
{
    public enum EffectUsePlace { environment, left_hand, right_hand }
    public GameObject effect;

    [SerializeReference]
    public List<EffectUsePlace> effectCells;

    [HideInInspector]
    public Vector3 envPosition;

    [HideInInspector]
    public Button currentCell;

    public bool IsSamePlaceEffect() 
    {
        if (currentCell == null) 
        {
            if (effectCells.Contains(EffectUsePlace.environment)) 
            {
                return true;
            }
        }

        if (currentCell == null || effectCells == null) 
        {
            return false;
        }

        foreach (var effectPlace in effectCells)
        {
            if (effectPlace.ToString() == currentCell.name) 
            {
                return true;
            }
        }

        return false;
    }

    public bool IsEffectExist() 
    {
        GameObject effectList = Global.Obj.GetEffectListObject();

        foreach (Transform child in effectList.transform)
        {
            if (child.name.Contains(effect.name)) 
            {
                return true;
            }
        }

        return false;
    }
}
