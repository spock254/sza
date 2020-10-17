using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsInHandDraw : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Button left_hand_btn;
    public Button right_hand_btn;

    public GameObject leftHand;
    public GameObject rightHand;

    SpriteRenderer left;
    SpriteRenderer right;

    Vector2 up_down_left_position = new Vector2(-0.211f, -0.113f);
    Vector2 up_down_right_position = new Vector2(0.211f, -0.113f);
    Vector2 right_left_position = new Vector2(0, -0.113f);

    private void Awake()
    {
        left = leftHand.GetComponent<SpriteRenderer>();
        right = rightHand.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerMovement.input.y > 0)
        {
            rightHand.SetActive(true);
            leftHand.SetActive(true);
            
            leftHand.transform.position = up_down_left_position + new Vector2(playerMovement.transform.position.x, playerMovement.transform.position.y);
            rightHand.transform.position = up_down_right_position + new Vector2(playerMovement.transform.position.x, playerMovement.transform.position.y);

            left.sortingOrder = right.sortingOrder = 4;
        }
        else if (playerMovement.input.y < 0)
        {
            rightHand.SetActive(true);
            leftHand.SetActive(true);

            leftHand.transform.position = up_down_left_position + new Vector2(playerMovement.transform.position.x, playerMovement.transform.position.y);
            rightHand.transform.position = up_down_right_position + new Vector2(playerMovement.transform.position.x, playerMovement.transform.position.y);

            left.sortingOrder = right.sortingOrder = 13;
        }
        else if (playerMovement.input.x > 0) 
        {
            rightHand.SetActive(false);
            leftHand.SetActive(true);
            leftHand.transform.position = right_left_position + new Vector2(playerMovement.transform.position.x, playerMovement.transform.position.y);
            left.sortingOrder = right.sortingOrder = 13;
        }
        else if (playerMovement.input.x < 0)
        {
            leftHand.SetActive(false);
            rightHand.SetActive(true);
            rightHand.transform.position = right_left_position + new Vector2(playerMovement.transform.position.x, playerMovement.transform.position.y);
            left.sortingOrder = right.sortingOrder = 13;
        }

        if (leftHand.activeInHierarchy) 
        { 
            left.sprite = left_hand_btn.GetComponent<Image>().sprite;   
        }
        if (rightHand.activeInHierarchy) 
        { 
            right.sprite = right_hand_btn.GetComponent<Image>().sprite;    
        }
    }
}
