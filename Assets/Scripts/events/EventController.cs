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
    [HideInInspector]
    public CaseEvent OnCaseEvent;
    [HideInInspector]
    public MouseClickEvent OnMouseClickEvent;
    [HideInInspector]
    public EnvChangeShapeEvent OnEnvChangeShapeEvent;
    [HideInInspector]
    public NextQuestEvent OnNextQuestEvent;
    [HideInInspector]
    public QuestDialogEvent OnDialogEvent;
    [HideInInspector]
    public UseOnPlayerEvent OnUseOnPlayerEvent;

    public CasePanelController casePanelController;
    public RightButtonClickController rightButtonClickController;
    //public DoorController doorController;
    //public CaseController caseController;

    void Awake()
    {
        OnMouseClickEvent = new MouseClickEvent();
        OnEnvChangeShapeEvent = new EnvChangeShapeEvent();

        OnStaticCaseItemEvent = new StaticCaseItemEvent();
        OnRightButtonClickEvent = new RightButtonClickEvent();

        OnDoorEvent = new DoorEvent();
        OnCaseEvent = new CaseEvent();

        OnNextQuestEvent = new NextQuestEvent();
        OnDialogEvent = new QuestDialogEvent();
        OnUseOnPlayerEvent = new UseOnPlayerEvent();
    }

    private void OnEnable()
    {
        OnStaticCaseItemEvent.AddListener(casePanelController.ActivateStaticItemPanel);
        OnRightButtonClickEvent.AddListener(rightButtonClickController.RightButtonClick);
        //OnDoorEvent.AddListener(doorController.OnDoorClick);
        //OnCaseEvent.AddListener(caseController.OnCaseCloseOpen);
    }

    private void OnDisable()
    {
        OnStaticCaseItemEvent.RemoveAllListeners();
        OnRightButtonClickEvent.RemoveAllListeners();
        OnDoorEvent.RemoveAllListeners();
    }
}
