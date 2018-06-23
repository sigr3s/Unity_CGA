using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InstantiateRule : IRule {
	public string pattern;
	public List<InstantiateOption> options;
	public void Execute(string input, ref CGAContext context){
        string primitive = ParsingHelper.QSString(input);
        
        string target = ParsingHelper.GetTarget(input);
        List<GameObject> targetRoots = null;
        if(!string.IsNullOrEmpty(target) && context.namedObjects.ContainsKey(target)){
            targetRoots = context.namedObjects[target];
        }

        string[] destinations = ParsingHelper.GetDest(input);

		foreach(InstantiateOption option in options){
			if(option.name == primitive){

                if(targetRoots == null){
		            GameObject ig =   GameObject.CreatePrimitive(option.primitive);
                    ig.transform.SetParent(context.root);
                    ig.transform.localPosition = context.current.position;
                    if(option.primitive == PrimitiveType.Cube) ig.transform.localPosition += new Vector3(0 , context.current.scale.y/2, 0);
                    else ig.transform.localPosition+= new Vector3(0 , context.current.scale.y, 0);
                    ig.transform.localRotation = context.current.rotation;
                    ig.transform.localScale = context.current.scale;
                    
                    if(destinations.Length > 0){
                        if(context.namedObjects.ContainsKey(destinations[0])){
                            context.namedObjects[destinations[0]].Add(ig);
                        }
                        else{
                            List<GameObject> gol = new List<GameObject>();
                            gol.Add(ig);
                            context.namedObjects.Add(destinations[0], gol);
                        }
                        ig.name = destinations[0];
                    }

                }
                else{
                    foreach(GameObject tr in targetRoots){
                        GameObject ig =   GameObject.CreatePrimitive(option.primitive);
                        ig.transform.SetParent(tr.transform);
                        ig.transform.localPosition = Vector3.zero;
                        ig.transform.localRotation = Quaternion.identity;
                        ig.transform.localScale = Vector3.one;

                        if(destinations.Length > 0){
                            if(context.namedObjects.ContainsKey(destinations[0])){
                                context.namedObjects[destinations[0]].Add(ig);
                            }
                            else{
                                List<GameObject> gol = new List<GameObject>();
                                gol.Add(ig);
                                context.namedObjects.Add(destinations[0], gol);
                            }
                            ig.name = destinations[0];
                        }
                    }
                    

                }
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
