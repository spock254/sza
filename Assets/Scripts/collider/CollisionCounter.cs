using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCounter : MonoBehaviour
{
    public List <CollisionUnit> currentCollisions = new List <CollisionUnit> ();
    void Start()
    {
        currentCollisions = new List<CollisionUnit>();
    }

    public bool IsEmpty()
    {
        return Count() == 0;
    }
    public int Count()
    {
        return currentCollisions.Count;
    }

    public void Remove(GameObject go)
    {
        CollisionUnit toRemuve = null;

        foreach (CollisionUnit col in currentCollisions)
        {
            if (col.GetGoName() == go.name)
            {
                toRemuve = col;
                break;
            }
        }

        if (toRemuve != null)
        {
            currentCollisions.Remove(toRemuve);
        }
    }

    /* макисмум 2 колизии */
    public CollisionUnit GetNextCollisionUnit(GameObject go)
    {
        foreach (CollisionUnit col in currentCollisions)
        {
            if (col.GetGoName() != go.gameObject.name)
            {
                return col;
            }
        }

        return null;
    }

    public void SetContact(GameObject go, Vector2 newContact)
    {
        CollisionUnit collision = null;

        foreach (CollisionUnit col in currentCollisions)
        {
            if (col.GetGoName() == go.gameObject.name)
            {
                collision = col;
            }
        }

        if (collision != null)
        {
            collision.SetContact(newContact);
        }
    }

    public void TryAddCollision(CollisionUnit currentGo)
    {
        bool colExist = false;

        foreach (CollisionUnit col in currentCollisions)
        {
            if (col.GetGoName() == currentGo.GetGoName())
            {
                colExist = true;
                break;
            }
        }

        if (colExist == false)
        {
            currentCollisions.Add(currentGo);
        }
    }

    public CollisionUnit GetFirstCollisionUnit()
    {
        return currentCollisions[0];
    }
}

[System.Serializable]
public class CollisionUnit
{
    [SerializeField]
    Vector2 contact = Vector2.zero;
    GameObject go;
    public CollisionUnit(Vector2 contact, GameObject go)
    {
        this.contact = contact;
        this.go = go;
    }

    public string GetGoName()
    {
        return go.gameObject.name;
    }

    public Vector2 GetContact()
    {
        return contact;
    }

    public void SetContact(Vector2 contact)
    {
        this.contact = contact;
    }
}