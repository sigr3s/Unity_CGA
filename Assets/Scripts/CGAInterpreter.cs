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
		ProcessInput(inputs);
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
		ProcessInput(processedInputs);
		FocusCameraOnGameObject(Camera.main, root);
	}
	
	void ProcessInput(List<string> inp){
		for(int i = 0; i < inp.Count; i++){
			IRule ir = GetRuleForInput(inp[i]);
			if(ir != null){
				ir.Execute(inp[i], ref context);
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
