using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    string input;

    public static DebugCommand PLAYER_SPEED;
    public static DebugCommand FPS;
    public static DebugCommand SCENE_LOAD;

    public List<object> commandList;

    private void Start()
    {
        PLAYER_SPEED = new DebugCommand("player_speed", "change player speed", "player_speed <float>", (arg) => 
        {
            GameObject player = Global.Obj.GetPlayerGameObject();
            player.GetComponent<PlayerMovement>().speed = float.Parse(arg, CultureInfo.InvariantCulture.NumberFormat);
        });

        FPS = new DebugCommand("fps", "shows fps", "fsp <on/off>", (arg) =>
        {
            GameObject fpsWindow = GameObject.Find("fpsWindow").gameObject;

            if (arg == "on")
            {
                fpsWindow.GetComponent<FPSCounter>().TurnFps(true);
            }
            else if (arg == "off") 
            {
                fpsWindow.GetComponent<FPSCounter>().TurnFps(false);
            }

        });

        SCENE_LOAD = new DebugCommand("scene", "change scene", "scene <int>", (arg) =>
        {
            ProgressSceneLoader sceneLoader = Global.Component.GetProgressSceneLoader();
            sceneLoader.LoadScene(int.Parse(arg));
        });

        commandList = new List<object>
        {
            PLAYER_SPEED,
            FPS,
            SCENE_LOAD
        };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            showConsole = !showConsole;
        }

        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            if (showConsole == true) 
            { 
                HandleInput();
                input = string.Empty;
            }
        }
    }

    void OnGUI()
    {
        if (showConsole == false) 
        {
            return;
        }

        if (Event.current.keyCode == KeyCode.Return)
        {
            if (input != string.Empty) 
            { 
                HandleInput();
            }

            input = string.Empty;
            showConsole = false;
            return;
        }


        float y = Screen.height - 30;
        float x = 0;
        GUI.Box(new Rect(x, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        GUI.SetNextControlName("MyTextField");
        input = GUI.TextField(new Rect(x + 10f, y + 5f, Screen.width - 20f, 20f), input); 
        GUI.FocusControl("MyTextField");
    }

    void HandleInput() 
    {
        if (input != null && input != string.Empty) 
        { 
            for (int i = 0; i < commandList.Count; i++)
            {
                DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

                if (input.Contains(commandBase.Id)) 
                {
                    if (commandList[i] as DebugCommand != null) 
                    {
                        string arg = input.Replace(commandBase.Id, string.Empty).Trim();
                        (commandList[i] as DebugCommand).Invoke(arg);
                    }
                }
            }
        
        }
    }
}
