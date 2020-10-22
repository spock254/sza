using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
[CreateAssetMenu(fileName = "Data", menuName = "Quest/QuestDialog")]
public class QuestDialog : ScriptableObject
{
    [TextArea]
    public string dialog;

}
