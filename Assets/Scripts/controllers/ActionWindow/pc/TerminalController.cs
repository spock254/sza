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

    void Start()
    {
        interpreter = GetComponent<Interpreter>();    
    }

    private void OnGUI()
    {
        if (isOpen) 
        { 
            terminalInput.ActivateInputField();
            terminalInput.Select();
        }


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

            //res.GetComponentInChildren<Text>().text = interpretation[i];
            res.GetComponentInChildren<Text>().text = string.Empty;
            StartCoroutine(PrintWithDilay(res, interpretation[i]));
            
        }
        //StartCoroutine(AddInterpriterLinesDilay(interpretation));

        

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

}
