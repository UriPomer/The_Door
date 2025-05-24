using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
	private CharacterStat _characterStat;
	public static Player Instance = null;
    PlayerController moveController;
    AnimationControl anim;
    public void Init()
    {
        Player.Instance = this;
        this.moveController = this.gameObject.AddComponent<PlayerController>();
        this._characterStat = GetComponent<CharacterStat>();
		_characterStat.OnHealthEqualsZero += OnHealthEqualsZero;
		this.anim = GetComponent<AnimationControl>();
        this.anim.Init();
    }

	private void OnHealthEqualsZero()
	{
		_characterStat.OnHealthEqualsZero -= OnHealthEqualsZero;
		GameMgr.Instance.SetState(GameMgr.GameState.End);
		Destroy(this.gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
        {
			CharacterStat stat = collision.GetComponent<CharacterStat>();
		}
	}
}
