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

    readonly Vector2 ALL_DIR_ACCESS = new Vector2(0, 0);

    private void Start() 
    {
        playerMovement = Global.Obj.GetPlayerGameObject().GetComponent<PlayerMovement>();
        colCounter = Global.Component.GetCollisionCounter();
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

                if (colCounter.IsEmpty())    /* только 1 колизия с обьектом */
                {
                    colCounter.TryAddCollision(new CollisionUnit(contact, this.gameObject));
                    playerMovement.SetDiractionAccess(contact);
                }
                else
                {
                    Vector2 sumAxis = contact + playerMovement.GetDiractionAccess();
                    colCounter.TryAddCollision(new CollisionUnit(contact, this.gameObject));
                    playerMovement.SetDiractionAccess(sumAxis);
                }
            }
            else if (colliderObj == ColliderObject.GameObject)
            {
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
                    colCounter.TryAddCollision(new CollisionUnit(roundAx, this.gameObject));
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
                            colCounter.SetContact(this.gameObject, other.contacts[0].normal);
                            contactToExit = other.contacts[0].normal;
                            playerMovement.SetDiractionAccess(other.contacts[0].normal);
                        }
                        else if (colCounter.Count() > 1)
                        {
                            Vector2 axisSum = other.contacts[0].normal + colCounter.GetNextCollisionUnit(this.gameObject).GetContact();
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
                            
                            //if (colCounter.Count() == 1)
                            //{
                            //    playerMovement.SetDiractionAccess(axisSum);
                            //}
                            //else if (colCounter.Count() > 1)
                            //{
                                playerMovement.SetDiractionAccess(axisSum);
                            //}
                        }
                    }

                }
            }
            else if (colliderObj == ColliderObject.GameObject)
            {
                Vector2 contact = new Vector2(Mathf.Round(other.contacts[0].normal.x), Mathf.Round(other.contacts[0].normal.y));

                contactToExit = contact;
                Vector2 firstAx = other.contacts[0].normal;
                Vector2 lastAx = other.contacts[other.contacts.Length - 1].normal;

                if(colCounter.Count() == 1)
                {
                    if (other.contacts.Length == 1)
                    {
                        /*   игрок на углу обьекта   */
                        if (firstAx.x != 0 && firstAx.y != 0)
                        {
                            Vector2 absCornerContact = new Vector2(Mathf.Abs(firstAx.x), Mathf.Abs(firstAx.y));
                            contact = (absCornerContact.x > absCornerContact.y) ? new Vector2(contact.x, 0) : new Vector2(0, contact.y);
                        }

                        playerMovement.SetDiractionAccess(contact);
                        // Debug.Log("_____________");
                        // foreach (var cpoint in other.contacts)
                        // {
                        //     Debug.Log(cpoint.normal);
                        // }
                    }
                    else if ((((firstAx.x == 1 || firstAx.x == -1) && firstAx.y == 0) || ((firstAx.y == 1 || firstAx .y == -1) && firstAx.x == 0)) 
                    && (((lastAx.x == 1 || lastAx.x == -1) && lastAx.y == 0) || ((lastAx.y == 1 || lastAx .y == -1) && lastAx.x == 0)))
                    {
                        playerMovement.SetDiractionAccess(other.contacts[0].normal + other.contacts[1].normal);
                    }
                }
                else if (colCounter.Count() > 1)
                {
                    playerMovement.SetDiractionAccess(contact + colCounter.GetNextCollisionUnit(this.gameObject).GetContact());
                }
                // Vector2 contact = new Vector2(Mathf.Round(other.contacts[0].normal.x), Mathf.Round(other.contacts[0].normal.y));
                
                // if (playerMovement.GetDiractionAccess() != contact)// && ((playerMovement.GetDiractionAccess().x != 0 && playerMovement.GetDiractionAccess().y == 0) 
                //                                                    //|| (playerMovement.GetDiractionAccess().x == 0 && playerMovement.GetDiractionAccess().y != 0)))
                // {
                //     contactToExit = contact;
                //     if (colCounter.Count() == 1)
                //     {
                //         Vector2 axisSum = colCounter.GetFirstCollisionUnit().GetContact() + contact;
                //         //colCounter.TryAddCollision(new CollisionUnit(contact, this.gameObject));
                //             //contact = axisSum;
                //         contactToExit = axisSum;
                //         playerMovement.SetDiractionAccess(axisSum);

                //     }
                //     else if (colCounter.Count() > 1)
                //     {
                //         playerMovement.SetDiractionAccess(contact + colCounter.GetNextCollisionUnit(this.gameObject).GetContact());
                //     }
                // }
            }
        }   
    }

    void OnCollisionExit2D(Collision2D other) 
    {
        if (other.gameObject.tag == "player")
        {
            colCounter.Remove(this.gameObject);

            if (colCounter.IsEmpty())
            {
                playerMovement.SetDiractionAccess(ALL_DIR_ACCESS);
            }
            else
            {
                playerMovement.SetDiractionAccess(playerMovement.GetDiractionAccess() - contactToExit);
            }

        } 
    }
}
