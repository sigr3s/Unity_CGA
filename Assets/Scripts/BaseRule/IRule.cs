using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRule
{
	void Execute(string input, ref CGAContext context);
	void Validate();
	string GetPattern();
	void SetPattern(string pattern);
}


