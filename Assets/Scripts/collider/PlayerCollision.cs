using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public enum ColliderObject { GameObject, Wall }
    
    [SerializeField]
    ColliderObject colliderObj = ColliderObject.Wall;
    PlayerMovement playerMovement;
    CollisionCounter colCounter;
    Vector2 contactToExit = Vector2.zero;

    private void Start() 
    {
        playerMovement = Global.Obj.GetPlayerGameObject().GetComponent<PlayerMovement>();
        colCounter = Global.Component.GetCollisionCounter();
    }


    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "player")
        {
            //colCounter.TryAddCollision(this.gameObject);

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
                
                //colCounter.TryAddCollision(new CollisionUnit(contact))

                if (colCounter.IsEmpty())    /* только 1 колизия с обьектом */
                {
                    colCounter.TryAddCollision(new CollisionUnit(contact, this.gameObject));
                    playerMovement.SetDiractionAccess(contact);
                }
                else
                {
                    Vector2 sumAxis = contact + playerMovement.GetDiractionAccess();
                    //Debug.Log(contact + playerMovement.GetDiractionAccess());
                    colCounter.TryAddCollision(new CollisionUnit(contact, this.gameObject));
                    playerMovement.SetDiractionAccess(sumAxis);
                }
            }
            else if (colliderObj == ColliderObject.GameObject)
            {
                //foreach (var contactPoints in other.contacts)
                //{
                //    Debug.Log(contactPoints.normal);
                //}
                if (colCounter.IsEmpty())    /* только 1 колизия с обьектом */
                {
                    Vector2 contact = new Vector2(Mathf.Round(other.contacts[0].normal.x), Mathf.Round(other.contacts[0].normal.y));
                    colCounter.TryAddCollision(new CollisionUnit(contact, this.gameObject));
                    playerMovement.SetDiractionAccess(contact);
                }
                else
                {
                    Vector2 roundAx = new Vector2(Mathf.Round(other.contacts[0].normal.x), Mathf.Round(other.contacts[0].normal.y));
                    Vector2 sumAxis = roundAx + playerMovement.GetDiractionAccess();
                    colCounter.TryAddCollision(new CollisionUnit(sumAxis, this.gameObject));
                    playerMovement.SetDiractionAccess(sumAxis);
                }
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
                        
                        if (colCounter.Count() == 1)
                        {
                            playerMovement.SetDiractionAccess(other.contacts[0].normal);
                        }
                        else if (colCounter.Count() > 1)
                        {
                            Vector2 axisSum = other.contacts[0].normal + colCounter.GetNextCollisionUnit(this.gameObject).GetContact();
                            //Debug.Log("HERE _ WALL" + axisSum);
                            contactToExit = axisSum;
                            playerMovement.SetDiractionAccess(axisSum);
                        }
                    }
                }
                else
                {
                    Vector2 firstAx = other.contacts[0].normal;
                    Vector2 lastAx = other.contacts[other.contacts.Length - 1].normal;

                    if ((((firstAx.x == 1 || firstAx.x == -1) && firstAx.y == 0) || ((firstAx.y == 1 || firstAx .y == -1) && firstAx.x == 0)) 
                    && (((lastAx.x == 1 || lastAx.x == -1) && lastAx.y == 0) || ((lastAx.y == 1 || lastAx .y == -1) && lastAx.x == 0)))
                    {
                        Vector2 axisSum = firstAx + lastAx;

                        if (contact != axisSum)
                        {
                            contactToExit = axisSum;
                            
                            if (colCounter.Count() == 1)
                            {
                                playerMovement.SetDiractionAccess(axisSum);
                            }
                            else if (colCounter.Count() > 1)
                            {
                                //Debug.Log("HERE _ WALL 2" + other.contacts[0].normal + colCounter.GetNextCollisionUnit(this.gameObject).GetContact());
                                playerMovement.SetDiractionAccess(axisSum);
                            }
                        }
                    }

                }
            }
            else if (colliderObj == ColliderObject.GameObject)
            {
                
                Vector2 contact = new Vector2(Mathf.Round(other.contacts[0].normal.x), Mathf.Round(other.contacts[0].normal.y));
                
                if (playerMovement.GetDiractionAccess() != contact)
                {
                    contactToExit = contact;

                    if (colCounter.Count() == 1)
                    {
                        playerMovement.SetDiractionAccess(contact);
                    }
                    else if (colCounter.Count() > 1)
                    {
                        //Debug.Log("HERE _ OBJ" + contact + colCounter.GetNextCollisionUnit(this.gameObject).GetContact());
                        playerMovement.SetDiractionAccess(contact + colCounter.GetNextCollisionUnit(this.gameObject).GetContact());
                    }
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
            colCounter.Remove(this.gameObject);
            //Debug.Log("EXIT");
            if (colCounter.IsEmpty())
            {
                playerMovement.SetDiractionAccess(new Vector2(0, 0));
            }
            else
            {
                playerMovement.SetDiractionAccess(playerMovement.GetDiractionAccess() - contactToExit);
            }

        } 
    }
}
