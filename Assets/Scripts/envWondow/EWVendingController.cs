using System.Collections;
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

    public Color red; 
    public Color green;

    bool isSwiped = false;

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
                    isSwiped = true;
                    balance.text = accaunt.GetAccautBalance().ToString();
                    balance.color = green;
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
        accaunt.Remove(prices[dropdown.value]);
        Close();
        //spam ticket
    }

    void OnDDValueChange(int index) 
    {
        price.text = prices[index].ToString();
    }
}
