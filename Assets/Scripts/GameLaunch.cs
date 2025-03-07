using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLaunch : MonoBehaviour
{
	private void Awake()
	{
		//初始化游戏框架
		this.InitFramework();
		//end

		//检查热更新
		this.CheckHotUpdate();
		//end

		//初始化游戏逻辑
		this.InitGameLogic();
		//end
	}

	private void InitFramework()
	{
		if (!this.gameObject.GetComponent<ResMgr>())
			this.gameObject.AddComponent<ResMgr>();
		if (!this.gameObject.GetComponent<UIMgr>())
			this.gameObject.AddComponent<UIMgr>();
		if (!this.gameObject.GetComponent<GameMgr>())
			this.gameObject.AddComponent<GameMgr>();
	}

	private void CheckHotUpdate()
	{
		//获取服务器资源 + 脚本代码的版本
		//end
		//拉取下载列表
		//end
		// 下载更新资源到本地
		//end
	}

	private void InitGameLogic()
	{
		this.gameObject.AddComponent<GameApp>();
		GameApp.Instance.InitGame();
	}
}
