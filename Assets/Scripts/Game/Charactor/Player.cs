using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
	private CharactorStat _charactorStat;
	public static Player Instance = null;
    PlayerController moveController;
    AnimationControl anim;
    public void Init()
    {
        Player.Instance = this;
        this.moveController = this.gameObject.AddComponent<PlayerController>();
        this._charactorStat = GetComponent<CharactorStat>();
		_charactorStat.OnHealthEqualsZero += OnHealthEqualsZero;
		this.anim = GetComponent<AnimationControl>();
        this.anim.Init();
    }

	private void OnHealthEqualsZero()
	{
		_charactorStat.OnHealthEqualsZero -= OnHealthEqualsZero;
		Destroy(this.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
        {
			CharactorStat stat = collision.GetComponent<CharactorStat>();
		}
	}
}
