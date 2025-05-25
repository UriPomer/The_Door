using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Animation))]
public class AnimationControl : MonoBehaviour
{
    // 在 Inspector 里配每个状态对应的动画名字
    [System.Serializable]
    public struct StateClip
    {
        public PlayerState State;
        public string      ClipName;
    }

    [SerializeField]
    List<StateClip> _stateClips = new List<StateClip>();

    Animation                         _anim;
    PlayerState                       _currentState = PlayerState.Invalid;
    Dictionary<PlayerState, string>   _clipMap;

    SignalBus                         _signals;

    [Inject]
    void Construct(SignalBus signals)
    {
        _signals = signals;
    }

    void Awake()
    {
        _anim = GetComponent<Animation>();

        // 把列表转成 Dictionary 方便查找
        _clipMap = _stateClips
            .GroupBy(sc => sc.State)
            .ToDictionary(g => g.Key, g => g.First().ClipName);

        // 默认设为 Idle
        SetState(PlayerState.Idle);
    }

    void OnEnable()
    {
        // 订阅玩家状态变化
        _signals.Subscribe<PlayerStateChangedSignal>(OnPlayerStateChanged);
    }

    void OnDisable()
    {
        _signals.Unsubscribe<PlayerStateChangedSignal>(OnPlayerStateChanged);
    }

    // 收到信号就切状态
    void OnPlayerStateChanged(PlayerStateChangedSignal signal)
    {
        SetState(signal.NewState);
    }

    private void SetState(PlayerState newState)
    {
        if (_currentState == newState) return;
        _currentState = newState;

        if (_clipMap.TryGetValue(newState, out var clip))
        {
            _anim.CrossFade(clip);
        }
        else
        {
            Debug.LogWarning($"[AnimationControl] 没有找到状态 {newState} 对应的动画剪辑");
        }
    }
}
