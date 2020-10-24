using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalController : MonoBehaviour
{
    public GameObject direcoryLine;
    public GameObject responceLine;

    public InputField terminalInput;
    public GameObject userInputLine;
    public ScrollRect sr;
    public GameObject msgList;

    Interpreter interpreter;

    void Start()
    {
        interpreter = GetComponent<Interpreter>();    
    }

    private void OnGUI()
    {
        terminalInput.ActivateInputField();
        terminalInput.Select();


        if (terminalInput.isFocused && terminalInput.text != "" && Input.GetKeyDown(KeyCode.Return)) 
        {
            string userInput = terminalInput.text;
            
            ClearInputField();
            AddDirectoryLine(userInput);

            int lines = AddInterpriterLines(interpreter.Interpret(userInput));

            ScrallToButtom(lines);

            userInputLine.transform.SetAsLastSibling();
            terminalInput.ActivateInputField();
            terminalInput.Select();
        }
    }

    void ClearInputField() 
    {
        terminalInput.text = "";
    }

    void AddDirectoryLine(string userInput) 
    {
        Vector2 msgListSize = msgList.GetComponent<RectTransform>().sizeDelta;
        msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, msgListSize.y + 20);

        GameObject msg = Instantiate(direcoryLine, msgList.transform);
        
        msg.transform.SetSiblingIndex(msgList.transform.childCount - 1);
        msg.GetComponentsInChildren<Text>()[1].text = userInput;
    }

    int AddInterpriterLines(List<string> interpretation) 
    {
        for (int i = 0; i < interpretation.Count; i++)
        {
            GameObject res = Instantiate(responceLine, msgList.transform);

            res.transform.SetAsLastSibling();

            Vector2 msgListSize = msgList.GetComponent<RectTransform>().sizeDelta;
            msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, msgListSize.y + 20);

            res.GetComponentInChildren<Text>().text = interpretation[i];
        }

        return interpretation.Count;
    }

    void ScrallToButtom(int lines) 
    {
        if (lines > 7)
        {
            sr.velocity = new Vector2(0, 150);
        }
        else 
        {
            sr.verticalNormalizedPosition = 0;
        }
    }
}
