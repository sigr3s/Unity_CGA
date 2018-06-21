using UnityEngine;
using UnityEditor;
using System.Collections.Generic;


[CustomEditor(typeof(InstantiateRule))]
public class InstantiateRuleEditor : Editor
{
    public static  void DrawCustomEditor(InstantiateRule ir)
    {
        GUILayoutOption[] options  = {};
        ir.pattern = EditorGUILayout.TextField ("Pattern", ir.pattern);
        
        GUILayout.Space(10);  

        if(ir.options == null){
            ir.options = new List<InstantiateOption>();
        }
        EditorGUILayout.LabelField ("Options", "", GUILayout.ExpandWidth(false));
        EditorGUI.indentLevel++;

        for(int i = 0; i < ir.options.Count; i++){
            EditorGUILayout.BeginHorizontal();
                ir.options[i].name = EditorGUILayout.TextField ("Name",  ir.options[i].name); 
            if (GUILayout.Button("Remove", GUILayout.ExpandWidth(false))) 
            {
                ir.options.RemoveAt(i);
                return;
            }
            EditorGUILayout.EndHorizontal();
            ir.options[i].primitive =(PrimitiveType) EditorGUILayout.EnumPopup ("Primitive", ir.options[i].primitive, options);                
            GUILayout.Space(5);  
        }
        if (GUILayout.Button("Add Option", GUILayout.ExpandWidth(false))) 
        {
            InstantiateOption o = new InstantiateOption();
            o.name = "Option " + (ir.options.Count+1);
            ir.options.Add(o);
        }
        EditorGUI.indentLevel--;
        GUILayout.Space(10);     
    }
}
