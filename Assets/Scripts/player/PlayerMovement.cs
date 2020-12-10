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
            //Debug.Log(direction + " : " + input);
            //SetSprite(direction);

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
    //void SetSprite(Vector2 dir) 
    //{
    //    if (dir.x > 0)
    //    {
    //        skin.sprite = right_skin;
    //        hair.sprite = right_hair;
    //        skin_base.sprite = right_skin_base;
    //    }
    //    else if (dir.x < 0)
    //    {
    //        skin.sprite = left_skin;
    //        hair.sprite = left_hair;
    //        skin_base.sprite = left_skin_base;
    //    }
    //    else if (dir.y > 0)
    //    {
    //        skin.sprite = up_skin;
    //        hair.sprite = up_hair;
    //        skin_base.sprite = up_skin_base;
    //    }
    //    else if(dir.y < 0)
    //    {
    //        skin.sprite = down_skin;
    //        hair.sprite = down_hair;
    //        skin_base.sprite = down_skin_base;
    //    }
    //}
}
