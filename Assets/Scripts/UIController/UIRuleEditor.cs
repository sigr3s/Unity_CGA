using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIRuleEditor : MonoBehaviour {

	public List<string> rules;
	public Transform content;
	public GameObject prefab;
	public InputField input;
	public InputField jsonInp;
	public CGAInterpreter interp;
	public GameObject ErrorPanel;
	public OnItemRemoved onElementRemoved = new OnItemRemoved();

	public List<string> ex0;
	public List<string> ex1;
	public List<string> ex2;
	
	
	public void AddRule(){
		if(!string.IsNullOrEmpty(input.text)){
			string r = input.text;
			AddRule(r);
		}
	}

	public void AddRule(string ru){
		if(interp.GetRuleForInput(ru) != null){
			GameObject g = Instantiate(prefab, content);
			UIRule uir = g.GetComponent<UIRule>();
			input.text = "";
			rules.Add(ru);
			uir.SetValues(rules.Count-1, ru, this);
		}
		else{
			StartCoroutine(ShowError());
		}
	}

	public void Start(){
		if(interp.inputs != null){
			foreach(string s in interp.inputs){
				AddRule(s);
			}
		}
	}

	IEnumerator ShowError(){
		ErrorPanel.SetActive(true);
		yield return new WaitForSeconds(3);
		ErrorPanel.SetActive(false);
	}
	
	public void RemoveAt(int id){
		rules.RemoveAt(id);
		onElementRemoved.Invoke(id);
	}

	public void Process(){
		interp.Process(rules);
	}

	public void CleanList(){
		rules.Clear();
 		var allChildren = content.GetComponentsInChildren<Transform>();
		for (var ac = 0; ac < allChildren.Length; ac ++){
			if(allChildren[ac].gameObject != content.gameObject) Destroy(allChildren[ac].gameObject);
		}
	}

	public void ShowCurrentJSON(){
		Creation c = new Creation{rules = rules, version = "1.0"};
		string json = JsonUtility.ToJson(c, true);
		jsonInp.text = json;
		TextEditor te = new TextEditor();
		te.text = json;
		te.SelectAll();
		te.Copy();
	}

	public void LoadFromJSON(){
		if(!string.IsNullOrEmpty(jsonInp.text)){
			Creation c = JsonUtility.FromJson<Creation>(jsonInp.text);
			if(c.rules != null && c.rules.Count > 0){
				CleanList();
				foreach(string s in c.rules){
					AddRule(s);
				}
				Process();
			}
		}

	}

	public void LoadExample(int i){
		switch(i){
			case 0:
				rules.Clear();
				foreach(string s in ex0){
					AddRule(s);
				}
				interp.Process(rules);
			break;
			case 1:
				rules.Clear();
				foreach(string s in ex1){
					AddRule(s);
				}
				interp.Process(rules);
			break;
			case 2:
				rules.Clear();
				foreach(string s in ex2){
					AddRule(s);
				}
				interp.Process(rules);	
			break;
		}
	}

	[ContextMenu("S0")]
	public void SaveToEX0(){
		ex0 = rules;
	}

	[ContextMenu("S1")]
	public void SaveToEX1(){
		ex1 = rules;		
	}
	
	[ContextMenu("S2")]
	public void SaveToEX2(){
		ex2 = rules;		
	}

}

[System.Serializable]
public class OnItemRemoved : UnityEvent<int>{

}
