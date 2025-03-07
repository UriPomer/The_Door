using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using ExcelDataReader;

public class ExcelReader
{
	public static string GetValueFromExcel(string path, int row, int column, int table = 0)
	{
		if (path.Substring(path.Length - 5, 5) == ".xlsx")
		{
			using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read))
			{
				var reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
				var result = reader.AsDataSet();
				return result.Tables[table].Rows[row][column].ToString();
			}
		}
		else if(path.Substring(path.Length - 4, 4) == ".xls")
		{
			using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read))
			{
				var reader = ExcelReaderFactory.CreateBinaryReader(stream);
				var result = reader.AsDataSet();
				return result.Tables[table].Rows[row][column].ToString();
			}
		}
		else if(path.Substring(path.Length - 4, 4) == ".csv")
		{
			using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read))
			{
				var reader = ExcelReaderFactory.CreateCsvReader(stream);
				var result = reader.AsDataSet();
				return result.Tables[table].Rows[row][column].ToString();
			}
		}
		else
		{
			return "文件格式不正确";
		}
	}
}
