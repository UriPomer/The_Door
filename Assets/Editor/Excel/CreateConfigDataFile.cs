using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CreateConfigDataFile : EditorWindow
{
    static string filePath = "/Scripts/Configs";
    private Object selectedObj;
	private bool canCreate = false;
	private bool canCreateObjects = false;

    [MenuItem("Tools/Excel/�����ļ�����")]
    public static void ShowWindow()
    {
        CreateConfigDataFile window = (CreateConfigDataFile)EditorWindow.GetWindow(typeof(CreateConfigDataFile));
        window.titleContent.text = "�����ļ�����";
    }

	private void OnGUI()
	{
		GUILayout.Label("ѡ��csv�ļ�");
		if (Selection.activeObject == null)
		{
			GUILayout.Label("û��ѡ���κ�csv");
			canCreate = false;
		}
		else
		{
			GUILayout.Label(Selection.activeObject.name);

			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			//����ļ���ʽ����csv
			if (path.Length < 4)
			{
				GUILayout.Label("ѡ����ļ�����csv");
				canCreate = false;
			}
			else if (path.ToLower().Substring(path.Length - 4, 4) != ".csv")
			{
				GUILayout.Label("ѡ����ļ�����csv");
				canCreate = false;
			}
			else
			{
				GUILayout.Label(filePath + "/" + Selection.activeObject.name + ".cs");
				selectedObj = Selection.activeObject;
				canCreate = true;
			}
		}
		if (GUILayout.Button("�������ͽű�"))
        {
			if(canCreate)
			{
				CreateConfigUtil.CreateConfigDataFile(selectedObj, filePath);
				canCreateObjects = true;
				AssetDatabase.Refresh();
				canCreateObjects = true;
			}
		}
		GUILayout.Label("�������������ͽű����ȴ��������¼��ز��ܽ��д˲���");
		if (GUILayout.Button("�������Ͷ���"))
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
