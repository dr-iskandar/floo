using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UILeaderPanelController : MonoBehaviour 
{
	#region VARIABLES
	public Transform parentLeadPoint;
	public Transform parentLeadTime;
	public Transform parentLeadEat;

	public ScrollRect scrollRectPoint;
	public ScrollRect scrollRectTime;
	public ScrollRect scrollRectEat;
	#endregion

	#region INIT
	public void InitPanelRefresh ()
	{
		DefaultPanelCondition ();
	}

	public Transform InitParentPanelPoint ()
	{
		return parentLeadPoint;
	}

	public Transform InitParentPanelTime ()
	{
		return parentLeadTime;
	}

	public Transform InitParentPanelEat ()
	{
		return parentLeadEat;
	}

	#endregion

	#region UI FUNCT

	void DefaultPanelCondition ()
	{
		scrollRectPoint.verticalNormalizedPosition = 1.0f;
		scrollRectTime.verticalNormalizedPosition = 1.0f;
		scrollRectEat.verticalNormalizedPosition = 1.0f;
	}

	#endregion

}
