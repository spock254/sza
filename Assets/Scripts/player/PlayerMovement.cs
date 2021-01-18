using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;                               

    [HideInInspector]
    public Vector3 input = Vector3.zero;    

    ActionWindowController actionWindow;     
    DialogueManager dialogWindow;          

    Vector3 turn = Vector3.zero;  
    Vector3 movement = Vector3.zero;          
    bool isMoving = false;
    Rigidbody2D rb = null;

    void Awake()
    {
        actionWindow = Global.Component.GetActionWindowController();   
        dialogWindow = Global.Component.GetDialogueManager();  
        rb = GetComponent<Rigidbody2D>();         
    }

    private void Update()
    {
        
        if (!actionWindow.isOpen && !dialogWindow.isOpen)   
        {                                                   

            input.x = Input.GetAxisRaw("Horizontal");   
            input.y = Input.GetAxisRaw("Vertical");     

            isMoving = (input != Vector3.zero);         

            if (isMoving == true)      
            {
                turn = input;
            }

            Vector3 direction = input.normalized;   
            movement = direction * speed;   
        }                                                   
    }

    void FixedUpdate() 
    {
        rb.MovePosition(rb.transform.position + movement * Time.fixedDeltaTime);
        //transform.position += movement * Time.fixedDeltaTime;
    }

    public Vector3 GetTurnSide() 
    {
        return turn;
    }


    public bool IsPlayerMoving() 
    {
        return isMoving;
    }
}
