using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindClosestObject : MonoBehaviour
{
    public static GameObject Find(GameObject thisObject, ObjectType objectType)
    {
		GameObject closestObject = null;
		float closestDistance = float.MaxValue;

		// 获取所有指定类型的游戏对象
		GameObject[] objectsOfType = GameObject.FindGameObjectsWithTag(objectType.ToString());

		if(objectsOfType.Length == 0)
		{
			return null;
		}
		foreach (GameObject obj in objectsOfType)
		{
			if (obj != thisObject)
			{
				float distance = Vector3.Distance(thisObject.transform.position, obj.transform.position);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestObject = obj;
				}
			}
		}

		return closestObject;
	}

	public static GameObject FindExcept(GameObject thisObject, List<GameObject> exceptObject, ObjectType objectType)
	{
		GameObject closestObject = null;
		float closestDistance = float.MaxValue;

		// 获取所有指定类型的游戏对象
		GameObject[] objectsOfType = GameObject.FindGameObjectsWithTag(objectType.ToString());

		if (objectsOfType.Length == 0)
		{
			return null;
		}
		foreach (GameObject obj in objectsOfType)
		{
			if (obj != thisObject && !exceptObject.Contains(obj))
			{
				float distance = Vector3.Distance(thisObject.transform.position, obj.transform.position);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestObject = obj;
				}
			}
		}

		return closestObject;
	}

	public static GameObject FindExcept(GameObject thisObject, GameObject exceptObject, ObjectType objectType)
	{
		GameObject closestObject = null;
		float closestDistance = float.MaxValue;

		// 获取所有指定类型的游戏对象
		GameObject[] objectsOfType = GameObject.FindGameObjectsWithTag(objectType.ToString());

		if (objectsOfType.Length == 0)
		{
			return null;
		}
		foreach (GameObject obj in objectsOfType)
		{
			if (obj != thisObject && obj != exceptObject)
			{
				float distance = Vector3.Distance(thisObject.transform.position, obj.transform.position);
				if (distance < closestDistance)
				{
					closestDistance = distance;
					closestObject = obj;
				}
			}
		}

		return closestObject;
	}
}
