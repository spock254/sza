using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    [HideInInspector]
    public StaticCaseItemEvent OnStaticCaseItemEvent;
    [HideInInspector]
    public RightButtonClickEvent OnRightButtonClickEvent;
    [HideInInspector]
    public DoorEvent OnDoorEvent;

    public CasePanelController casePanelController;
    public RightButtonClickController rightButtonClickController;
    //public DoorController doorController;

    void Awake()
    {
        OnStaticCaseItemEvent = new StaticCaseItemEvent();
        OnRightButtonClickEvent = new RightButtonClickEvent();
        OnDoorEvent = new DoorEvent();
        
    }

    private void OnEnable()
    {
        OnStaticCaseItemEvent.AddListener(casePanelController.ActivateStaticItemPanel);
        OnRightButtonClickEvent.AddListener(rightButtonClickController.RightButtonClick);
        //OnDoorEvent.AddListener(doorController.OnDoorClick);
    }

    private void OnDisable()
    {
        OnStaticCaseItemEvent.RemoveAllListeners();
        OnRightButtonClickEvent.RemoveAllListeners();
        OnDoorEvent.RemoveAllListeners();
    }
}
