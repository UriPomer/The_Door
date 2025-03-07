using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class A_TargetSpawner : MonoBehaviour
{
	protected int numberOfTargets;
    protected List<GameObject> targets = new();
	[SerializeField] protected GameObject emptyTargetPrefab;

	protected abstract void SpawnTargets(int num);

	public List<GameObject> GetTargets()
	{
		return targets;
	}

	public void SetNumOfTargets(int num)
	{
		numberOfTargets = num;
		SpawnTargets(numberOfTargets);
	}
}
