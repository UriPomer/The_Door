using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpBall : MonoBehaviour
{
	public float flyToPlayerTime = 1f;
	public float range = 10f;
	private IMoveMethod pidMove;
	private Transform player;

	private void Start()
	{
		StartCoroutine(FlyToPlayer());
		pidMove = this.gameObject.AddComponent<PID>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private IEnumerator FlyToPlayer()
	{
		yield return new WaitForSeconds(flyToPlayerTime);
		while (Vector2.Distance(player.transform.position, transform.position) >= range)
		{
			yield return new WaitForSeconds(0.2f);
		}
		pidMove.Attach(this.gameObject, player.gameObject);
	}


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			// Destroy the exp ball
			Destroy(this.gameObject);
			// Add exp to player
			GameMgr.Instance.Experience += 10;
		}
	}
}