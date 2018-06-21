using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CGAInterpreter : MonoBehaviour {
	public RuleSet rules;
	public List<string> inputs;
	CGAContext context;

	public Dictionary<string, IRule> ruleSetMapping = new Dictionary<string, IRule>();

	void Start () {
		GameObject root = new GameObject("ROOT");
		context =new CGAContext();
		context.current = new CGATransform {position = Vector3.zero, rotation = Quaternion.identity, scale = Vector3.one};
		context.root = root.transform;
		context.stack = new Stack<CGATransform>();
		ProcessInput(inputs);
	}
	
	void ProcessInput(List<string> inp){
		for(int i = 0; i < inp.Count; i++){
			IRule ir = GetRuleForInput(inp[i]);
			if(ir != null){
				ir.Execute(inp[i], ref context);
			}
		}
	}

	IRule GetRuleForInput(string i){
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
}
