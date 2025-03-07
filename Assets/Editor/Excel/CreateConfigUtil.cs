using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class CreateConfigUtil : MonoBehaviour
{
    public static void CreateConfigDataFile(UnityEngine.Object selectObj, string writePath)
    {
        string fileName = selectObj.name;
        string className = fileName;
        StreamWriter sw = new StreamWriter(Application.dataPath + writePath + "/" + className + ".cs");

        sw.WriteLine("using System.Collections;");
        sw.WriteLine("using System.Collections.Generic;");
        sw.WriteLine("using UnityEngine;");
        sw.WriteLine("");
		sw.WriteLine("[CreateAssetMenu(fileName = \"" + className + "\", menuName = \"Config/" + className + "\")]");
        sw.WriteLine("public class " + className + " : ScriptableObject");
        sw.WriteLine("{");
        
        string filePath = AssetDatabase.GetAssetPath(selectObj);
		string[] fileData = File.ReadAllLines(filePath);
		/* CSV文件的第一行为Key字段，先读取key字段 */
		string[] keys = fileData[0].Split(',');
		string[] types = fileData[1].Split(',');
		for(int i = 0; i < keys.Length; i++)
		{
			sw.WriteLine("    public " + types[i] + " " + keys[i] + ";");
		}
		sw.WriteLine("}");

		sw.Flush();
		sw.Close();
	}

	public static void CreateConfigDataObjects(UnityEngine.Object selectObj, string writePath)
	{
		string fileName = selectObj.name;
		string className = fileName;

		string filePath = AssetDatabase.GetAssetPath(selectObj);
		string[] fileData = File.ReadAllLines(filePath);
		/* CSV文件的第一行为Key字段，先读取key字段 */
		string[] variableNames = fileData[0].Split(',');
		string[] types = fileData[1].Split(',');

		/* 第三行开始是数据,根据这些数据创建对应的可编程对象，且都放置在名为变量fileName的文件夹下 */
		string folderPath = Application.dataPath + writePath + "/" + fileName;
		if(!Directory.Exists(folderPath))
		{
			Directory.CreateDirectory(folderPath);
		}
		Assembly assembly = Assembly.Load("Assembly-CSharp"); // 替换为你的程序集名称，通常是Assembly-CSharp
		Type type = assembly.GetType(className);
		Debug.Log(className);
		Debug.Log(type);
		if (type != null)
		{
			FieldInfo[] fields = type.GetFields();
			for (int i = 2; i < fileData.Length; i++)
			{
				string[] datas = fileData[i].Split(',');
				string dataName = datas[1];
				string dataPath = "Assets/" + writePath + "/" + fileName + "/" + dataName + ".asset";
				ScriptableObject data = ScriptableObject.CreateInstance(type);
				for (int j = 0; j < variableNames.Length; j++)
				{
					string variableName = variableNames[j];
					string thistype = types[j];
					string value = datas[j];

					//根据变量名进行赋值value
					FieldInfo field = type.GetField(variableName);
					if (field != null)
					{
						if (thistype == "int")
						{
							field.SetValue(data, int.Parse(value));
						}
						else if (thistype == "float")
						{
							field.SetValue(data, float.Parse(value));
						}
						else if (thistype == "string")
						{
							field.SetValue(data, value);
						}
						else if (thistype == "bool")
						{
							field.SetValue(data, bool.Parse(value));
						}
					}
				}
				Debug.Log(dataPath);
				AssetDatabase.CreateAsset(data, dataPath);
			}
		}
	}

	public static string GetValueFromCSV(string path,int row,int column)
	{
		string[] fileData = File.ReadAllLines(path);
		string[] values = fileData[row].Split(',');
		return values[column];
	}

	public static string GetValueFromXLSX(string path, int row, int column)
	{
		string strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + path + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'";
		using (OleDbConnection conn = new OleDbConnection(strConn))
		{
			conn.Open();
			using (OleDbCommand cmd = new OleDbCommand($"SELECT * FROM [Sheet1$]", conn)) // Assuming you want to read from the first sheet
			{
				using (OleDbDataReader reader = cmd.ExecuteReader())
				{
					while (reader.Read())
					{
						int currentRow = reader.GetOrdinal("F" + row); // Assuming column headers start with F
						if (currentRow != -1)
						{
							string cellValue = reader.GetValue(currentRow).ToString();
							return cellValue;
						}
					}
				}
			}
		}
		return string.Empty; // Return empty string if cell is not found
	}
}
