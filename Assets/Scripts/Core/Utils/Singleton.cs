using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class Singleton<T> where T : new()
{
	private static T _instance;
	private static readonly object mutex = new object();
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (mutex)
				{
					if (_instance == null)
					{
						_instance = new T();
					}
				}
			}
			return _instance;
		}
	}
	
}

public class UnitySingleton<T> : MonoBehaviour where T : Component
{
	private static T _instance = null;
	public static T Instance
	{
		get
		{
			if (isQuitting)
			{
				Debug.LogWarning($"Trying to access {typeof(T).Name} during application quit. Returning null.");
				return null;
			}
			
			if (_instance == null)
			{
				_instance = FindObjectOfType(typeof(T)) as T;
				if(_instance == null)
				{
					GameObject obj = new GameObject();
					obj.hideFlags = HideFlags.HideAndDontSave;
					_instance = (T)obj.AddComponent(typeof(T));
					obj.name = typeof(T).Name;
				}
			}
			return _instance;
		}
	}

	public virtual void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		if (_instance == null)
		{
			_instance = this as T;
		}
		else
		{
			Destroy(this.gameObject);
		}
	}
	
	protected virtual void OnDestroy()
	{
		if (_instance == this)
		{
			_instance = null;
		}
	}
	
	private static bool isQuitting = false;

	protected virtual void OnApplicationQuit()
	{
		isQuitting = true;
	}
}
