using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UICreator : EditorWindow
{
	private static string filePath = "Assets/Scripts/GamePlay/Controller";

    [MenuItem("Tools/UI Creator")]
    public static void CreateUI()
    {
		UICreator win = EditorWindow.GetWindow<UICreator>("UI Creator");
        win.Show();
	}

	public void OnGUI()
	{
		GUILayout.Label("ѡ��һ��UI��ͼ");
		if(Selection.activeObject != null)
		{
			GUILayout.Label(Selection.activeObject.name);
			GUILayout.Label(filePath + "/" + Selection.activeObject.name + "_Ctrl.cs");
		}
		else
		{
			GUILayout.Label("û��ѡ���κ�UI��ͼ");
		}

		if(GUILayout.Button("����UI������"))
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
