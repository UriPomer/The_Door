using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IMonsterSpawner
{
	Vector2[] CircleSpawn(int count);
	Vector2[] RandomSpawn(int count);
	Vector2[] GroupSpawn(int count, float groupRadius = 2f);
	void SpawnMonsters(IEnumerable<Vector2> positions, GameObject prefab);
}

public class MonsterSpawner : IMonsterSpawner, IInitializable, IDisposable
{
    readonly DiContainer       _container;
    readonly Player            _player;
    readonly MapConfig   _config;
    Transform                   _parent;

    [Inject]
    public MonsterSpawner(
        Player player,
        DiContainer container,
        MapConfig  config)
    {
        _player    = player;
        _container = container;
        _config = config;
    }

    // 创建一个空节点挂在场景里，作为所有怪物的父物体
    public void Initialize()
    {
        var go = new GameObject("Monsters");
        _parent = go.transform;
    }

    public void Dispose()
    {
        if (_parent != null)
            GameObject.Destroy(_parent.gameObject);
    }

    public Vector2[] CircleSpawn(int count)
    {
        var pts = new Vector2[count];
        var center = (Vector2)_player.transform.position;
        for (int i = 0; i < count; i++)
        {
            float angle = i * Mathf.PI * 2 / count;
            pts[i] = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _config.spawnRadius;
        }
        return pts;
    }

    public Vector2[] RandomSpawn(int count)
    {
        var pts = new Vector2[count];
        var center = (Vector2)_player.transform.position;
        for (int i = 0; i < count; i++)
        {
            float a = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
            pts[i] = center + new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * _config.spawnRadius;
        }
        return pts;
    }

    public Vector2[] GroupSpawn(int count, float groupRadius = 2f)
    {
        var pts = new Vector2[count];
        var center = (Vector2)_player.transform.position +
                     new Vector2(
                         Mathf.Cos(UnityEngine.Random.value * 2 * Mathf.PI),
                         Mathf.Sin(UnityEngine.Random.value * 2 * Mathf.PI)
                     ) * _config.spawnRadius;
        for (int i = 0; i < count; i++)
        {
            float a = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
            pts[i] = center + new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * groupRadius;
        }
        return pts;
    }

    public void SpawnMonsters(IEnumerable<Vector2> positions, GameObject prefab)
    {
        foreach (var pos in positions)
        {
            // 用 DiContainer.InstantiatePrefab ，可以给怪物上的 [Inject] 也注入
            _container.InstantiatePrefab(
                prefab, 
                new Vector3(pos.x, pos.y, 0), 
                Quaternion.identity, 
                _parent
            );
        }
    }
}
