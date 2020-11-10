using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsInHandDraw : MonoBehaviour
{
    Controller controller;          /*   isEmpty()   */

    public PlayerMovement movement;
    public Button left_hand_btn;
    public Button right_hand_btn;

    public GameObject leftHand;
    public GameObject rightHand;

    SpriteRenderer left;
    SpriteRenderer right;

    Vector2 up_down_left_position = new Vector2(-0.122f, -0.120f);
    Vector2 up_down_right_position = new Vector2(0.142f, -0.120f);
    //Vector2 right_left_position = new Vector2(0, -0.113f);

    void Awake()
    {
        controller = Global.Component.GetController();
        
        left = leftHand.GetComponent<SpriteRenderer>();
        right = rightHand.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //if (movement.input.x > 0)
        //{
        //    rightHand.SetActive(false);
        //    leftHand.SetActive(true);
        //    leftHand.transform.position = right_left_position + new Vector2(movement.transform.position.x, movement.transform.position.y);
        //    left.sortingOrder = right.sortingOrder = 13;
        //}
        //else if (movement.input.x < 0)
        //{
        //    leftHand.SetActive(false);
        //    rightHand.SetActive(true);
        //    rightHand.transform.position = right_left_position + new Vector2(movement.transform.position.x, movement.transform.position.y);
        //    left.sortingOrder = right.sortingOrder = 13;
        //}
        if (movement.input.y > 0 && movement.input.x == 0)
        {
            rightHand.SetActive(true);
            leftHand.SetActive(true);

            leftHand.transform.position = up_down_left_position + new Vector2(movement.transform.position.x, movement.transform.position.y);
            rightHand.transform.position = up_down_right_position + new Vector2(movement.transform.position.x, movement.transform.position.y);

            left.sortingOrder = right.sortingOrder = 4;


            left.sprite = left_hand_btn.GetComponent<Image>().sprite;
            right.sprite = right_hand_btn.GetComponent<Image>().sprite;
        }
        else if (movement.input.y < 0 && movement.input.x == 0)
        {
            rightHand.SetActive(true);
            leftHand.SetActive(true);

            leftHand.transform.position = up_down_left_position + new Vector2(movement.transform.position.x, movement.transform.position.y);
            rightHand.transform.position = up_down_right_position + new Vector2(movement.transform.position.x, movement.transform.position.y);

            left.sortingOrder = right.sortingOrder = 13;


            left.sprite = left_hand_btn.GetComponent<Image>().sprite;
            right.sprite = right_hand_btn.GetComponent<Image>().sprite;
            
        }
        else if (movement.input == Vector3.zero)
        {
            rightHand.SetActive(true);
            leftHand.SetActive(true);

            leftHand.transform.position = up_down_left_position + new Vector2(movement.transform.position.x, movement.transform.position.y);
            rightHand.transform.position = up_down_right_position + new Vector2(movement.transform.position.x, movement.transform.position.y);

            left.sortingOrder = right.sortingOrder = 13;

            left.sprite = left_hand_btn.GetComponent<Image>().sprite;
            right.sprite = right_hand_btn.GetComponent<Image>().sprite;
            // anim.Play("idle");
        }
        else 
        {
            rightHand.SetActive(false);
            leftHand.SetActive(false);
        }

        //if (leftHand.activeInHierarchy && !controller.IsEmpty(left_hand_btn))
        //{
        //    left.sprite = left_hand_btn.GetComponent<Image>().sprite;
        //}
        //if (rightHand.activeInHierarchy && !controller.IsEmpty(right_hand_btn))
        //{
        //    right.sprite = right_hand_btn.GetComponent<Image>().sprite;
        //}

        if (!leftHand.activeInHierarchy || !rightHand.activeInHierarchy) 
        {
            right.sprite = left.sprite = null;
        }

        if (controller.IsEmpty(left_hand_btn))
        {
            left.sprite = null;
        }

        if (controller.IsEmpty(right_hand_btn))
        {
            right.sprite = null;
        }

        //if (playerMovement.input.y > 0)
        //{
        //    rightHand.SetActive(true);
        //    leftHand.SetActive(true);

        //    leftHand.transform.position = up_down_left_position + new Vector2(playerMovement.transform.position.x, playerMovement.transform.position.y);
        //    rightHand.transform.position = up_down_right_position + new Vector2(playerMovement.transform.position.x, playerMovement.transform.position.y);

        //    left.sortingOrder = right.sortingOrder = 4;
        //}
        //else if (playerMovement.input.y < 0)
        //{
        //    rightHand.SetActive(true);
        //    leftHand.SetActive(true);

        //    leftHand.transform.position = up_down_left_position + new Vector2(playerMovement.transform.position.x, playerMovement.transform.position.y);
        //    rightHand.transform.position = up_down_right_position + new Vector2(playerMovement.transform.position.x, playerMovement.transform.position.y);

        //    left.sortingOrder = right.sortingOrder = 13;
        //}
        //else if (playerMovement.input.x > 0)
        //{
        //    rightHand.SetActive(false);
        //    leftHand.SetActive(true);
        //    leftHand.transform.position = right_left_position + new Vector2(playerMovement.transform.position.x, playerMovement.transform.position.y);
        //    left.sortingOrder = right.sortingOrder = 13;
        //}
        //else if (playerMovement.input.x < 0)
        //{
        //    leftHand.SetActive(false);
        //    rightHand.SetActive(true);
        //    rightHand.transform.position = right_left_position + new Vector2(playerMovement.transform.position.x, playerMovement.transform.position.y);
        //    left.sortingOrder = right.sortingOrder = 13;
        //}

        //if (leftHand.activeInHierarchy)
        //{
        //    left.sprite = left_hand_btn.GetComponent<Image>().sprite;
        //}
        //if (rightHand.activeInHierarchy)
        //{
        //    right.sprite = right_hand_btn.GetComponent<Image>().sprite;
        //}
    }
}
