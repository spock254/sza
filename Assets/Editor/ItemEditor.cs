using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    string[] _choices = new[] { "BagUse", "EquipmentUse" , "FoodUse" };

    Dictionary<string, IUse> use;
    
    int _choiceIndex = 0;

    public override void OnInspectorGUI()
    {
        Item item = (Item)target;

        use = new Dictionary<string, IUse>();

        use.Add("BagUse", new BagUse());
        use.Add("EquipmentUse", new EquipmentUse());
        use.Add("FoodUse", new FoodUse());

        DrawDefaultInspector();

        // Update the selected choice in the underlying object
        //Debug.Log(item.itemUseData.use == null);
        //myTarget.playerName = _choices[_choiceIndex];
        //myTarget.player = GameObject.Find(myTarget.playerName);
        //EditorUtility.SetDirty(item);
        _choiceIndex = EditorGUILayout.Popup("Use", _choiceIndex, _choices);
        // AssetDatabase.SaveAssets();
        if (GUILayout.Button("Save"))
        {
            item.itemUseData.use = use[_choices[_choiceIndex]];
            
            EditorUtility.SetDirty(item);
            AssetDatabase.SaveAssets();
        }
    }
}
