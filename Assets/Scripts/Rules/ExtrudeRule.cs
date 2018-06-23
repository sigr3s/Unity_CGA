using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExtrudeRule : IRule {
    public string pattern;
    public void Execute(string input, ref CGAContext context)
    {
        string ax = ParsingHelper.QSString(input);
        ExtrudeAxis sa = (ExtrudeAxis) Enum.Parse(typeof(ExtrudeAxis), ax);;
        string target = ParsingHelper.GetTarget(input);
        List<GameObject> t= null;
        if(!string.IsNullOrEmpty(target) && context.namedObjects.ContainsKey(target)){
            t = context.namedObjects[target];
            float size;
            foreach(GameObject g in t){
                switch(sa){
                    case ExtrudeAxis.X:
                    break;
                    case ExtrudeAxis.Y:
                    break;
                    case ExtrudeAxis.Z:
                    break;
                }
            }
        }
        else{
            Debug.LogWarning("Cannot extrude without any target");
            return;
        }
    }

    public string GetPattern()
    {
        return pattern;
    }

    public void SetPattern(string pattern)
    {
        this.pattern = pattern;
    }

    public void Validate()
    {
        
    }
}

public enum ExtrudeAxis{
    X,
    Y,
    Z
}