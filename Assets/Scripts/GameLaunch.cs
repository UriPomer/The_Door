using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameLaunch : MonoBehaviour
{
	private void Awake()
	{
		//��ʼ����Ϸ���
		this.InitFramework();
		//end

		//����ȸ���
		this.CheckHotUpdate();
		//end

		//��ʼ����Ϸ�߼�
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
		//��ȡ��������Դ + �ű�����İ汾
		//end
		//��ȡ�����б�
		//end
		// ���ظ�����Դ������
		//end
	}

	private void InitGameLogic()
	{
		this.gameObject.AddComponent<GameApp>();
		GameApp.Instance.InitGame();
	}
}
