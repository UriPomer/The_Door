using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PID : MonoBehaviour, IMoveMethod
{
	private Transform targetLocation;
	private Transform thisLocation;

	[Header("PIDMove")]
	[SerializeField]
	private float Ti = 40f;
	[SerializeField]
	private float T = 20f;
	[SerializeField]
	private float Td = 0.7f;
	[SerializeField]
	public float Kp = 0.03f;
	float Etx = 0f, Etx1 = 0f, Etx2 = 0f;
	float Ety = 0f, Ety1 = 0f, Ety2 = 0f;

	[Header("PIDRotate")]
	[SerializeField]
	private float TiR = 400f;
	[SerializeField]
	private float TR = 20f;
	[SerializeField]
	private float TdR = 0.7f;
	[SerializeField]
	public float KpR = 0.5f;
	float EtAngle = 0f, EtAngle1 = 0f, EtAngle2 = 0f;

	float q0,q1,q2;
	float q0R, q1R, q2R;
	float u0,u1;
	// Start is called before the first frame update
	void Awake()
	{
		q0 = Kp * (1 + T / Ti + Td / T);
		q1 = -Kp * (1 + 2 * Td / T);
		q2 = Kp * Td / T;

		q0R = KpR * (1 + TR / TiR + TdR / T);
		q1R = -KpR * (1 + 2 * TdR / T);
		q2R = KpR * TdR / T;
	}

	public IEnumerator Move(Transform targetLocation, Transform thisLocation)
	{
		if(targetLocation == null)
		{
			yield break;
		}
		while (true)
		{
			if (targetLocation == null)
			{
				yield break;
			}
			Etx = targetLocation.position.x - thisLocation.position.x;
			Ety = targetLocation.position.y - thisLocation.position.y;

			u0 = q0 * Etx + q1 * Etx1 + q2 * Etx2;
			u1 = q0 * Ety + q1 * Ety1 + q2 * Ety2;

			Etx2 = Etx1;
			Etx1 = Etx;
			Ety2 = Ety1;
			Ety1 = Ety;

			thisLocation.position = new Vector3(thisLocation.position.x + u0, thisLocation.position.y + u1, thisLocation.position.z);
			yield return null;
		}
	}

	public IEnumerator PIDRotate(Transform targetAngle, Transform thisAngle)
	{
		if(targetAngle == null)
		{
			yield break;
		}
		while (true)
		{
			if (targetLocation == null)
			{
				yield break;
			}
			Vector2 direction = (targetAngle.transform.position - thisAngle.transform.position).normalized;
			EtAngle = Vector2.Angle(Vector2.up, direction) - thisAngle.eulerAngles.z;

			
			if (!targetAngle.gameObject.CompareTag("Enemy"))
			{
				EtAngle = 0 - thisAngle.transform.rotation.eulerAngles.z;
			}
			else if(direction.x > 0f)
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

	public void Attach(GameObject location, GameObject targetlocation)
	{
		thisLocation = location.transform;
		targetLocation = targetlocation.transform;
		StartCoroutine(Move(targetLocation, thisLocation));
		StartCoroutine(PIDRotate(targetLocation, thisLocation));
	}

	public void Stop()
	{
		StopAllCoroutines();
	}
}
