using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* используется для квест ивента FindGameObjectInSceneState */

public class GameObjectTrigger : MonoBehaviour
{
    bool isChanged = false;

    public void SetIsTriggerd(bool isChanged) 
    {
        this.isChanged = isChanged;
    }

    public bool GetIsTriggerd() 
    {
        return isChanged;
    }

    public string GetGameObjectName() 
    {
        return this.gameObject.name;
    }
}
