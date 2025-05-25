using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PIDRotate
{
	[Header("PIDRotate")]
	[SerializeField]
	private float T = 20f;
	[SerializeField]
	private float TiR = 400f;
	[SerializeField]
	private float TR = 20f;
	[SerializeField]
	private float TdR = 0.7f;
	[SerializeField]
	public float KpR = 0.5f;
	float EtAngle = 0f, EtAngle1 = 0f, EtAngle2 = 0f;
	float q0R, q1R, q2R;

	public IEnumerator Rotate(Transform targetAngle, Transform thisAngle)
	{
		if (targetAngle == null)
		{
			yield break;
		}
		q0R = KpR * (1 + TR / TiR + TdR / T);
		q1R = -KpR * (1 + 2 * TdR / T);
		q2R = KpR * TdR / T;
		while (true)
		{
			if (targetAngle == null)
			{
				yield break;
			}
			Vector2 direction = (targetAngle.transform.position - thisAngle.transform.position).normalized;
			EtAngle = Vector2.Angle(Vector2.up, direction) - thisAngle.eulerAngles.z;


			if (!targetAngle.gameObject.CompareTag("Enemy"))
			{
				EtAngle = 0 - thisAngle.transform.rotation.eulerAngles.z;
			}
			else if (direction.x > 0f)
			{
				EtAngle = -Vector2.Angle(Vector2.up, direction) - thisAngle.eulerAngles.z;
				if (EtAngle < -180f)
				{
					EtAngle += 360f;
				}
				else if (EtAngle > 180f)
				{
					EtAngle -= 360f;
				}
			}
			float u = q0R * EtAngle + q1R * EtAngle1 + q2R * EtAngle2;
			EtAngle2 = EtAngle1;
			EtAngle1 = EtAngle;
			thisAngle.Rotate(new Vector3(0, 0, u));
			yield return null;
		}
	}
}
