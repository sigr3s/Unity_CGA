using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstantiateRule : IRule {
	public string pattern;
	public List<InstantiateOption> options;
	public void Execute(string input, ref CGAContext context){
        string primitive = ParsingHelper.QSString(input);
        
		foreach(InstantiateOption option in options){
			if(option.name == primitive){
		       GameObject ig =   GameObject.CreatePrimitive(option.primitive);
               ig.transform.SetParent(context.root);
               ig.transform.localPosition = context.current.position;
               ig.transform.localRotation = context.current.rotation;
               ig.transform.localScale = context.current.scale;
            }
		}
	}

	public void Validate()
	{	
        throw new System.NotImplementedException();
    }

    public string GetPattern()
    {
        return pattern;
    }

    public void SetPattern(string pattern)
    {
        this.pattern = pattern;
    }

}


//TOD: Allow this
[System.Serializable]
public class InstantiateOption{
	public string name;
    public PrimitiveType primitive;
}
