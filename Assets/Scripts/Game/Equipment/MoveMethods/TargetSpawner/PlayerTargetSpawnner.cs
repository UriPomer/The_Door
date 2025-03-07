using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetSpawnner : A_TargetSpawner
{
	public static PlayerTargetSpawnner Instance;
	Transform playerTransform;

	IMoveMethod moveMethod;

	private void Awake()
	{
		moveMethod = gameObject.AddComponent<PID>();
		if(Instance == null)
		{
			Instance = this;
		}
	}

	protected override void SpawnTargets(int num)
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		GameObject spawnedObject = Instantiate(emptyTargetPrefab);
		spawnedObject.transform.SetParent(playerTransform);
		PlayerController playerController = playerTransform.GetComponent<PlayerController>();
		if(playerController.direction.x > 0 )
		{
			spawnedObject.transform.position = playerTransform.position + new Vector3(-1, 0, 0);
		}
		else if(playerController.direction.x < 0)
		{
			spawnedObject.transform.position = playerTransform.position + new Vector3(1, 0, 0);
		}
		GameObject targetObj = Instantiate(emptyTargetPrefab);
		targets.Add(targetObj);
		moveMethod.Attach(targetObj, spawnedObject);
	}
}
