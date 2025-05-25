using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
	[Header("UI")]
	public UIConfig uiConfig;                    // 你在 Project 里创建好的 UIConfig.asset
	[SerializeField] Canvas canvas;             // 场景里的 Canvas
	
	[Header("Configs")]
	public MapConfig mapConfig;
	
	[Header("Gameplay Prefabs")]
	[SerializeField] private GameObject expBallPrefab;
	
	public override void InstallBindings()
	{
		// 安装 SignalBus
		SignalBusInstaller.Install(Container);
		Container.DeclareSignal<GameStateChangedSignal>();
		Container.DeclareSignal<PlayerStateChangedSignal>();

		Container.Bind<CinemachineCamera>()
			.FromComponentInHierarchy()
			.AsSingle()
			.NonLazy();
		
		Container.BindInstance(uiConfig);

		Container.BindInstance(canvas.transform)
			.WithId("CanvasRoot");
		
		Container.Bind<Player>()
			.FromComponentInHierarchy()
			.AsSingle()
			.NonLazy();

		Container.Bind<AnimationControl>()
			.FromComponentInHierarchy()
			.AsSingle()
			.NonLazy();

		Container.BindFactory<ExpBall, ExpBall.Factory>()
			.FromComponentInNewPrefab(expBallPrefab)
			.AsTransient()
			.NonLazy();
		
		Container.Bind<MapManager>()        // 绑定你的 MapManager 脚本
			.FromComponentInHierarchy() // 场景里挂一个带 MapManager 的 GameObject
			.AsSingle()
			.NonLazy();
		
		Container.BindInstance(mapConfig)
			.AsSingle();
		
		Container.BindInterfacesAndSelfTo<MonsterSpawner>()
			.AsSingle()
			.NonLazy();
		
		// 注册并马上运行 EntryPoint
		Container.BindInterfacesAndSelfTo<GameEntryPoint>()
			.AsSingle()
			.NonLazy();
	}
}

