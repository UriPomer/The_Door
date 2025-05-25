using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowFireSummon : A_Summon
{
	protected override void Start()
	{
		System.Type type = System.Type.GetType(moveMethodType.ToString());
		if (moveMethodType == CurveTypeEnum.CurveType.Custom)
		{
			if (moveCustomCurve == null)
			{
				throw new System.Exception("moveMethodType is null");
			}
			moveMethod = moveCustomCurve;
		}
		else
		{
			moveMethod = (IMoveMethod)gameObject.AddComponent(type);
		}
		base.Start();
	}

}
