using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestEditor : EditorWindow
{
    Quest quest;

    [MenuItem("Window/Quest")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(Quest));
    }

    void OnEnable()
    {
        quest = AssetDatabase.LoadAssetAtPath("Assets/MonsterDefnList.asset",
                            typeof(Quest)) as Quest;
    }

    void OnGUI()
    {
        if (GUILayout.Button("Create New Quest events"))
        {
            quest = ScriptableObject.CreateInstance<Quest>();
            quest.questEvents = new Queue<QuestEvent>();
            AssetDatabase.CreateAsset(quest, "Assets/Quest.asset");
            AssetDatabase.SaveAssets();
        }

        if (GUILayout.Button("Add quest event"))
        {
            QuestEvent questEvent = ScriptableObject.CreateInstance<QuestEvent>();
            quest.questEvents.Enqueue(questEvent);
            AssetDatabase.AddObjectToAsset(questEvent, quest);
            AssetDatabase.SaveAssets();
        }

        if (GUI.changed) 
        {
            EditorUtility.SetDirty(quest);
        }
    }
}
