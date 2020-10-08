using UnityEngine;
using UnityEngine.Events;
[System.Serializable]

public class RightButtonClickEvent : UnityEvent<RaycastHit2D[], Vector2>
{
}
