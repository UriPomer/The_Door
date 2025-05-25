using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;  // 如果你用的是 Zenject，留着它；否则删掉这一行

public abstract class UIController : MonoBehaviour
{
    // 缓存所有子物体的引用，key 为层级路径：比如 "RootPanel/StartButton"
    protected readonly Dictionary<string, GameObject> view = new();

    // 如果你用 UIService，你可以直接在子类里通过 UIService 关闭自己
    [Inject] protected IUIService UIService;

    // Awake 时递归缓存所有子节点
    protected virtual void Awake()
    {
        CacheViews(transform, "");
    }

    // 递归遍历所有子节点，把路径 + 名字当做 key
    private void CacheViews(Transform parent, string path)
    {
        foreach (Transform child in parent)
        {
            string childPath = string.IsNullOrEmpty(path) ? child.name : $"{path}/{child.name}";
            if (!view.ContainsKey(childPath))
                view[childPath] = child.gameObject;

            CacheViews(child, childPath);
        }
    }

    /// <summary>
    /// 根据层级路径获取子物体上的组件 T
    /// </summary>
    protected T GetViewComponent<T>(string path) where T : Component
    {
        if (view.TryGetValue(path, out var go))
        {
            var comp = go.GetComponent<T>();
            if (comp == null)
                Debug.LogWarning($"Controller: '{path}' 上没有组件 {typeof(T).Name}");
            return comp;
        }
        Debug.LogWarning($"Controller: 没有找到 View 路径 '{path}'");
        return null;
    }

    /// <summary>
    /// 快速给按钮（按路径定位）绑定点击回调
    /// </summary>
    protected void AddButtonListener(string path, UnityEngine.Events.UnityAction callback)
    {
        var btn = GetViewComponent<Button>(path);
        if (btn != null)
            btn.onClick.AddListener(callback);
    }
}