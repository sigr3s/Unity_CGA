using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SplitRule : IRule {
    public string pattern;
    public void Execute(string input, ref CGAContext context)
    {
        string ax = ParsingHelper.QSString(input);
        SplitAxis sa =(SplitAxis) Enum.Parse(typeof(SplitAxis), ax);;

        string target = ParsingHelper.GetTarget(input);
        List<GameObject> t= null;
        if(!string.IsNullOrEmpty(target) && context.namedObjects.ContainsKey(target)){
            t = context.namedObjects[target];
            float size;
            foreach(GameObject g in t){
                switch(sa){
                    case SplitAxis.X:
                        size= g.transform.localScale.x;
                    break;
                    case SplitAxis.Y:
                        size = g.transform.localScale.y;
                    break;
                    case SplitAxis.Z:
                        size = g.transform.localScale.z;
                    break;
                }
            }
        }
        else{
            Debug.LogWarning("Cannot split without any target");
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

public enum SplitAxis{
    X,
    Y,
    Z
}