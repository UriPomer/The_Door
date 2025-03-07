using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CircleTargetSpawner : A_TargetSpawner
{
	[Header("CircleTargetSpawner")]
	public Transform centerPoint; // Բ��λ��
	public float radius = 5f; // �뾶
	public float rotationSpeed = 5f; // ��ת�ٶȣ���/�룩

	protected override void SpawnTargets(int num)
	{
		if(centerPoint == null)
		{
			centerPoint = GameObject.FindGameObjectWithTag("Player").transform;
		}
		float angleStep = 360f / numberOfTargets;

		for (int i = 0; i < numberOfTargets; i++)
		{
			float angle = i * angleStep;
			Vector3 spawnPosition = centerPoint.position + Quaternion.Euler(0f, 0f, angle) * (Vector3.right * radius);

			GameObject spawnedObject = Instantiate(emptyTargetPrefab);
			spawnedObject.transform.position = spawnPosition;
			spawnedObject.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
			targets.Add(spawnedObject);
		}
	}

	private void FixedUpdate()
	{
		// Χ��Բ����ת�������ɵĶ���
		foreach (GameObject obj in targets)
		{
			obj.transform.RotateAround(centerPoint.position, Vector3.forward, rotationSpeed * Time.deltaTime);
		}
	}
}
