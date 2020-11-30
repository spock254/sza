using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DebugController : MonoBehaviour
{
    bool showConsole;
    string input;

    public static DebugCommand PLAYER_SPEED;
    public static DebugCommand FPS;
    public static DebugCommand SCENE_LOAD;
    public static DebugCommand MOTION_BLUR;
    public static DebugCommand MOTION_BLUR_INTENCITY;
    public static DebugCommand MOTION_BLUR_CLAMP;

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

        MOTION_BLUR = new DebugCommand("motion_blur", "change scene", "motion_blur <on/off>", (arg) =>
        {
            Volume volume = GameObject.Find("Global Volume").GetComponent<Volume>();
            MotionBlur motionBlur = null;
            
            volume.profile.TryGet(out motionBlur);

            if (arg == "on")
            {
                motionBlur.active = true;
            }
            else if (arg == "off") 
            {
                motionBlur.active = false;
            }
        });

        MOTION_BLUR_INTENCITY = new DebugCommand("motion_blur_intensity", "motion blure intencity", "motion_blur_intensity <int>", (arg) =>
        {
            Volume volume = GameObject.Find("Global Volume").GetComponent<Volume>();
            MotionBlur motionBlur = null;

            volume.profile.TryGet(out motionBlur);

            motionBlur.intensity.value = float.Parse(arg, CultureInfo.InvariantCulture.NumberFormat);
        });

        MOTION_BLUR_CLAMP = new DebugCommand("motion_blur_clamp", "motion blure clamp", "motion_blur_clamp <int>", (arg) =>
        {
            Volume volume = GameObject.Find("Global Volume").GetComponent<Volume>();
            MotionBlur motionBlur = null;

            volume.profile.TryGet(out motionBlur);

            motionBlur.clamp.value = float.Parse(arg, CultureInfo.InvariantCulture.NumberFormat);
        }); ;

    commandList = new List<object>
        {
            PLAYER_SPEED,
            FPS,
            SCENE_LOAD,
            MOTION_BLUR,
            MOTION_BLUR_INTENCITY,
            MOTION_BLUR_CLAMP
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
