﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EWVendingController : MonoBehaviour, IEWInit
{
    GameObject vendingwindow = null;
    Controller controller = null;
    AccauntController accaunt = null;

    public Text price;
    public Text balance;
    public Dropdown dropdown;
    
    public Item card;
    public Item ticket;
    public GameObject prefToSpawn;

    public Color red; 
    public Color green;

    bool isSwiped = false;

    Vector3 ticketSpawnPosition = Vector3.zero;

    readonly int[] prices = new int[4] { 15, 20, 10, 15 };
    public void Init(GameObject vendingwindow) 
    {
        controller = Global.Component.GetController();
        accaunt = Global.Component.GetAccauntController();

        this.vendingwindow = vendingwindow;
        dropdown.onValueChanged.AddListener(OnDDValueChange);
        dropdown.onValueChanged.Invoke(dropdown.value);

        
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);
            foreach (var hit in hits)
            {
                if (hit.collider.tag == "envObj" 
                && card.IsSameItems(controller.GetItemInHand(controller.currentHand))) 
                {
                    int accauntBalance = accaunt.GetAccautBalance();

                    if (accauntBalance >= prices[dropdown.value])
                    {
                        isSwiped = true;
                        balance.color = green;

                    }
                    balance.text = accauntBalance.ToString();
                    ticketSpawnPosition = new Vector3(mousePos.x, mousePos.y, 0);
                }
            }
        }
    }
    public void Close() 
    {
        // разблочить движение
        Destroy(this.vendingwindow);
    }

    public void Pay() 
    {
        if (isSwiped) 
        { 
            accaunt.Remove(prices[dropdown.value]);
            Close();

            
            Item ticketClone = Instantiate(ticket);
            ticketClone.itemOptionData.text = dropdown.captionText.text;
            prefToSpawn.GetComponent<ItemCell>().item = ticketClone;
            prefToSpawn = Instantiate(prefToSpawn, ticketSpawnPosition, Quaternion.identity);
            prefToSpawn.name = Global.DROPED_ITEM_PREFIX + prefToSpawn.name;
        
        }
    }

    void OnDDValueChange(int index) 
    {
        price.text = prices[index].ToString();
    }
}
