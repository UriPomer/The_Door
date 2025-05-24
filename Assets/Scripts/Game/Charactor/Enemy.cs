using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{ 
	private CharacterStat _characterStat;
	private SpeedMove speedMove;

	private void Awake()
	{
		_characterStat = GetComponent<CharacterStat>();
		speedMove = gameObject.AddComponent<SpeedMove>();
	}

	private void Start()
	{
		speedMove.speed = 1f;
		speedMove.Attach(transform.gameObject, GameObject.FindGameObjectWithTag("Player"));
		_characterStat.OnHealthEqualsZero += OnHealthEqualsZero;
	}

	private void OnHealthEqualsZero()
	{
		GameObject expBall = Resources.Load<GameObject>("Prefabs/Drops/ExpBall");
		Instantiate(expBall, transform.position, Quaternion.identity);
		_characterStat.OnHealthEqualsZero -= OnHealthEqualsZero;
		Destroy(this.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			CharacterStat stat = collision.GetComponent<CharacterStat>();
			stat.TakeDamage(_characterStat.GetStat(Stat.Stats.Damage));
		}
	}
}
