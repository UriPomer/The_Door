using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateConfigDataFile : EditorWindow
{
    static string filePath = "/Scripts/ConfigDatas";
    private Object selectedObj;
	private bool canCreate = false;
	private bool canCreateObjects = false;

    [MenuItem("Tools/Excel/配置文件生成")]
    public static void ShowWindow()
    {
        CreateConfigDataFile window = (CreateConfigDataFile)EditorWindow.GetWindow(typeof(CreateConfigDataFile));
        window.titleContent.text = "配置文件生成";
    }

	private void OnGUI()
	{
		GUILayout.Label("选择csv文件");
		if (Selection.activeObject == null)
		{
			GUILayout.Label("没有选择任何csv");
			canCreate = false;
		}
		else
		{
			GUILayout.Label(Selection.activeObject.name);

			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			//如果文件格式不是csv
			if (path.Length < 4)
			{
				GUILayout.Label("选择的文件不是csv");
				canCreate = false;
			}
			else if (path.ToLower().Substring(path.Length - 4, 4) != ".csv")
			{
				GUILayout.Label("选择的文件不是csv");
				canCreate = false;
			}
			else
			{
				GUILayout.Label(filePath + "/" + Selection.activeObject.name + ".cs");
				selectedObj = Selection.activeObject;
				canCreate = true;
			}
		}
		if (GUILayout.Button("生成类型脚本"))
        {
			if(canCreate)
			{
				CreateConfigUtil.CreateConfigDataFile(selectedObj, filePath);
				canCreateObjects = true;
				AssetDatabase.Refresh();
				canCreateObjects = true;
			}
		}
		GUILayout.Label("必须先生成类型脚本并等待引擎重新加载才能进行此操作");
		if (GUILayout.Button("生成类型对象"))
		{
			if(canCreateObjects)
			{
				CreateConfigUtil.CreateConfigDataObjects(selectedObj, filePath);
				AssetDatabase.Refresh();
			}
		}
		
	}

	public void OnSelectionChange()
	{
		this.Repaint();
	}
}
