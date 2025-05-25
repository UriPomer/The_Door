using System.Collections;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(PID))]
public class ExpBall : MonoBehaviour
{
    [Tooltip("等待多长时间后开始向玩家移动")]
    public float flyToPlayerTime = 1f;
    [Tooltip("距离玩家多近时才触发吸附")]
    public float range = 10f;

    private IMoveMethod _pidMove;
    private Transform   _playerTransform;
    private IXPService  _xpService;
    
    public class Factory : PlaceholderFactory<ExpBall> { }

    // 通过 Construct 注入 Player（从场景中绑定）和 XP 服务
    [Inject]
    public void Construct(Player player, IXPService xpService)
    {
        _playerTransform = player.transform;
        _xpService       = xpService;
    }

    void Awake()
    {
        // PID 脚本已经 RequireComponent，直接取
        _pidMove = GetComponent<PID>();
    }

    void Start()
    {
        StartCoroutine(FlyToPlayer());
    }

    private IEnumerator FlyToPlayer()
    {
        // 等待一段时间再开始检测距离
        yield return new WaitForSeconds(flyToPlayerTime);

        // 每隔 0.2 秒检查一次距离
        while (Vector2.Distance(_playerTransform.position, transform.position) >= range)
        {
            yield return new WaitForSeconds(0.2f);
        }

        // 超过范围后，开始用 PID 吸向玩家
        _pidMove.Attach(gameObject, _playerTransform.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 仅在碰到玩家时才生效
        if (collision.transform == _playerTransform)
        {
            Destroy(gameObject);
            // 用注入的服务增加经验
            _xpService.Experience += 10;
        }
    }
}