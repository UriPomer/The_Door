using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterStat), typeof(AnimationControl))]
public class Player : MonoBehaviour
{
    // --- 原来 Init 里这些字段 ---
    private PlayerController _moveController;
    private CharacterStat    _stat;

    // --- 注入进来的服务 ---
    [Inject] private IGameStateService _gameStateService;
    [Inject] private SignalBus         _signalBus;

    // Awake 里做 “构造” + 组件添加 + 初始化
    void Awake()
    {
        // 拿到角色属性
        _stat = GetComponent<CharacterStat>();

        // 给自己加上移动控制器
        _moveController = gameObject.AddComponent<PlayerController>();
    }

    // OnEnable/OnDisable 里做事件订阅和解绑
    void OnEnable()
    {
        _stat.OnHealthEqualsZero += OnHealthEqualsZero;
        _signalBus.Subscribe<GameStateChangedSignal>(OnGameStateChanged);
    }

    void OnDisable()
    {
        _stat.OnHealthEqualsZero -= OnHealthEqualsZero;
        _signalBus.Unsubscribe<GameStateChangedSignal>(OnGameStateChanged);
    }

    // 原来 Init 里那段：血量归零时的逻辑
    private void OnHealthEqualsZero()
    {
        _gameStateService.SetState(GameState.End);
        Destroy(gameObject);
    }

    private void OnGameStateChanged(GameStateChangedSignal signal)
    {
        Debug.Log($"[Player] GameState changed to: {signal.State}");
    }
}