using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapMgr : UnitySingleton<MapMgr>
{
	public GameObject mapPrefab; // 地图块的Prefab
	private Transform player; // 摄像头或玩家位置
	[SerializeField]private float generationDistance = 10f; // 触发生成新地图块的距离阈值

	private float mapBlockWidth = 10f; // 地图块的宽度
	private float mapBlockHeight = 10f; // 地图块的高度

	private Dictionary<Vector2, MapBlock> m_mapBlocks = new Dictionary<Vector2, MapBlock>(); // 地图块的字典
	private GameObject mapCurrentStay;
	private Vector2 indexCurrentStay;

	[Header("怪物生成")]
	public GameObject monsterPrefab;
	public AnimationCurve monsterSpawnCurve;
	[SerializeField]private int MaxSpawnNum = 200;


	[Header("游戏计时")]
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

	private IEnumerator Spawn()
	{
		int spawnNum = 0;
		float spawnPercentage = 0;
		while (true)
		{
			spawnPercentage = gameTime / (20 * 60);
			spawnNum = (int)monsterSpawnCurve.Evaluate(spawnPercentage) * MaxSpawnNum;
			SimpleSpawn(5, monsterPrefab);
			yield return new WaitForSeconds(2f);
		}
	}

	private void Update()
	{
		// 检测是否需要生成新地图块
		if (Vector2.Distance(player.position, mapCurrentStay.transform.position) > generationDistance)
		{
			GenerateNewMapBlock();
		}
	}

	private void FixedUpdate()
	{
		gameTime += Time.deltaTime;
	}

	// 生成初始地图块
	private void GenerateInitialMap(Vector2 spawnPosition)
	{
		GameObject initialMap = Instantiate(mapPrefab, spawnPosition, Quaternion.identity);
		m_mapBlocks.Add(new Vector2(0,0),initialMap.GetComponent<MapBlock>());
	}

	// 生成新地图块
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
		Debug.Log(indexCurrentStay);
	}

	public void SimpleSpawn(int num, GameObject monsterPrefab)
	{
		Vector2[] positions = MonsterSpawner.Instance.SimpleRandomSpawner(num);
		foreach(var position in positions)
		{
			Instantiate(monsterPrefab, position, Quaternion.identity);
		}
	}

	public void CircleSpawn(int num, GameObject monsterPrefab)
	{
		Vector2[] positions = MonsterSpawner.Instance.CircleSpawner(num);
		foreach (var position in positions)
		{
			Instantiate(monsterPrefab, position, Quaternion.identity);
		}
	}
}
