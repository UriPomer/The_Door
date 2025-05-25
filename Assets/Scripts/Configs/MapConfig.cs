using UnityEngine;

[CreateAssetMenu(menuName = "Configs/MapConfig", fileName = "MapConfig")]
public class MapConfig : ScriptableObject
{
    [Header("地图块")]
    public GameObject mapPrefab;

    [Header("生成距离阈值")]
    public float generationDistance = 10f;

    [Header("怪物生成")]
    public GameObject monsterPrefab;
    public AnimationCurve monsterSpawnCurve;
    public int MaxSpawnNum = 200;
    public int MinSpawnNum = 5;
    public float spawnInterval = 2f;
    
    [Header("怪物生成半径")]
    public float spawnRadius = 15f;

    [Header("地图块大小（可留空，让 MapManager 自动读取）")]
    public float mapBlockWidth  = 0f;
    public float mapBlockHeight = 0f;
}