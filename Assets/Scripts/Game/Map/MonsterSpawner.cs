using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : UnitySingleton<MonsterSpawner>
{
    [SerializeField] private const float radius = 15f;
    Transform player;

	public override void Awake()
	{
		base.Awake();
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	public void Start()
	{
		if(player == null)
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	public Vector2[] CircleSpawner(int num)
    {
		//���λ��radius��Բ��ƽ��������num����
		Vector2[] points = new Vector2[num];
		for (int i = 0; i < num; i++)
		{
			float angle = i * Mathf.PI * 2 / num;
			float x = Mathf.Cos(angle) * radius;
			float y = Mathf.Sin(angle) * radius;
			points[i] = new Vector2(player.position.x + x, player.position.y + y);
		}
		return points;
    }

	public Vector2[] SimpleRandomSpawner(int num)
	{
		//���λ��radius��Բ���������num����
		Vector2[] points = new Vector2[num];
		for (int i = 0; i < num; i++)
		{
			float angle = Random.Range(0, 2 * Mathf.PI);
			float x = Mathf.Cos(angle) * radius;
			float y = Mathf.Sin(angle) * radius;
			points[i] = new Vector2(player.position.x + x, player.position.y + y);
		}
		return points;
	}

	public Vector2[] GroupSpawner(int num, float _radius = 2f)
	{
		//���λ��radius��Բ�����Ѱ��һ���㣬Ȼ���������_radius��������num����
		Vector2[] points = new Vector2[num];
		float angle = Random.Range(0, 2 * Mathf.PI);
		float x = Mathf.Cos(angle) * radius;
		float y = Mathf.Sin(angle) * radius;
		Vector2 center = new Vector2(player.position.x + x, player.position.y + y);
		for (int i = 0; i < num; i++)
		{
			float angle2 = Random.Range(0, 2 * Mathf.PI);
			float x2 = Mathf.Cos(angle2) * _radius;
			float y2 = Mathf.Sin(angle2) * _radius;
			points[i] = center + new Vector2(x2, y2);
		}
		return points;
	}
}
