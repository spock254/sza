using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    string[] _choices = new[] 
    {
        "UseHead",
        "UseFace",
        "UseBody",
        "UseBagCell",
        "UseBag",
        "UseLags",
        "UseArm",
        "UseLeftHand",
        "UseRightHand",
        "UseLeftPack",
        "UseRightPack",
        "UseCard",

        "BagUse", 
        "EquipmentUse", 
        "FoodUse",
        "DrinkUse",

        "MobUse",
        "PhoneUse"
    };

    Dictionary<string, IUse> use;
    
    int _choiceIndex = 0;

    public override void OnInspectorGUI()
    {
        Item item = (Item)target;

        use = new Dictionary<string, IUse>();

        use.Add("UseHead", new UseHead());
        use.Add("UseFace", new UseFace());
        use.Add("UseBody", new UseBody());
        use.Add("UseBagCell", new UseBagCell());
        use.Add("UseBag", new UseBag());
        use.Add("UseLags", new UseLags());
        use.Add("UseArm", new UseArm());
        use.Add("UseLeftHand", new UseLeftHand());
        use.Add("UseRightHand", new UseRightHand());
        use.Add("UseLeftPack", new UseLeftPack());
        use.Add("UseRightPack", new UseRightPack());
        use.Add("UseCard", new UseCard());

        use.Add("BagUse", new BagUse());
        use.Add("EquipmentUse", new EquipmentUse());
        use.Add("FoodUse", new FoodUse());
        use.Add("DrinkUse", new DrinkUse());

        use.Add("MobUse", new MobUse());
        use.Add("PhoneUse", new PhoneUse());

        DrawDefaultInspector();

        // Update the selected choice in the underlying object
        //Debug.Log(item.itemUseData.use == null);
        //myTarget.playerName = _choices[_choiceIndex];
        //myTarget.player = GameObject.Find(myTarget.playerName);
        //EditorUtility.SetDirty(item);
        _choiceIndex = EditorGUILayout.Popup("Use", _choiceIndex, _choices);
        // AssetDatabase.SaveAssets();
        EditorGUILayout.LabelField(item.itemUseData.use.ToString());
        
        if (GUILayout.Button("Save"))
        {
            item.id = item.GenerateId();
            item.itemUseData.use = use[_choices[_choiceIndex]];
            EditorUtility.SetDirty(item);
            AssetDatabase.SaveAssets();
        }
    }
}
