using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{ 
	private CharactorStat _charactorStat;
	private SpeedMove speedMove;

	private void Awake()
	{
		_charactorStat = GetComponent<CharactorStat>();
		speedMove = gameObject.AddComponent<SpeedMove>();
	}

	private void Start()
	{
		speedMove.speed = 1f;
		speedMove.Attach(transform.gameObject, GameObject.FindGameObjectWithTag("Player"));
		_charactorStat.OnHealthEqualsZero += OnHealthEqualsZero;
	}

	private void OnHealthEqualsZero()
	{
		GameObject expBall = Resources.Load<GameObject>("Prefabs/Drops/ExpBall");
		Instantiate(expBall, transform.position, Quaternion.identity);
		_charactorStat.OnHealthEqualsZero -= OnHealthEqualsZero;
		Destroy(this.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			CharactorStat stat = collision.GetComponent<CharactorStat>();
			stat.TakeDamage(_charactorStat.GetStat(CharactorStat.Stats.Damage));
		}
	}
}
