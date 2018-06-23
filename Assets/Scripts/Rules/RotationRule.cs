using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RotationRule : IRule
{
    public string pattern;
    public void Execute(string input, ref CGAContext context)
    {
        string target = ParsingHelper.GetTarget(input);
        List<GameObject> t= null;
        if(!string.IsNullOrEmpty(target) && context.namedObjects.ContainsKey(target)){
            t = context.namedObjects[target];
        }
        Vector3 r =  ParsingHelper.CSVector3(input);
        if(t == null) context.current.rotation *= Quaternion.Euler(r);
        else{
            foreach(GameObject g in t) g.transform.Rotate(r);
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