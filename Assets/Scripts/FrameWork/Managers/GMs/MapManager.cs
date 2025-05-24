using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapMgr : UnitySingleton<MapMgr>
{
	public GameObject mapPrefab;
	private Transform player;
	[SerializeField]private float generationDistance = 10f;

	private float mapBlockWidth = 10f;
	private float mapBlockHeight = 10f;

	private Dictionary<Vector2, MapBlock> m_mapBlocks = new Dictionary<Vector2, MapBlock>();
	private GameObject mapCurrentStay;
	private Vector2 indexCurrentStay;

	[Header("Spawn")]
	public GameObject monsterPrefab;
	public AnimationCurve monsterSpawnCurve;
	[SerializeField]private int MaxSpawnNum = 200;


	[Header("Game")]
	private float gameTime = 0f;

	public override void Awake()
	{
		if (!this.gameObject.GetComponent<MonsterSpawner>())
			this.gameObject.AddComponent<MonsterSpawner>();
	}

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		mapBlockHeight = mapPrefab.GetComponent<BoxCollider2D>().size.y;
		mapBlockWidth = mapPrefab.GetComponent<BoxCollider2D>().size.x;
		GenerateInitialMap(Vector2.zero);
		SetCurrentStay(Vector2.zero);
		StartCoroutine(Spawn());
	}

	private const int MinMonsterNum = 5;
	private IEnumerator Spawn()
	{
		while (true)
		{
			var spawnPercentage = gameTime / (10);
			var spawnNum = Mathf.Max((float)monsterSpawnCurve.Evaluate(spawnPercentage) * MaxSpawnNum, MinMonsterNum);
			Debug.Log(spawnNum);
			SimpleSpawn((int)spawnNum, monsterPrefab);
			yield return new WaitForSeconds(2f);
		}
	}

	private void Update()
	{
		if (Vector2.Distance(player.position, mapCurrentStay.transform.position) > generationDistance)
		{
			GenerateNewMapBlock();
		}
	}

	private void FixedUpdate()
	{
		gameTime += Time.deltaTime;
	}

	private void GenerateInitialMap(Vector2 spawnPosition)
	{
		GameObject initialMap = Instantiate(mapPrefab, spawnPosition, Quaternion.identity);
		m_mapBlocks.Add(new Vector2(0,0),initialMap.GetComponent<MapBlock>());
	}

	private void GenerateNewMapBlock()
	{
		if(player.position.x > mapCurrentStay.transform.position.x)
		{
			GenerateSingleMapBlock(new Vector2(indexCurrentStay.x + 1, indexCurrentStay.y));
			if(player.position.y > mapCurrentStay.transform.position.y)
			{
				GenerateSingleMapBlock(new Vector2(indexCurrentStay.x,indexCurrentStay.y +1));
				GenerateSingleMapBlock(new Vector2(indexCurrentStay.x + 1, indexCurrentStay.y + 1));
			}
			else
			{
				GenerateSingleMapBlock(new Vector2(indexCurrentStay.x, indexCurrentStay.y - 1));
				GenerateSingleMapBlock(new Vector2(indexCurrentStay.x + 1, indexCurrentStay.y - 1));
			}
		}
		else
		{
			GenerateSingleMapBlock(new Vector2(indexCurrentStay.x - 1, indexCurrentStay.y));
			if (player.position.y > mapCurrentStay.transform.position.y)
			{
				GenerateSingleMapBlock(new Vector2(indexCurrentStay.x, indexCurrentStay.y + 1));
				GenerateSingleMapBlock(new Vector2(indexCurrentStay.x - 1, indexCurrentStay.y + 1));
			}
			else
			{
				GenerateSingleMapBlock(new Vector2(indexCurrentStay.x, indexCurrentStay.y - 1));
				GenerateSingleMapBlock(new Vector2(indexCurrentStay.x - 1, indexCurrentStay.y - 1));
			}
		}
	}

	private void GenerateSingleMapBlock(Vector2 index)
	{
		if(m_mapBlocks.ContainsKey(index))
		{
			return;
		}
		GameObject newMap = Instantiate(mapPrefab, new Vector2(index.x * mapBlockWidth, index.y * mapBlockHeight), Quaternion.identity);
		m_mapBlocks.Add(index, newMap.GetComponent<MapBlock>());
	}

	public void SetCurrentStay(Vector2 index)
	{
		mapCurrentStay = m_mapBlocks[index].gameObject;
		indexCurrentStay = index;
	}

	public void SetCurrentStay(MapBlock mapBlock)
	{
		mapCurrentStay = mapBlock.gameObject;
		foreach(var item in m_mapBlocks)
		{
			if(item.Value == mapBlock)
			{
				indexCurrentStay = item.Key;
				break;
			}
		}
	}

	public void SimpleSpawn(int num, GameObject MonsterPrefab)
	{
		Vector2[] positions = MonsterSpawner.Instance.SimpleRandomSpawner(num);
		MonsterSpawner.Instance.SpawnMonsters(positions, MonsterPrefab);
	}

	public void CircleSpawn(int num, GameObject MonsterPrefab)
	{
		Vector2[] positions = MonsterSpawner.Instance.CircleSpawner(num);
		MonsterSpawner.Instance.SpawnMonsters(positions, MonsterPrefab);
	}
}
