using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class UICreatorUtil
{
    public static void GenerateUIController(string filePath, string className)
	{
		StreamWriter streamWriter = null;
		if(File.Exists(filePath + "/" + className + ".cs"))
		{
			Debug.LogWarning("File already exists: " + filePath + "/" + className + ".cs");
			return;
		}
		try
		{
			string path = filePath + "/" + className + ".cs";
			streamWriter = new StreamWriter(path);
			streamWriter.WriteLine("// Path: " + path);
			streamWriter.WriteLine("using System.Collections;");
			streamWriter.WriteLine("using System.Collections.Generic;");
			streamWriter.WriteLine("using UnityEngine;");
			streamWriter.WriteLine("using UnityEngine.UI;");
			streamWriter.WriteLine("");
			streamWriter.WriteLine("public class " + className + " : Controller");
			streamWriter.WriteLine("{");
			streamWriter.WriteLine("	public override void Awake()");
			streamWriter.WriteLine("	{");
			streamWriter.WriteLine("		base.Awake();");
			streamWriter.WriteLine("	}");
			streamWriter.WriteLine("");
			streamWriter.WriteLine("	public void Start()");
			streamWriter.WriteLine("	{");
			streamWriter.WriteLine("		");
			streamWriter.WriteLine("	}");
			streamWriter.WriteLine("}");
		}
		catch(System.Exception e)
		{
			Debug.LogError(e.Message);
		}
		finally
		{
			if(streamWriter != null)
			{
				streamWriter.Flush();
				streamWriter.Close();
			}
		}
	}
}
