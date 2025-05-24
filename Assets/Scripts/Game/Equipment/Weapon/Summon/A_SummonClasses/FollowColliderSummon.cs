using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowColliderSummon : A_Summon
{
	protected IMoveMethod fireMoveMethod;
	[SerializeField] public CurveTypeEnum.CurveType fireMoveMethodType;
	[SerializeField] public CustomCurve fireCustomCurve;
	protected override void Start()
	{
		System.Type type = System.Type.GetType(moveMethodType.ToString());
		if (moveMethodType == CurveTypeEnum.CurveType.Custom)
		{
			if (moveCustomCurve == null)
			{
				throw new System.Exception("readyCustomCurve is null");
			}
			moveMethod = moveCustomCurve;
		}
		else
		{
			moveMethod = (IMoveMethod)gameObject.AddComponent(type);
		}
		type = System.Type.GetType(fireMoveMethodType.ToString());
		if (fireMoveMethodType == CurveTypeEnum.CurveType.Custom)
		{
			if (fireCustomCurve == null)
			{
				throw new System.Exception("fireCustomCurve is null");
			}
			fireMoveMethod = fireCustomCurve;
		}
		else
		{
			fireMoveMethod = (IMoveMethod)gameObject.AddComponent(type);
		}
		base.Start();
	}
	protected override IEnumerator OnReadyState()
	{
		return base.OnReadyState();
	}
	protected override IEnumerator OnReloadingState()
	{
		return base.OnReloadingState();
	}
	protected override IEnumerator OnFiringState()
	{
		moveMethod.Stop();
		fireMoveMethod.Attach(gameObject, fireTarget);
		while(!Hited)
		{
			if(fireTarget == null)
			{
				fireTarget = FindClosestObject.FindExcept(this.gameObject, hitedGameObjects, ObjectType.Enemy);
				if(fireTarget == null)
				{
					Hited = true;
					break;
				}
				fireMoveMethod.Attach(gameObject, fireTarget);
			}
			yield return null;
		}
		if(Hited == true)
		{
			StartCoroutine(OnReloadingState());
		}
		hitedGameObjects.Clear();
		fireMoveMethod.Stop();
		Hited = false;
		fireTarget = null;
		StartCoroutine(OnReadyState());
		yield return null;
	}
	protected virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject == fireTarget)
		{
			hitedGameObjects.Add(collision.gameObject);
			collision.gameObject.GetComponent<CharacterStat>().TakeDamage(GetStat(Stats.Damage));
			if (hitedGameObjects.Count < GetStat(Stats.HitCount) - 1)
			{
				fireMoveMethod.Stop();
				fireTarget = FindClosestObject.FindExcept(this.gameObject, hitedGameObjects, ObjectType.Enemy);
				if (fireTarget == this.gameObject || fireTarget == null)
				{
					Hited = true;
					return;
				}
				fireMoveMethod.Attach(this.gameObject, fireTarget);
			}
			else
			{
				Hited = true;
				return;
			}
		}
	}

	protected virtual void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy")) {
			
		}
		if (collision.gameObject == fireTarget)
		{
			hitedGameObjects.Add(collision.gameObject);
			if (hitedGameObjects.Count < GetStat(Stats.HitCount) - 1)
			{
				fireMoveMethod.Stop();
				fireTarget = FindClosestObject.FindExcept(this.gameObject, hitedGameObjects, ObjectType.Enemy);
				if (fireTarget == this.gameObject || fireTarget == null)
				{
					Hited = true;
					return;
				}
				fireMoveMethod.Attach(this.gameObject, fireTarget);
			}
			else
			{
				Hited = true;
				return;
			}
		}
	}
	protected override bool FireCondition()
	{
		return base.FireCondition();
	}
	public void SetFireTarget(GameObject gameObject)
	{
		fireTarget = gameObject;
	}
}
