using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicInvControll : MonoBehaviour
{
    public Controller controller;
    void Start()
    {
        //Item item = controller.left_hand_btn.GetComponent<ItemCell>().item;

        ////if (item.itemName.Contains("hot")) 
        ////{ 
        //    StartCoroutine(ItemTransformation(item, controller.left_hand_btn));
        ////}

    }

    //IEnumerator ItemTransformation(Item item, Button cell) 
    //{
    //    float currentTime = item.transformationTime;
    //    yield return new WaitForSeconds(1);

    //    currentTime -= Time.deltaTime;

    //    Debug.Log(currentTime);
    //}
}
