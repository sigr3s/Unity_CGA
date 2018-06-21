using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CGAContext  {
	public Transform root;
	public CGATransform current;
	public Stack<CGATransform> stack;

}

[System.Serializable]
public struct CGATransform{
	public Vector3 position;
	public Quaternion rotation;
	public Vector3 scale;
}
