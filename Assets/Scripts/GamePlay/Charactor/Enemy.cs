using UnityEngine;
using Zenject;

[RequireComponent(typeof(CharacterStat))]
public class Enemy : MonoBehaviour
{
    [Header("掉落的经验球 Prefab（在 Inspector 里赋值）")]
    [SerializeField] private GameObject expBallPrefab;

    private CharacterStat _characterStat;
    private SpeedMove   _speedMove;

    private Player        _player;
    private DiContainer   _container;
    
    ExpBall.Factory _expBallFactory;

    // 1) 注入场景中的 Player 实例和 Zenject 容器
    [Inject]
    public void Construct(Player player, DiContainer container, ExpBall.Factory expBallFactory)
    {
        _player    = player;
        _container = container;
        _expBallFactory = expBallFactory;
    }

    private void Awake()
    {
        _characterStat = GetComponent<CharacterStat>();

        // 如果你依旧想用 SpeedMove，可以像这样加组件并配置
        _speedMove = gameObject.AddComponent<SpeedMove>();
        _speedMove.speed = 1f;
    }

    private void Start()
    {
        _speedMove.Attach(gameObject, _player.gameObject);

        _characterStat.OnHealthEqualsZero += HandleDeath;
    }

    private void HandleDeath()
    {
        var exp = _expBallFactory.Create();
        exp.transform.position = transform.position;

        _characterStat.OnHealthEqualsZero -= HandleDeath;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 碰到注入进来的 Player 即可
        if (collision.transform == _player.transform)
        {
            var targetStat = collision.GetComponent<CharacterStat>();
            targetStat.TakeDamage(
                _characterStat.GetStat(Stat.Stats.Damage)
            );
        }
    }
}