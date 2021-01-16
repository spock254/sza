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
    
    //const float DIAGONAL_TIMEOUT = 0.5f;
    //float current2BtnsPress = 0f;
    //bool twoButtonsPressed = false;
    //Vector2 prevDiagonPos = Vector2.zero;

    void Awake()
    {
        actionWindow = Global.Component.GetActionWindowController();
        dialogWindow = Global.Component.GetDialogueManager();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        var input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        Vector3 velocity = input.normalized * speed;
        transform.position += velocity * Time.deltaTime;
    }

    //void FixedUpdate()
    //{
    //    if (!actionWindow.isOpen && !dialogWindow.isOpen)
    //    {

    //        input.x = Input.GetAxisRaw("Horizontal");
    //        input.y = Input.GetAxisRaw("Vertical");

    //        //if (input.x != 0 && input.y != 0)
    //        //{
    //        //    //twoButtonsPressed = true;
    //        //    current2BtnsPress = 0;
    //        //    prevDiagonPos = new Vector2(input.x, input.y);
    //        //}
    //        //else 
    //        //{
    //        //    current2BtnsPress += Time.deltaTime;
    //        //    //twoButtonsPressed = false;
    //        //}

    //        isMoving = (input != Vector3.zero);

    //        if (isMoving == true)
    //        {
    //            turn = input;
    //        }
    //        //else 
    //        //{
    //        //    //if (DIAGONAL_TIMEOUT <= current2BtnsPress)
    //        //    //{
    //        //    //    turn = prevDiagonPos;
    //        //    //}
    //        //    //else 
    //        //    //{
    //        //    //    prevDiagonPos = Vector2.zero;
    //        //    //}
    //        //}

    //        Vector3 direction = input.normalized;
    //        Vector3 movement = direction * speed * Time.fixedDeltaTime;
    //        rigidBody.MovePosition(transform.position + movement);

    //    }
    //}

    public Vector3 GetTurnSide() 
    {
        return turn;
    }

    public bool IsPlayerMoving() 
    {
        return isMoving;
    }
}
