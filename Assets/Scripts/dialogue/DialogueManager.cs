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

    void Start()
    {
        eventController.OnStartDialogEvent.AddListener(OnDialogeActivate);
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
        text.text = initDialg;
    }

    public void NextDialogPart() 
    {
        currentPart++;

        if (currentPart < dialogParts.Count)
        {
            text.text = dialogParts[currentPart];
        }
        else 
        {
            speaker = string.Empty;
            currentPart = 0;
            dialogParts = new List<string>();
            DialogPanel.SetActive(false);
        }
    }
}
