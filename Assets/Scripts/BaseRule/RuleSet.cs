using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuleSet : ScriptableObject {
	public InstantiateRule instantiateRule;
	public PositionRule positionRule;
	public RotationRule rotiationRule;
	public ScaleRule scaleRule;
	public PushPopRule pushPopRule;
}
