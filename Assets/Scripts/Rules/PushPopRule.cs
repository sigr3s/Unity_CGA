using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PushPopRule : IRule
{
    public string pattern;
    public void Execute(string input, ref CGAContext context)
    {
       if(input == "["){
           context.stack.Push(context.current);
       }
       else if (input == "]"){
           if(context.stack != null && context.stack.Count>0) context.current = context.stack.Pop();
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