using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CGAInterpreter : MonoBehaviour {

#region Editor public vars
	public RuleSet rules;
	public List<string> inputs;
	public Creation cr;
	public Material mat;
	public Material matR;
	public Material matW;
#endregion

#region Private vars
	CGAContext context;
	GameObject root;

#endregion
	void Start () {
		root = new GameObject("ROOT");
		context =new CGAContext();
		context.current = new CGATransform {position = Vector3.zero, rotation = Quaternion.identity, scale = Vector3.one};
		context.root = root.transform;
		context.namedObjects = new Dictionary<string, List<GameObject>>();
		context.stack = new Stack<CGATransform>();
		context.material = mat;
		context.roofMat = matR;
		context.windMat = matW;
		ProcessInput(inputs);
		FocusCameraOnGameObject(Camera.main, root);
	}

	public void CleanScene(){
		Destroy(root);
		root = new GameObject("ROOT");
	}

	public void Process (List<string> processedInputs) {
		CleanScene();
		context =new CGAContext();
		context.current = new CGATransform {position = Vector3.zero, rotation = Quaternion.identity, scale = Vector3.one};
		context.root = root.transform;
		context.namedObjects = new Dictionary<string, List<GameObject>>();
		context.stack = new Stack<CGATransform>();
		context.material = mat;
		context.roofMat = matR;
		context.windMat = matW;
		ProcessInput(processedInputs);
		FocusCameraOnGameObject(Camera.main, root);
	}
	
	void ProcessInput(List<string> inp){
		float accumulatedP = 0;
		List<RandomIRule> percRules = new List<RandomIRule>();

		for(int i = 0; i < inp.Count; i++){
			IRule ir = GetRuleForInput(inp[i]);
			if(ir != null){
				float per = ParsingHelper.ExtractProbavility(inp[i]);
				if(per == 1 && accumulatedP == 0) ir.Execute(inp[i], ref context);
				else{
					accumulatedP += per;
					percRules.Add(new RandomIRule{rule = ir, p = per,inputString = inp[i]});
					
					if(accumulatedP >= 1.0f){ // maybe errors?
						float cp = Random.Range(0.0f, 1.0f);

						float acp = 0;
						bool selected = false;
						for(int j = 0; j < percRules.Count; j++){
							RandomIRule irl = percRules[j];
							acp += irl.p;
							
							if(acp > cp && !selected){
								irl.rule.Execute(irl.inputString, ref context);
								selected = true;;
							}
						}
						
						accumulatedP = 0;
						percRules = new List<RandomIRule>();
					}	
				}
			}
		}
	}

	public IRule GetRuleForInput(string i){
		if(rules != null){
			if(Regex.IsMatch(i, rules.instantiateRule.pattern, RegexOptions.IgnoreCase)){
				return rules.instantiateRule;
			}
			if(Regex.IsMatch(i, rules.positionRule.pattern, RegexOptions.IgnoreCase)){
				return rules.positionRule;
			}
			if(Regex.IsMatch(i, rules.rotiationRule.pattern, RegexOptions.IgnoreCase)){
				return rules.rotiationRule;
			}
			if(Regex.IsMatch(i, rules.scaleRule.pattern, RegexOptions.IgnoreCase)){
				return rules.scaleRule;
			}
			if(Regex.IsMatch(i, rules.pushPopRule.pattern, RegexOptions.IgnoreCase)){
				return rules.pushPopRule;
			}
			if(Regex.IsMatch(i, rules.splitRule.pattern, RegexOptions.IgnoreCase)){
				return rules.splitRule;
			}
		}
		Debug.LogWarning("No matching found for input " + i);
		return null;
	}

	Bounds CalculateBounds(GameObject go) {
		Bounds b = new Bounds(go.transform.position, Vector3.zero);
		Object[] rList = go.GetComponentsInChildren(typeof(Renderer));
		foreach (Renderer r in rList) {
			b.Encapsulate(r.bounds);
		}
		return b;
	}
	void FocusCameraOnGameObject(Camera c, GameObject go) {
		Bounds b = CalculateBounds(go);
		Vector3 max = b.size;
		float radius = Mathf.Max(max.x, Mathf.Max(max.y, max.z));
		float dist = radius /  (Mathf.Sin(c.fieldOfView * Mathf.Deg2Rad / 2f));
		Vector3 pos = ((go.transform.forward*1.5f - go.transform.right + go.transform.up)/3) * dist + b.center;
		c.transform.position = pos;
		c.transform.LookAt(b.center);
	}

}

[System.Serializable]
public struct RandomIRule{
	public IRule rule;
	public float p;
	public string inputString;
}

[System.Serializable]
public struct Creation{
	public List<string> rules;
	public string version;
}
