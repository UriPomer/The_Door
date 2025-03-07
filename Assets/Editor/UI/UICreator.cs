using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UICreator : EditorWindow
{
	private static string filePath = "Assets/Scripts/Game/UIController";

    [MenuItem("Tools/UI Creator")]
    public static void CreateUI()
    {
		UICreator win = EditorWindow.GetWindow<UICreator>("UI Creator");
        win.Show();
	}

	public void OnGUI()
	{
		GUILayout.Label("选择一个UI视图");
		if(Selection.activeObject != null)
		{
			GUILayout.Label(Selection.activeObject.name);
			GUILayout.Label(filePath + "/" + Selection.activeObject.name + "_Ctrl.cs");
		}
		else
		{
			GUILayout.Label("没有选择任何UI视图");
		}

		if(GUILayout.Button("创建UI控制器"))
		{
			if(Selection.activeObject != null)
			{
				UICreatorUtil.GenerateUIController(filePath, Selection.activeObject.name + "_Ctrl");
				AssetDatabase.Refresh();
			}
		}
	}

	public void OnSelectionChange()
	{
		this.Repaint();
	}
}
