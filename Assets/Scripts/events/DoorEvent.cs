
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class DoorEvent : UnityEvent<Item, Vector3, Collider2D, bool>
{
}
