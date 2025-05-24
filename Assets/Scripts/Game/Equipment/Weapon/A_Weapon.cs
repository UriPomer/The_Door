using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public abstract class A_Weapon : Stat
{
	[Header("Basic")]
	protected Rigidbody2D rb;
	protected Animator animator;
	protected new Collider2D collider;
	protected CharacterStat playerStats;

	[SerializeField] protected readonly float DETERMINE_TIME_INTERVAL = 0.1f;

	[Header("ProjectilePrefab")]
	[SerializeField] protected GameObject projectilePrefab;

	[Header("State")]
	[SerializeField] protected State state;

	[Header("Fire")]
	protected GameObject fireTarget;


	protected virtual void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		collider = GetComponent<Collider2D>();
		playerStats = GameObject.FindWithTag("Player").GetComponent<CharacterStat>();
		if (playerStats == null)
		{
			throw new System.Exception("PlayerCharatorStat is null");
		}
		state = State.Reloading;
		StartCoroutine(OnReloadingState());
		StartCoroutine(OnReadyState());
	}
	protected enum State
	{
		Ready,
		Reloading,
		Firing,
	}
	protected virtual IEnumerator OnReadyState()
	{
		while (state != State.Firing)
		{
			if (FireCondition() && state == State.Ready)
			{
				state = State.Firing;
				StartCoroutine(OnFiringState());
				yield break;
			}
			yield return new WaitForSeconds(DETERMINE_TIME_INTERVAL);
		}
		yield return null;
	}
	protected virtual IEnumerator OnFiringState()
	{
		Vector2 direction = (fireTarget.transform.position - transform.position).normalized;
		float etAngle = Vector2.Angle(Vector2.up, direction);
		if(direction.x > 0)
		{
			etAngle = -etAngle;
		}


		GameObject projectileObj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
		projectileObj.transform.eulerAngles = new Vector3(0, 0, etAngle + projectilePrefab.transform.eulerAngles.z);
		projectileObj.GetComponent<Projectile>().SetWeapon(this);
		projectileObj.GetComponent<Projectile>().SetTarget(fireTarget);
		yield return null;
	}
	protected virtual IEnumerator OnReloadingState()
	{
		state = State.Reloading;
		yield return new WaitForSeconds(GetStat(Stats.FireInterval));
		state = State.Ready;
		StartCoroutine(OnReadyState());
	}
	protected virtual bool FireCondition()
	{
		if (state != State.Ready)
		{
			return false;
		}
		GameObject gameObject = FindClosestObject.Find(this.gameObject, ObjectType.Enemy);
		if (gameObject == null)
		{
			return false;
		}
		if (Vector2.Distance(gameObject.transform.position, transform.position) < GetStat(Stats.Range))
		{
			fireTarget = gameObject;
			return true;
		}
		else
		{
			return false;
		}
	}

	public void StartReloading()
	{
		state = State.Reloading;
		StartCoroutine(OnReloadingState());
	}

	public void StartReady()
	{
		state = State.Ready;
		StartCoroutine(OnReadyState());
	}
}
