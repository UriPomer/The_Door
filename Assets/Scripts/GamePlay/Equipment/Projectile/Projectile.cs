using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using static Projectile;
using static Stat;

public class Projectile : MonoBehaviour
{
	[Header("Basic")]
	protected SpriteRenderer spriteRenderer;
	protected Rigidbody2D rb;
	protected Animator animator;
	protected new Collider2D collider;
	protected CharacterStat playerAttributes;

	[Header("Projectile")]
	protected A_Weapon weapon;

	[Header("Follow")]
	protected GameObject target;
	protected IMoveMethod moveMethod;
	[SerializeField] public CurveTypeEnum.CurveType moveMethodType;
	[SerializeField] public CustomCurve moveCustomCurve;

	public enum ProjectileState
	{
		OnRoad,
		Hited,
		EnemyMiss,
	}

	public enum ProjectileType
	{
		Player,
		Enemy,
	}

	[SerializeField] protected ProjectileType projectileType;
	[SerializeField] public ProjectileState projectileState;

	protected void Awake()
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
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		collider = GetComponent<Collider2D>();
		playerAttributes = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStat>();
	}


	protected virtual IEnumerator DoAttack()
	{
		moveMethod.Attach(this.gameObject, target);
		Vector2 lastPosition = new Vector2(1000,1000);
		while (target != null)
		{
			if (target == null)
			{
				projectileState = ProjectileState.EnemyMiss;
				Destroy(this.gameObject);
			}
			if (Vector2.Distance(lastPosition,transform.position) <= 0.01f)
			{
				projectileState = ProjectileState.EnemyMiss;
				Destroy(this.gameObject);
			}
			lastPosition = transform.position;
			yield return new WaitForSeconds(0.1f);
		}
		if (target == null)
		{
			projectileState = ProjectileState.EnemyMiss;
			Destroy(this.gameObject);
		}
		yield return null;
	}

	protected virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player" && this.projectileType == ProjectileType.Enemy)
		{
			collision.gameObject.GetComponent<CharacterStat>().TakeDamage(playerAttributes.GetStat(Stats.Damage));
			projectileState = ProjectileState.Hited;
			Destroy(this.gameObject);
		}
		if(collision.gameObject.tag == "Enemy" && this.projectileType == ProjectileType.Player)
		{
			collision.gameObject.GetComponent<CharacterStat>().TakeDamage(playerAttributes.GetStat(Stats.Damage));
			projectileState = ProjectileState.Hited;
			Destroy(this.gameObject);
		}
	}

	protected virtual void OnDestroy()
	{
		if(projectileState == ProjectileState.Hited)
		{
			weapon.StartReloading();
		}
		else if(projectileState == ProjectileState.EnemyMiss)
		{
			weapon.StartReady();
		}
		StopAllCoroutines();
	}

	public void SetWeapon(A_Weapon weapon)
	{
		this.weapon = weapon;
	}

	public void SetTarget(GameObject target)
	{
		this.target = target;
		StartCoroutine(DoAttack());
	}
}
