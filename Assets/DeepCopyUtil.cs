using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class DeepCopyUtil
{ 
	public static T DeepCopy<T>(T obj)
	{
		if (obj == null)
		{
			throw new ArgumentNullException(nameof(obj));
		}
			
		Type type = obj.GetType();
		if (type.IsValueType || type == typeof(string))
		{
			return obj;
		}
		else if (type.IsArray)
		{
			Type elementType = type.GetElementType();
			Array sourceArray = obj as Array;
			Array destinationArray = Array.CreateInstance(elementType, sourceArray.Length);
			for (int i = 0; i < sourceArray.Length; i++)
			{
				object sourceElement = sourceArray.GetValue(i);
				object destinationElement = DeepCopy(sourceElement);
				destinationArray.SetValue(destinationElement, i);
			}
			return (T)Convert.ChangeType(destinationArray, obj.GetType());
		}
		else if (type.IsClass)
		{
			object copy = Activator.CreateInstance(obj.GetType());
			FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			foreach (FieldInfo field in fields)
			{
				object fieldValue = field.GetValue(obj);
				object fieldCopy = DeepCopy(fieldValue);
				field.SetValue(copy, fieldCopy);
			}
			return (T)copy;
		}
		else
		{
			throw new ArgumentException("Unsupported type");
		}
	}
}
