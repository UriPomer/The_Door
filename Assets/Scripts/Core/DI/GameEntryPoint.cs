using Zenject;
using Unity.Cinemachine;
using UnityEngine;

public class GameEntryPoint : IInitializable
{
    readonly IGameStateService _gameState;
    readonly DiContainer        _container;
    readonly CinemachineCamera _vcam;

    [Inject]
    public GameEntryPoint(
        IGameStateService gameState,
        DiContainer container,
        CinemachineCamera vcam)
    {
        _gameState  = gameState;
        _container  = container;
        _vcam       = vcam;
    }

    public void Initialize()
    {
        var playerGO = GameObject.FindGameObjectWithTag("Player");

        _container.InjectGameObject(playerGO);

        // 绑定摄像机
        _vcam.Follow = playerGO.transform;
        _vcam.LookAt = playerGO.transform;

        // 触发游戏开始
        _gameState.SetState(GameState.Start);
    }
}