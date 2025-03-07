using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A_Summon : A_Weapon
{
	[Header("Follow")]
	protected GameObject followTarget;
	protected IMoveMethod moveMethod;
	[SerializeField] public CurveTypeEnum.CurveType moveMethodType;
	[SerializeField] public CustomCurve moveCustomCurve;

	protected List<GameObject> hitedGameObjects = new();
	protected bool Hited = false;

	protected override IEnumerator OnReadyState()
	{
		moveMethod.Attach(gameObject, followTarget);
		return base.OnReadyState();
	}

	public void SetTarget(GameObject gameObject)
	{
		followTarget = gameObject;
	}

}
