// Assets/Scripts/Gameplay/Map/MapManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(MonsterSpawner))]
public class MapManager : MonoBehaviour
{
    // 注入玩家、生成器、配置和容器
    Transform       _player;
    MonsterSpawner  _spawner;
    MapConfig       _config;
    DiContainer     _container;

    float           _gameTime = 0f;
    Dictionary<Vector2, MapBlock> _mapBlocks = new();

    MapBlock        _currentBlock;
    Vector2         _currentIndex;

    [Inject]
    public void Construct(
        Player player,
        MonsterSpawner spawner,
        MapConfig config,
        DiContainer container)
    {
        _player     = player.transform;
        _spawner    = spawner;
        _config     = config;
        _container  = container;
    }

    void Awake()
    {
        // 自动读取地图块大小（如果配置里没填）
        if (_config.mapBlockWidth  <= 0f ||
            _config.mapBlockHeight <= 0f)
        {
            var box = _config.mapPrefab.GetComponent<BoxCollider2D>().size;
            _config.mapBlockWidth  = box.x;
            _config.mapBlockHeight = box.y;
        }
    }

    void Start()
    {
        // 生成初始块（索引 0,0）
        var block = TryCreateBlock(Vector2.zero);
        SetCurrentBlock(block);

        // 开始怪物生成协程
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        // 检查玩家距离中心块，必要时生成新块
        if (Vector2.Distance(_player.position, _currentBlock.transform.position)
            > _config.generationDistance)
        {
            GenerateNeighbors();
        }
    }

    void FixedUpdate()
    {
        _gameTime += Time.deltaTime;
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float pct = _gameTime / 10f;
            int num = Mathf.Max(
                Mathf.RoundToInt(_config.monsterSpawnCurve.Evaluate(pct)
                                 * _config.MaxSpawnNum),
                _config.MinSpawnNum
            );
            _spawner.SpawnMonsters(_spawner.RandomSpawn(num), _config.monsterPrefab);
            yield return new WaitForSeconds(_config.spawnInterval);
        }
    }

    void GenerateNeighbors()
    {
        // 类似原逻辑，按 _currentIndex 和玩家位置生成 3 个新块
        Vector2 p = _player.position;
        Vector2 c = (Vector2)_currentBlock.transform.position;
        int dx = p.x > c.x ? 1 : -1;
        int dy = p.y > c.y ? 1 : -1;

        TryCreateBlock(_currentIndex + new Vector2(dx, 0));
        TryCreateBlock(_currentIndex + new Vector2(0, dy));
        TryCreateBlock(_currentIndex + new Vector2(dx, dy));
    }

    MapBlock TryCreateBlock(Vector2 idx)
    {
        if (_mapBlocks.TryGetValue(idx, out var createBlock)) return createBlock;

        Vector2 pos = new Vector2(
            idx.x * _config.mapBlockWidth,
            idx.y * _config.mapBlockHeight
        );
        GameObject go = _container.InstantiatePrefab(
            _config.mapPrefab,
            pos,
            Quaternion.identity,
            this.transform
        );

        var block = go.GetComponent<MapBlock>();
        _mapBlocks[idx] = block;
        return block;
    }

    public void SetCurrentBlock(MapBlock block)
    {
        _currentBlock = block;
        // 反推索引
        foreach (var kv in _mapBlocks)
            if (kv.Value == block)
            {
                _currentIndex = kv.Key;
                break;
            }
    }
}
