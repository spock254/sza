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
    bool isMoving = false;


    void Awake()
    {
        actionWindow = Global.Component.GetActionWindowController();   
        dialogWindow = Global.Component.GetDialogueManager();           
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
            Vector3 movement = direction * speed;

            transform.position += movement * Time.deltaTime;

        }                                                   
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
