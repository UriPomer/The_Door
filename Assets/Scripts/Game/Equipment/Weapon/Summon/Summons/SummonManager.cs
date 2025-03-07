using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonManager : MonoBehaviour
{
    [SerializeField] public int numberOfObjects;
    [SerializeField] private GameObject summonPrefab;
    [SerializeField] private A_TargetSpawner targetSpawner;

	private void Start()
	{
		if(targetSpawner != null)
		{
			targetSpawner.SetNumOfTargets(numberOfObjects);
			if (summonPrefab != null)
			{
				for (int i = 0; i < numberOfObjects; i++)
				{
					if (targetSpawner.GetTargets().Count <= i)
					{
						return;
					}
					GameObject summonObj = Instantiate(summonPrefab);
					summonObj.transform.parent = transform;
					A_Summon summon = summonObj.GetComponent<A_Summon>();
					if(summon == null)
					{
						throw new System.Exception("Summon is null");
					}
					summon.SetTarget(targetSpawner.GetTargets()[i]);
				}
			}
			else
			{
				throw new System.Exception("SummonPrefab is null");
			}
		}
		else
		{
			throw new System.Exception("targetSpawner is null");
		}
	}
}
