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
        string[] destinations = ParsingHelper.GetDest(input);
        CSF csfVals = ParsingHelper.CSFValues(input);
        List<GameObject> t= null;
        

        if(!string.IsNullOrEmpty(target) && context.namedObjects.ContainsKey(target)){
            t = context.namedObjects[target];
            List<GameObject> newNamedObjectContent = new List<GameObject>();
            Vector3 a = new Vector3(1,0,0);


            foreach(GameObject g in t){
                GameObject newg  = GameObject.Instantiate(g);
                newg.name = g.name;
                GameObject.Destroy(newg.GetComponent<MeshRenderer>());
                GameObject.Destroy(newg.GetComponent<MeshFilter>());
                newNamedObjectContent.Add(newg);
                newg.transform.SetParent(g.transform.parent);
                newg.transform.localScale = g.transform.localScale;
                newg.transform.localPosition = g.transform.localPosition;
                newg.transform.localRotation = g.transform.localRotation;

                GameObject tar = g;
                Vector3 start = g.transform.position + g.transform.lossyScale/2; 
                int s = csfVals.orderedCSF.Count;
                float size = 0;

                switch(sa){
                    case SplitAxis.X:
                        a = -g.transform.right;
                        size= g.transform.lossyScale.x;
                    break;
                    case SplitAxis.Y:
                        a = -g.transform.up;
                        size = g.transform.localScale.y;     
                    break;
                    case SplitAxis.Z:
                        a = -g.transform.forward;
                        size = g.transform.localScale.z;    
                    break;
                }

                List<float> sizes = new List<float>();
                float sizeOfDynamic = size;
                float perc = 0;

                foreach(float v in csfVals.fixedValues) {
                    sizeOfDynamic -= (size*v);
                    perc += v;
                }
                if(perc > 1){
                    Debug.LogWarning("You messed up something with percentages...");
                }
                
                int relativeCount = 0;
                foreach(int v in csfVals.relativeValues){
                    relativeCount += v;
                }
                
                float acumulatedPosition = 0;

                for(int i = 1; i < s;i ++){
                    if(csfVals.orderedCSF[i-1].isRelative){
                        acumulatedPosition += (csfVals.relativeValues[csfVals.orderedCSF[i-1].index]/(relativeCount*1.0f))*sizeOfDynamic;
                    }else{
                        acumulatedPosition += csfVals.fixedValues[csfVals.orderedCSF[i-1].index]*size;
                    }
                    GameObject[] splited = MeshCut.Cut(tar , start + a*acumulatedPosition , a , tar.GetComponent<Renderer>().material); //((size/s) * i)
                    tar = splited[1];
                    if(destinations != null && destinations.Length > i){
                        string name = destinations[i-1];
                        splited[0].name = name;
                        if(context.namedObjects.ContainsKey(name) ){
                            context.namedObjects[name].Add(splited[0]);                                    
                        }
                        else{
                            context.namedObjects.Add(name, new List<GameObject>(){splited[0]});
                        }
                    }
                    
                    splited[0].transform.SetParent(newg.transform);
                    splited[0].transform.localScale = Vector3.one;

                    splited[1].transform.SetParent(newg.transform);
                    splited[1].transform.localScale = Vector3.one;
                    if(destinations != null && destinations.Length > i){
                        string name = destinations[i];
                        splited[1].name = name;
                        if(context.namedObjects.ContainsKey(name) ){
                            context.namedObjects[name].Add(splited[1]);
                        }
                        else{
                            context.namedObjects.Add(name, new List<GameObject>(){splited[1]});
                        }
                    }
                }

                //Rebuild dict!
                context.namedObjects[target] = newNamedObjectContent;
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