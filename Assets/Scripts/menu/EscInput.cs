using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscInput : MonoBehaviour
{
    Controller controller = null;
    PCController pcController = null;
    EventController eventController;
    CasePanelController casePanel;

    void Start()
    {
        //pcController = Global.Component.GetTerminalController().GetCurrentPc();    
        controller = Global.Component.GetController();
        eventController = Global.Component.GetEventController();
        casePanel = Global.Component.GetCasePanelController();
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            pcController = Global.Component.GetTerminalController().GetCurrentPc();
            if (controller.isBagOpen == true) 
            {
                controller.CloseOpenContainer(controller.bag_panel, ref controller.isBagOpen);
            }

            if (pcController != null) 
            {
                if (pcController.IsTerminalOpen() == true) 
                { 
                    pcController.Close();
                    eventController.OnTerminalClose.Invoke();
                }
            }

            if (casePanel.IsCaseOpen() == true) 
            {
                controller.CloseOpenContainer(casePanel.staticItemPanel, 
                    ref casePanel.caseIsOpen);
            }
        }
    }
}
