using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMove : MonoBehaviour, IMoveMethod
{
	public float speed = 20f;
	public PIDRotate pid = new PIDRotate();

	public void Attach(GameObject location, GameObject targetlocation)
	{
		pid.KpR = 0.6f;
		StartCoroutine(pid.Rotate(targetlocation.transform, location.transform));
		StartCoroutine(Move(location.transform, targetlocation.transform));
	}

	public void Stop()
	{
		StopAllCoroutines();
	}

	public IEnumerator Move(Transform location, Transform targetlocation)
	{
		while (true)
		{
			if(targetlocation)
			location.transform.position = Vector3.MoveTowards(location.transform.position, targetlocation.transform.position, speed * Time.deltaTime);
			yield return null;
		}
	}
}
