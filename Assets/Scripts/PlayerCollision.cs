using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public enum ColliderObject { GameObject, Wall }
    
    [SerializeField]
    ColliderObject colliderObj = ColliderObject.Wall;
    PlayerMovement playerMovement;

    private void Start() 
    {
        playerMovement = Global.Obj.GetPlayerGameObject().GetComponent<PlayerMovement>();
    }


    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "player")
        {
            if (colliderObj == ColliderObject.Wall)
            {
                Vector2 contact = Vector2.zero;

                foreach (var contactPoints in other.contacts)
                {
                    if (contactPoints.normal.x == 1 || contactPoints.normal.x == -1)
                    {
                        contact.x = contactPoints.normal.x;
                    }

                    if (contactPoints.normal.y == 1 || contactPoints.normal.y == -1)
                    {
                        contact.y = contactPoints.normal.y;
                    }
                }
                
                playerMovement.SetDiractionAccess(contact);
            }
            else if (colliderObj == ColliderObject.GameObject)
            {
                //foreach (var contactPoints in other.contacts)
                //{
                //    Debug.Log(contactPoints.normal);
                //}
                playerMovement.SetDiractionAccess(other.contacts[0].normal);
            }
        }   
    }

    void OnCollisionStay2D(Collision2D other) 
    {
        if (other.gameObject.tag == "player")
        {
            if (colliderObj == ColliderObject.Wall)
            {
                Vector2 contact = playerMovement.GetDiractionAccess();

                if (other.contacts.Length == 1)
                {
                    if (contact != other.contacts[0].normal)
                    {
                        playerMovement.SetDiractionAccess(other.contacts[0].normal);
                    }
                }
                else
                {
                    Vector2 firstAx = other.contacts[0].normal;
                    Vector2 lastAx = other.contacts[other.contacts.Length - 1].normal;

                    if ((((firstAx.x == 1 || firstAx .x == -1) && firstAx.y == 0) || ((firstAx.y == 1 || firstAx .y == -1) && firstAx.x == 0)) 
                    && (((lastAx.x == 1 || lastAx .x == -1) && lastAx.y == 0) || ((lastAx.y == 1 || lastAx .y == -1) && lastAx.x == 0)))
                    {
                        Vector2 axisSum = firstAx + lastAx;

                        if (contact != axisSum)
                        {
                            playerMovement.SetDiractionAccess(axisSum);
                        }
                    }

                }
            }
            else if (colliderObj == ColliderObject.GameObject)
            {
                
                Vector2 contact = new Vector2(Mathf.Round(other.contacts[0].normal.x), Mathf.Round(other.contacts[0].normal.y));
                
                if (playerMovement.GetDiractionAccess() != contact)
                {
                    playerMovement.SetDiractionAccess(contact);
                }
                //Debug.Log("___________________");
                //foreach (var contactPoints in other.contacts)
                //{
                //    Debug.Log(contactPoints.normal);
                //}
            }
        }   
    }

    void OnCollisionExit2D(Collision2D other) 
    {
        if (other.gameObject.tag == "player")
        {
            playerMovement.SetDiractionAccess(new Vector2(0, 0));
        } 
    }
}

            //if ((this.transform.position.x - other.transform.position.x) < 0) {
              //  print("hit left");
            //} else if ((this.transform.position.x - other.transform.position.x) > 0) {
             //   print("hit right");
            //}