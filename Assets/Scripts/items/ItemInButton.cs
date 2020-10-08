using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInButton : MonoBehaviour
{
    [HideInInspector]
    public GameObject item;
    [HideInInspector]
    public Controller controller;

    public void OnClickPickUp()
    {
        controller.ItemPickUp(item);
    }
}
