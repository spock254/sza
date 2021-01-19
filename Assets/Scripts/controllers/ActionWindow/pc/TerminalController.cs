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
    Vector2 initMsgListSize = Vector2.zero;
    public float printDelay = 0.1f;

    Interpreter interpreter;
    public bool isOpen = false;

    PCController pcController;

    //public List<string> history = new List<string>();
    public List<string> history = new List<string>();
    List<GameObject> responceContainer = new List<GameObject>();

    bool isInit = false;
    bool isInstaling = false;
    void Start()
    {
        initMsgListSize = msgList.GetComponent<RectTransform>().sizeDelta;
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

    bool errorOneFram = false;
    //int index = 0;
    private void OnGUI()
    {
        if (isOpen == true)
        {

            if (pcController.IsSystemInstaled() == false) 
            { 
                if (pcController.IsSystemInstaled() == false
                    && (pcController.disk == null || pcController.disk.itemOptionData.text != "installer") 
                    && isInstaling == false)
                {
                    if (errorOneFram == false) 
                    {
                        userInputLine.gameObject.SetActive(false);

                //        ClearInputField();
                        //AddDirectoryLine(userInput);

                        AddContent(new List<string>()
                        {
                            "critical error [3213 .32]",
                        " ",
                        " ",
                        " ",
                        " ",
                        " ",
                        " ",
                        " ",
                        " ",
                        " "
                        });

                    
                    }

                    errorOneFram = true;
                }
                else if (pcController.IsSystemInstaled() == false
                    && (pcController.disk != null && pcController.disk.itemOptionData.text == "installer") 
                    && isInstaling == false) 
                {
                    isInstaling = true;
                    StartCoroutine(InstallingSystem());

                    //pcController.SetSystemInstall(true);
                }

                return;
            }
            //if (pcController.IsSystemInstaled() == true) 
            //{ 
            //}

            terminalInput.ActivateInputField();
            terminalInput.Select();
            
            if (isInit == false)
            {
                isInit = true;
                userInputLine.gameObject.SetActive(true);
                //AddToHistory(userInput);
                // если вышел в гайде сессии вернутся к пред юзер моду
                //if (pcController.currentMemory.userMode == CommandDB.UserMode.Guide) 
                //{
                //    CommandDB.UserMode prevUserMode = commands.GuideCommand.prevUserMode;
                //    PCMempryContent guestMemoryContent = pcController.memoryContents.Where(x => x.userMode == prevUserMode)
                //                                        .FirstOrDefault();
                //    pcController.currentMemory = guestMemoryContent;
                //}

                ClearInputField();
                //AddDirectoryLine(userInput);
                CleanResponceLines();

                int lines = AddInterpriterLines(new List<string>()
                {
                    " ",
                    " ",
                    " ",
                    " ",
                    "\t\tWelcome " + pcController.currentMemory.userName + " to <color=#FFFFFF>SYSTEM_32s</color>.",
                    " ",
                    "to familiarize yourself with the system, ",
                    "use command: guide -on",
                    " ",
                    " ",
                   " ",
                    " ",
                    " ",
                    " ",
                    " ",
                    " ",
                    " "
                });

                ScrallToButtom(lines);

                SetUserInputLineAsLastSibling();

            }
        }
        else
        {
            isInit = false;
            //CleanResponceLines();
        }


        //terminalInput.text = ScrollHistory(terminalInput.text);

        //if (terminalInput.isFocused && Input.GetKeyDown(KeyCode.UpArrow) && history.Count > 0)
        //{
        //    if (index == history.Count)
        //    {
        //        index = history.Count - 1;
        //    }

        //    terminalInput.text = history[index];
        //    StartCoroutine(ChangeCaretPosition());
        //    index++;
        //    return;
        //}
        //else if (terminalInput.isFocused && Input.GetKeyDown(KeyCode.DownArrow) && history.Count > 0)
        //{
        //    if (index == -1)
        //    {
        //        index = 0;
        //    }

        //    terminalInput.text = history[index];
        //    StartCoroutine(ChangeCaretPosition());
        //    index--;
        //    return;
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

    public void AddContent(List<string> content) 
    {
        ClearInputField();
        int lines = AddInterpriterLines(content);

        ScrallToButtom(lines);
        SetUserInputLineAsLastSibling();
    }

    void SetUserInputLineAsLastSibling() 
    {
        userInputLine.transform.SetAsLastSibling();
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

        responceContainer.Add(msg);

        msg.transform.SetSiblingIndex(msgList.transform.childCount - 1);
        msg.GetComponentsInChildren<Text>()[1].text = userInput;
    }

    int AddInterpriterLines(List<string> interpretation)
    {
        for (int i = 0; i < interpretation.Count; i++)
        {
            GameObject res = Instantiate(responceLine, msgList.transform);
            
            responceContainer.Add(res);
        
            res.transform.SetAsLastSibling();

            Vector2 msgListSize = msgList.GetComponent<RectTransform>().sizeDelta;
            msgList.GetComponent<RectTransform>().sizeDelta = new Vector2(msgListSize.x, msgListSize.y + 20);

            res.GetComponentInChildren<Text>().text = string.Empty;
            res.GetComponentInChildren<Text>().text = interpretation[i];
        }

        return interpretation.Count;
    }

    void ScrallToButtom(int lines) 
    {
        //if (lines > 0)
        //{
        //    sr.velocity = new Vector2(0, 0);
        //}
        //else 
        //{
        //    sr.verticalNormalizedPosition = 0;
        //}
        
        sr.velocity = new Vector2(0, 0);
        
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
            toAssigh = toAssigh.Insert(entry.Key, "<color=#FFFFFF><b>");

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

    IEnumerator InstallingSystem() 
    {
        for (int i = 1; i < 101; i += 5)
        {
            AddContent(new List<string>() 
            {
                "",
                "",
                "",
                "installing " + i + "%",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " "
            });

            yield return new WaitForSeconds(Random.Range(0.03f, 0.08f));
        }


        CleanResponceLines();

        AddContent(new List<string>() { "almost done...",                
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " ",
                " " });

        yield return new WaitForSeconds(3f);

        pcController.SetSystemInstall(true);
    }

    void CleanResponceLines() 
    {
        if (responceContainer.Count == 0) 
        {
            return;
        }

        foreach (var res in responceContainer)
        {
            Destroy(res);
        }

        msgList.GetComponent<RectTransform>().sizeDelta = initMsgListSize;
        responceContainer.Clear();
    }
}
