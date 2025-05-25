using Zenject;
using UnityEngine;

public class GameServicesInstaller : MonoInstaller
{
    [SerializeField] private AnimationCurve xpCurve;
    [SerializeField] private int baseXP = 100;
    [SerializeField] private int xpOffset = 10;

    public override void InstallBindings()
    {
        // 绑定 AnimationCurve 和 基础经验值
        Container.BindInstance(xpCurve).WithId("XP_CURVE");
        Container.BindInstance(baseXP).WithId("BASE_XP");
        Container.BindInstance(xpOffset).WithId("XP_OFFSET");

        // 绑定服务
        Container.Bind<IGameStateService>().To<GameStateService>().AsSingle();
        Container.Bind<IXPService>().To<XPService>().AsSingle();
        Container.Bind<IScoreService>().To<ScoreService>().AsSingle();
        
        Container.Bind<IUIService>().To<UIService>()
            .AsSingle()
            .NonLazy();
    }
}