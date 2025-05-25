using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MapBlock : MonoBehaviour
{
	[Inject] MapManager _mapManager;
	
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			_mapManager.SetCurrentBlock(this);
		}
	}
}
