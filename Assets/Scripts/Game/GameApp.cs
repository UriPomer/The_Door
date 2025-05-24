using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

public class GameApp : UnitySingleton<GameApp>
{
    public void InitGame()
    {
        this.EnterMainScene();
    }

    public void EnterMainScene()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player.GetComponent<Player>() == null)
        {
            player.AddComponent<Player>().Init();
        }
        else
        {
			player.GetComponent<Player>().Init();
		}
        
        CinemachineVirtualCamera vcam = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
        vcam.Follow = player.transform;
        vcam.LookAt = player.transform;


        //end

        //UIMgr.Instance.ShowUI("UIExp");
        //UIMgr.Instance.ShowUI("UIHome");
        //end
    }
}
