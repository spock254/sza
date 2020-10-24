using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;
    public Vector3 input = Vector3.zero;
    //SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody;

    public GameObject skinGo;
    public GameObject skin_baseGo;
    public GameObject hairGo;

    SpriteRenderer skin;
    SpriteRenderer skin_base;
    SpriteRenderer hair;

    [Header("skin")]
    public Sprite up_skin;
    public Sprite down_skin;
    public Sprite left_skin;
    public Sprite right_skin;

    [Header("skin base")]
    public Sprite up_skin_base;
    public Sprite down_skin_base;
    public Sprite left_skin_base;
    public Sprite right_skin_base;

    [Header("hair")]
    public Sprite up_hair;
    public Sprite down_hair;
    public Sprite left_hair;
    public Sprite right_hair;

    ActionWindowController actionWindow;

    void Awake()
    {
        actionWindow = Global.Component.GetActionWindowController();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();

        skin = skinGo.GetComponent<SpriteRenderer>();
        skin_base = skin_baseGo.GetComponent<SpriteRenderer>();
        hair = hairGo.GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        skin.sprite = down_skin;
        hair.sprite = down_hair;
        skin_base.sprite = down_skin_base;
    }

    void FixedUpdate()
    {
        if (!actionWindow.isOpen) 
        { 
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            Vector3 direction = input.normalized;

            SetSprite(direction);

            Vector3 movement = direction * speed * Time.fixedDeltaTime;

            rigidBody.MovePosition(transform.position + movement);
        
        }
    }

    void SetSprite(Vector2 dir) 
    {
        if (dir.x > 0)
        {
            skin.sprite = right_skin;
            hair.sprite = right_hair;
            skin_base.sprite = right_skin_base;
        }
        else if (dir.x < 0)
        {
            skin.sprite = left_skin;
            hair.sprite = left_hair;
            skin_base.sprite = left_skin_base;
        }
        else if (dir.y > 0)
        {
            skin.sprite = up_skin;
            hair.sprite = up_hair;
            skin_base.sprite = up_skin_base;
        }
        else if(dir.y < 0)
        {
            skin.sprite = down_skin;
            hair.sprite = down_hair;
            skin_base.sprite = down_skin_base;
        }
    }
}
