using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCurve : MonoBehaviour, IMoveMethod
{
	public AnimationCurve curve; // 在Inspector窗口中分配曲线
	public float referenceSpeed = 20f;

	private float speed;

	private PIDRotate pid;

	public void Attach(GameObject location, GameObject targetlocation)
	{
		pid = new PIDRotate();
		StartCoroutine(pid.Rotate(targetlocation.transform, location.transform));
		StartCoroutine(Move(location.transform, targetlocation.transform));
	}

	public void Stop()
	{
		StopAllCoroutines();
	}

	public IEnumerator Move(Transform location, Transform targetlocation)
	{
		if(targetlocation == null)
		{
			yield break;
		}
		Vector2 startPosition = location.position;
		float startDistance = Vector2.Distance(startPosition, targetlocation.position);
		float distance;
		while (true)
		{
			if (targetlocation == null)
			{
				yield break;
			}
			distance = Vector2.Distance(location.position, targetlocation.position);
			float x = (startDistance - distance) / startDistance;
			speed = curve.Evaluate(x) * referenceSpeed;
			location.transform.position = Vector3.MoveTowards(location.transform.position, targetlocation.transform.position, speed * Time.deltaTime);
			yield return null;
		}
	}
}
