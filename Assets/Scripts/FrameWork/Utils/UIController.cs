using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Dictionary<string, GameObject> view = new Dictionary<string, GameObject>();

	private void LoadAllObjectsToView(GameObject root, string path)
	{
		foreach(Transform child in root.transform)
		{
			if(this.view.ContainsKey(path + child.gameObject.name))
			{
				continue;
			}
			this.view.Add(path + child.gameObject.name, child.gameObject);
			LoadAllObjectsToView(child.gameObject, path + child.gameObject.name + "/");
		}
	}

	public virtual void Awake()
	{
		this.LoadAllObjectsToView(this.gameObject, "");
	}

	//public void AddButtonListener(string viewName, UnityEngine.Events.UnityAction onClick)
	//{
	//	Button btn = this.view[viewName].GetComponent<Button>();
	//	if(btn == null)
	//	{
	//		Debug.LogWarning("AddButtonListener: " + viewName + " is not a button");
	//		return;
	//	}
	//	btn.onClick.AddListener(onClick);
	//}
}
