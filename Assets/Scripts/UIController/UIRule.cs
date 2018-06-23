using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRule : MonoBehaviour {
	[Header("Automatic")]
	public int id;
	public string rule;
	[Header("Set from ui")]
	public Button but;
	public Text ruleText;

	UIRuleEditor ured;

	public void Remove(){
		if(ured != null){
			ured.RemoveAt(id);
		}
		Destroy(gameObject);
	}

	public void SetValues(int ind, string r, UIRuleEditor cont){
		id = ind;
		rule = r;
		ured = cont; 
		cont.onElementRemoved.AddListener(ElementRemoved);
		ruleText.text = rule;
	}

    private void ElementRemoved(int i)
    {
		if(id > i) id--;
    }

    void Start(){
		but.onClick.AddListener(Remove);
	}
}
