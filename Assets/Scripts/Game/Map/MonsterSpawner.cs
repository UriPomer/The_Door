using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : UnitySingleton<MonsterSpawner>
{
    [SerializeField] private const float radius = 15f;
    Transform player;
    
    private Transform monstersParent;

    public override void Awake()
    {
	    base.Awake();
	    player = GameObject.FindGameObjectWithTag("Player").transform;

	    GameObject monstersGO = GameObject.Find("Monsters");
	    if (!monstersGO)
	    {
		    monstersGO = new GameObject("Monsters");
	    }
	    monstersParent = monstersGO.transform;
    }
    
	public void Start()
	{
		if(player == null)
			player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	public Vector2[] CircleSpawner(int num)
    {
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
	
	public void SpawnMonsters(Vector2[] positions, GameObject MonsterPrefab)
	{
		foreach (var pos in positions)
		{
			GameObject monster = Instantiate(MonsterPrefab, pos, Quaternion.identity);
			monster.transform.SetParent(monstersParent);
		}
	}
}
