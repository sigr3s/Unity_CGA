using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Text.RegularExpressions;

public class RuleSetEditor  : EditorWindow {
    
    public RuleSet rulesList;
    private RuleTypes viewIndex = RuleTypes.InstancePrimitives;
    
    [MenuItem ("Window/Inventory Item Editor %#e")]
    static void  Init () 
    {
        EditorWindow.GetWindow (typeof (RuleSetEditor));
    }

    void  OnEnable () {
        if(EditorPrefs.HasKey("ObjectPath")) 
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            rulesList = AssetDatabase.LoadAssetAtPath (objectPath, typeof(RuleSet)) as RuleSet;
        }
        
    }
    
    void  OnGUI () {
        GUILayout.BeginHorizontal ();
        GUILayout.Label ("Rule Editor", EditorStyles.boldLabel);
        if (rulesList != null) {
            if (GUILayout.Button("Show Rules List")) 
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = rulesList;
            }
        }
        if (GUILayout.Button("Open Rule List")) 
        {
                OpenItemList();
        }
        if (GUILayout.Button("New Rule List")) 
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = rulesList;
        }
        GUILayout.EndHorizontal ();
        
        if (rulesList == null) 
        {
            GUILayout.BeginHorizontal ();
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Rule List", GUILayout.ExpandWidth(false))) 
            {
                CreateNewItemList();
            }
            if (GUILayout.Button("Open Existing Rule List", GUILayout.ExpandWidth(false))) 
            {
                OpenItemList();
            }
            GUILayout.EndHorizontal ();
        }
            
            GUILayout.Space(20);
            
        if (rulesList != null) 
        {
            GUILayout.BeginHorizontal ();
            
            GUILayout.Space(10);
            string[] rtps = Enum.GetNames(typeof(RuleTypes));
            
            foreach(string rt in rtps){
                RuleTypes wrt = (RuleTypes) Enum.Parse(typeof(RuleTypes), rt);
                if(wrt == viewIndex) EditorGUI.BeginDisabledGroup(true);

                if (GUILayout.Button(Regex.Replace(rt, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0"), GUILayout.ExpandWidth(true)))
                {
                    viewIndex = wrt;
                }
                if(wrt == viewIndex) EditorGUI.EndDisabledGroup();
            }            
                        
            GUILayout.EndHorizontal ();

            GUILayout.Space(25);

            switch(viewIndex){
                case RuleTypes.InstancePrimitives:
                    if (rulesList.instantiateRule != null) InstantiateRuleEditor.DrawCustomEditor((InstantiateRule) rulesList.instantiateRule);
                    else GUILayout.Label ("This Rule Set is Empty.");
                break;
                default:
                    GUILayout.Label ("Not yet implemented");
                break;
            }
        }
        
        if (GUI.changed && rulesList != null) 
        {
            EditorUtility.SetDirty(rulesList);
        }
    }
    
    void CreateNewItemList () 
    {
        rulesList = CreateRuleSet.Create();
        if (rulesList) 
        {
            string relPath = AssetDatabase.GetAssetPath(rulesList);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }
    
    void OpenItemList () 
    {
        string absPath = EditorUtility.OpenFilePanel ("Select Rule Set", "", "");
        if (absPath.StartsWith(Application.dataPath)) 
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            rulesList = AssetDatabase.LoadAssetAtPath (relPath, typeof(RuleSet)) as RuleSet;
            if (rulesList) {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }
}

public class CreateRuleSet {
    [MenuItem("Assets/Create/Inventory Item List")]
    public static RuleSet  Create()
    {
        RuleSet asset = ScriptableObject.CreateInstance<RuleSet>();

        AssetDatabase.CreateAsset(asset, "Assets/RuleSet.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }
}

public enum RuleTypes{
    InstancePrimitives,
    Position,
    Scale, 
    Rotation
}
