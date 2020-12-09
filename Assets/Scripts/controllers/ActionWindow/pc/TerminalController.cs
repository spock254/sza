using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TerminalController : MonoBehaviour
{
    public GameObject direcoryLine;
    public GameObject responceLine;

    public InputField terminalInput;
    public GameObject userInputLine;
    public ScrollRect sr;
    public GameObject msgList;

    public float printDelay = 0.1f;

    Interpreter interpreter;
    public bool isOpen = false;

    PCController pcController;

    //public List<string> history = new List<string>();
    public List<string> history = new List<string>();

    bool isInit = false;
    void Start()
    {
        interpreter = GetComponent<Interpreter>();
    }

    string ScrollHistory(string input) 
    {
        int index = 0;

        if (history.Count == 0) 
        {
            return input;
        }


        for (int i = 0; i < history.Count; i++)
        {
            if (input == history[i]) 
            {
                index = i;
                break;
            }
        }

        if (index + 1 == history.Count)
        {
            index = 0;
        }
        else 
        {
            index++;
        }

        return history[index];
        //int index = 0;

        //if (history.Count > 0) 
        //{ 
        //    if (Input.GetKeyDown(KeyCode.DownArrow))
        //    {
        //        index++;

        //        if (index > history.Count) 
        //        {
        //            index = history.Count - 1;
                    
        //        }

        //        return history[index];
        //    }
        //    else if (Input.GetKeyDown(KeyCode.UpArrow)) 
        //    {
        //        index--;
            
        //        if (index < 0) 
        //        {
        //            index = 0;
        //            return string.Empty;
        //        }
                
        //        return history[index];
        //    }
        //}

        //Debug.Log(index);
        //return input;
    }

    void AddToHistory(string command) 
    {
        //if (history.Count == 100)
        //{
        //    history.RemoveAt(99);
        //}
        //else if (history.Contains(command))
        //{
        //    history.Remove(command);
        //}
        //Debug.Log("added");
        history.Add(command);
    }


    int index = 0;
    private void OnGUI()
    {
        if (isOpen) 
        {
            terminalInput.ActivateInputField();
            terminalInput.Select();
            
            if (!isInit) 
            {
                isInit = true;

                //string userInput = "help -all";

                //AddToHistory(userInput);

                ClearInputField();
                //AddDirectoryLine(userInput);

                int lines = AddInterpriterLines(new List<string>() 
                {
                    " ",
                    " ",
                    " ",
                    " ",
                    "\t\tWelcome " + pcController.currentMemory.userName + " to <color=#FFFFFF>SYSTEM_32s</color>.",
                    " ",
                    "to familiarize yourself with the system use",
                    "help -sf help",
                    " ",
                    " ",
                    " ",
                    " ",
                    " ",
                    " "
                });

                ScrallToButtom(lines);

                userInputLine.transform.SetAsLastSibling();
            }
        }
        else 
        {
            isInit = false;
            
        }


        //terminalInput.text = ScrollHistory(terminalInput.text);

        //if (terminalInput.isFocused && Input.GetKeyDown(KeyCode.UpArrow) && history.Count > 0)
        //{
        //    if (index == history.Count || index == -1)
        //    {
        //        index = 0;
        //    }

        //    terminalInput.text = history[index];
        //    StartCoroutine(ChangeCaretPosition());
        //    index++;
        //}
        //else if (terminalInput.isFocused && Input.GetKeyDown(KeyCode.DownArrow) && history.Count > 0) 
        //{
        //    if (index == 0 || index > history.Count)
        //    {
        //        index = history.Count - 1;
        //    }

        //    terminalInput.text = history[index];
        //    StartCoroutine(ChangeCaretPosition());
        //    index--;
        //}

        if (terminalInput.isFocused && terminalInput.text != "" && Input.GetKeyDown(KeyCode.Return)) 
        {
            string userInput = terminalInput.text;

            AddToHistory(userInput);

            ClearInputField();
            AddDirectoryLine(userInput);

            int lines = AddInterpriterLines(interpreter.Interpret(userInput));

            ScrallToButtom(lines);

            userInputLine.transform.SetAsLastSibling();
            terminalInput.ActivateInputField();
            terminalInput.Select();
        }
    }

    IEnumerator ChangeCaretPosition() 
    {
        yield return new WaitForEndOfFrame();
        terminalInput.caretPosition = terminalInput.text.Length;
        terminalInput.ForceLabelUpdate();
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

            //res.GetComponentInChildren<Text>().text = interpretation[i];
            res.GetComponentInChildren<Text>().text = string.Empty;
            //StartCoroutine(PrintWithDilay(res, interpretation[i]));
            res.GetComponentInChildren<Text>().text = interpretation[i];
        }
        //StartCoroutine(AddInterpriterLinesDilay(interpretation));

        

        return interpretation.Count;
    }

    void ScrallToButtom(int lines) 
    {
        if (lines > 4)
        {
            sr.velocity = new Vector2(0, 450);
        }
        else 
        {
            sr.verticalNormalizedPosition = 0;
        }
    }

    IEnumerator PrintWithDilay(GameObject res, string str) 
    {
        Dictionary<int, char> dict = new Dictionary<int, char>();

        char[] temp = new char[str.Length];

        for (int i = 0; i < str.Length; i++)
        {
            dict.Add(i, str[i]);
            temp[i] = ' ';
        }

        System.Random random = new System.Random();
        dict = dict.OrderBy(x => random.Next()).ToDictionary(item => item.Key, item => item.Value);

        foreach (KeyValuePair<int, char> entry in dict) 
        {
            temp[entry.Key] = entry.Value;

            string toAssigh = new string(temp);

            toAssigh = toAssigh.Insert(entry.Key + 1, "</b></color>");
            toAssigh = toAssigh.Insert(entry.Key, "<color=#FFF408><b>");

            res.GetComponentInChildren<Text>().text = toAssigh;
            yield return new WaitForSeconds(printDelay);

            
        }
        //foreach (var ch in str)
        //{
        //    res.GetComponentInChildren<Text>().text += ch;
        //    yield return new WaitForSeconds(printDelay);
        //}
    }

    public void SetCurrentPc(PCController pcController) 
    {
        this.pcController = pcController;
    }

    public PCController GetCurrentPc() 
    {
        return pcController;
    }
}
