using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PositionRule : IRule
{
    public string pattern;
    public void Execute(string input, ref CGAContext context)
    {
       Vector3 r =  ParsingHelper.CSVector3(input);
       context.current.position += r;
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