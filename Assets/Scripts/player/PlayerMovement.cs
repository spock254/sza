using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;
    [HideInInspector]
    public Vector3 input = Vector3.zero;
    //SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody;

    ActionWindowController actionWindow;
    DialogueManager dialogWindow;

    Vector3 turn = Vector3.zero;
    bool isMoving = false;

    void Awake()
    {
        actionWindow = Global.Component.GetActionWindowController();
        dialogWindow = Global.Component.GetDialogueManager();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {

    }
    void FixedUpdate()
    {
        if (!actionWindow.isOpen && !dialogWindow.isOpen) 
        {

            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            
            isMoving = (input != Vector3.zero);

            if (input != Vector3.zero) 
            {
                turn = input;
            }

            Vector3 direction = input.normalized;
            Vector3 movement = direction * speed * Time.fixedDeltaTime;
            rigidBody.MovePosition(transform.position + movement);
        
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
