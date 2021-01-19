using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    PlayerMovement playerMovement;

    private void Start() 
    {
        playerMovement = Global.Obj.GetPlayerGameObject().GetComponent<PlayerMovement>();
    }


    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag == "player")
        {
            Debug.Log("ENTER");
            
            Vector2 contactPoints = other.contacts[0].normal;
            playerMovement.SetDiractionAccess(contactPoints);
        }   
    }
    Vector2 dirAccess = Vector2.zero;
    void OnCollisionStay2D(Collision2D other) 
    {
        if (other.gameObject.tag == "player")
        {
            //Debug.Log("STAY");
            //Debug.Log(playerMovement.GetDiractionAccess() + " OLD");
            //Vector2 contactPoints = other.contacts[0].normal;
            //Debug.Log(contactPoints);
            //if (playerMovement.GetDiractionAccess() != Vector2.zero)
            //{
            //    dirAccess = playerMovement.GetDiractionAccess();

            //}
            //if (other.contacts.Length == 4)
            //{
            //    playerMovement.SetDiractionAccess(new Vector2(other.contacts[0].normal.x, other.contacts[3].normal.x));
            //}
            //else if (other.contacts.Length == 2)
            //{
            //    playerMovement.SetDiractionAccess(new Vector2(other.contacts[0].normal.x, other.contacts[0].normal.y));
            //}
            //if (contactPoints.x != dirAccess.x)
            //{
                //playerMovement.SetDiractionAccess(new Vector2(contactPoints.x, dirAccess.y));
            //}
            //else if (contactPoints.y != dirAccess.y)
            //{
              //  playerMovement.SetDiractionAccess(new Vector2(dirAccess.x, contactPoints.y));
                //Debug.Log(playerMovement.GetDiractionAccess());
            //}

            //Vector2 contact = Vector2.zero;

            //foreach (var contactPoints in other.contacts)
            //{
            //    if (contactPoints.normal.x != 0)
            //    {
            //        contact.x = contactPoints.normal.x;
            //    }

            //    if (contactPoints.normal.y != 0)
            //    {
            //        contact.y = contactPoints.normal.y;
            //    }
            //}

            //playerMovement.SetDiractionAccess(contact);

            //Debug.Log("____________________");
            //foreach (var contactPoints in other.contacts)
            //{
            //    Debug.Log(contactPoints.normal);

            //}
            //playerMovement.SetDiractionAccess(contactPoints);
        }   
    }

    void OnCollisionExit2D(Collision2D other) 
    {
        if (other.gameObject.tag == "player")
        {
            Debug.Log("EXIT");
            playerMovement.SetDiractionAccess(new Vector2(0, 0));
            dirAccess = Vector2.zero;
        } 
    }
}

            //if ((this.transform.position.x - other.transform.position.x) < 0) {
              //  print("hit left");
            //} else if ((this.transform.position.x - other.transform.position.x) > 0) {
             //   print("hit right");
            //}