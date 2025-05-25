using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public interface IUIService
{
    T ShowUI<T>() where T : UIController;
    void CloseUI<T>() where T : UIController;
}

public class UIService : IUIService, IInitializable, IDisposable
{
    readonly DiContainer _container;
    readonly Transform _canvasRoot;
    readonly UIConfig _uiConfig;              // ScriptableObject，存 Prefab 映射
    readonly Dictionary<Type, UIController> _opened = new();

    [Inject]
    public UIService(DiContainer container,
        [Inject(Id="CanvasRoot")] Transform canvasRoot,
        UIConfig uiConfig)
    {
        _container   = container;
        _canvasRoot  = canvasRoot;
        _uiConfig    = uiConfig;
    }

    public void Initialize()
    {
        // 可以在这里做一次预加载，或清空历史
    }

    public T ShowUI<T>() where T : UIController
    {
        var type = typeof(T);
        if (_opened.TryGetValue(type, out var existing))
            return existing as T;

        // 从配置里取对应的 Prefab
        var prefab = _uiConfig.GetPrefab<T>();
        var go = GameObject.Instantiate(prefab, _canvasRoot, false);
        var ctrl = _container.InstantiateComponent<T>(go);
        _opened[type] = ctrl;
        return ctrl;
    }

    public void CloseUI<T>() where T : UIController
    {
        var type = typeof(T);
        if (_opened.TryGetValue(type, out var ctrl))
        {
            Object.Destroy(ctrl.gameObject);
            _opened.Remove(type);
        }
    }

    public void Dispose()
    {
        // 场景切换或退出时统一清理
        foreach (var ctrl in _opened.Values)
            Object.Destroy(ctrl.gameObject);
        _opened.Clear();
    }
}

#if UNITY_EDITOR
[CreateAssetMenu(
    menuName = "Configs/UIConfig",    // 菜单路径
    fileName = "UIConfig",            // 默认创建的资源名字
    order    = 0                      // 在菜单中的排序（可选）
)]
#endif
public class UIConfig : ScriptableObject
{
    [Serializable]
    struct Entry
    {
        public string typeName;       // typeof(T).FullName
        public GameObject prefab;
    }

    [SerializeField] List<Entry> entries = new();

    public GameObject GetPrefab<T>() where T : UIController
    {
        var UIName = typeof(T).FullName;
        var entry = entries.FirstOrDefault(e => e.typeName == UIName);
        if (entry.prefab == null)
            throw new Exception($"UIConfig 中没有找到类型 {UIName} 的 Prefab 映射");
        return entry.prefab;
    }
}

