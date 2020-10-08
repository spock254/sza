using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;

    Rigidbody2D rigidBody;

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        Vector3 input = Vector3.zero;
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        Vector3 direction = input.normalized;

        Vector3 movement = direction * speed * Time.fixedDeltaTime;

        rigidBody.MovePosition(transform.position + movement);
    }
    //public float moveSpeed = 4;
    //private Vector2 playerDir;
    //public static int x_player;
    //public static int y_player;
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    Move();
    //}

    //private void Move()
    //{
    //    if (Input.GetAxis("Vertical") != 0) 
    //    {
    //        if (Input.GetAxis("Vertical") > 0)
    //        {
    //            y_player = 1;
    //        }
    //        else 
    //        {
    //            y_player = -1;
    //        }

    //        x_player = 0;

    //        playerDir = new Vector2(0, Input.GetAxisRaw("Vertical"));
    //    }
    //    else if (Input.GetAxis("Horizontal") != 0) 
    //    {
    //        if (Input.GetAxis("Horizontal") > 0)
    //        {
    //            x_player = 1;
    //        }
    //        else
    //        {
    //            x_player = -1;
    //        }

    //        y_player = 0;

    //        playerDir = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

    //    }
    //    transform.Translate(playerDir * moveSpeed * Time.deltaTime);
    //}
}
