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
    //[HideInInspector]
    //public QuestDialogEvent OnDialogEvent;
    [HideInInspector]
    public UseOnPlayerEvent OnUseOnPlayerEvent;
    [HideInInspector]
    public StartDialogEvent OnStartDialogEvent;

    /* TERMINAL EVENTS  */
    [HideInInspector]
    public TerminalEvent OnTerminalOpen;
    [HideInInspector]
    public TerminalEvent OnTerminalClose;

    [HideInInspector]
    public CaseCloseEvent OnCaseCloseEvent;
    [HideInInspector]
    public ChangeSpriteEvent OnChangeSpriteEvent;

    [HideInInspector]
    public AddBuffEvent OnAddBuffEvent;
    
    [HideInInspector]
    public NewTicEvent OnNewTicEvent;

    public CasePanelController casePanelController;
    public RightButtonClickController rightButtonClickController;
    

    void Awake()
    {
        OnMouseClickEvent = new MouseClickEvent();
        OnEnvChangeShapeEvent = new EnvChangeShapeEvent();

        OnStaticCaseItemEvent = new StaticCaseItemEvent();
        OnRightButtonClickEvent = new RightButtonClickEvent();

        OnDoorEvent = new DoorEvent();
        OnCaseEvent = new CaseEvent();

        OnNextQuestEvent = new NextQuestEvent();
        //OnDialogEvent = new QuestDialogEvent();
        OnUseOnPlayerEvent = new UseOnPlayerEvent();
        OnStartDialogEvent = new StartDialogEvent();

        OnTerminalOpen = new TerminalEvent();
        OnTerminalClose = new TerminalEvent();

        OnChangeSpriteEvent = new ChangeSpriteEvent();

        OnNewTicEvent = new NewTicEvent();
        OnAddBuffEvent = new AddBuffEvent();
    }

    private void OnEnable()
    {
        OnStaticCaseItemEvent.AddListener(casePanelController.ActivateStaticItemPanel);
        
        /* right btn click */
        //OnRightButtonClickEvent.AddListener(rightButtonClickController.RightButtonClick);
        
        
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
