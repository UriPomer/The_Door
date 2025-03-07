using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : UnitySingleton<UIMgr>
{
    public GameObject canvas;
    string uiPrefabPath = "GUI/UIPrefabs/";

    public override void Awake()
    {
		base.Awake();
		canvas = GameObject.Find("Canvas");
        if(canvas == null)
        {
            Debug.LogWarning("Canvas not found!");
        }
	}

    public UIController ShowUI(string name, Transform parent = null)
    {
        GameObject uiPrefab = Resources.Load<GameObject>(uiPrefabPath + name);
        GameObject uiView = Instantiate(uiPrefab);
        uiView.name = name;
        if(parent == null)
        {
			parent = canvas.transform;
		}
        uiView.transform.SetParent(parent, false);

        Type type = Type.GetType(name + "_Ctrl");
        UIController uiCtrl = (UIController)uiView.AddComponent(type);
        return uiCtrl;
    }

    public void RemoveUI(string name)
    {
        Transform uiView = canvas.transform.Find(name);
        if(uiView != null)
        {
			Destroy(uiView.gameObject);
		}
		else
        {
			Debug.LogWarning("UI not found: " + name);
		}
    }

    public void RemoveAllUI()
    {
		foreach(Transform child in canvas.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
