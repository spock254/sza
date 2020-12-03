using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class DialogueManager : MonoBehaviour
{
    public EventController eventController;

    public string speaker;
    public List<string> dialogParts;

    public GameObject DialogPanel;
    public Text text;

    int currentPart = -1;
    public bool isOpen = false;

    TextPrintController printController;
    void Start()
    {
        printController = new TextPrintController(text, this);
        eventController.OnStartDialogEvent.AddListener(OnDialogeActivate);
    }

    public bool isLastPart() 
    {
        return currentPart == dialogParts.Count - 1;
    }

    List<string> SplitDialoge(string dialoge) 
    {
        return dialoge.Split('^').ToList();
    }

    public void SetDialog(string dialoge) 
    {
        dialogParts = SplitDialoge(dialoge);
    }

    void OnDialogeActivate(string speaker, string initDialg) 
    {
        this.speaker = speaker;

        DialogPanel.SetActive(true);

        if (initDialg != string.Empty)
        {
            text.text = initDialg;
            //printController.ProcessText(initDialg);
        }
        else 
        {
            text.text = speaker;
        }

        isOpen = true;
    }

    public void NextDialogPart() 
    {
        if (printController.IsRunning() == false) 
        { 
            currentPart++;
        }

        if (currentPart < dialogParts.Count)
        {
            if (printController.IsRunning() == false)
            {
                text.text = string.Empty;
                printController.ProcessText(dialogParts[currentPart]);
            }
            else 
            {
                printController.Stop();
                printController.CompleteText(dialogParts[currentPart]);
            }
            //text.text = dialogParts[currentPart];
        }
        else 
        {
            DisableDialog();
        }
    }

    public void DisableDialog() 
    {
        speaker = string.Empty;
        currentPart = -1;
        dialogParts = new List<string>();
        DialogPanel.SetActive(false);
        isOpen = false;
    }
}
