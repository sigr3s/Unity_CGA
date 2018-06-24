using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshCutTest : MonoBehaviour {

	public GameObject victim;
	
	public Transform some;
	
	

	[ContextMenu("Cut")]
	public void Cut(){
		MeshCut.Cut(victim, some.position, victim.transform.right, null);
	}
}
