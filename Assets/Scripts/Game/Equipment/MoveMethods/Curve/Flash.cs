using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour, IMoveMethod
{
	void IMoveMethod.Attach(GameObject location, GameObject targetlocation)
	{
		StartCoroutine(Move(location.transform, targetlocation.transform));
	}


	void IMoveMethod.Stop()
	{
		StopAllCoroutines();
	}


	public IEnumerator Move(Transform location, Transform targetlocation)
	{
		while (true)
		{
			location.transform.position = targetlocation.transform.position;
			yield return null;
		}
	}
}
