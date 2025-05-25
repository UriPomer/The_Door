using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;               // ← 注入用

public class UISkillChooser_Ctrl : UIController
{
    public List<GameObject> skillPrefabList = new List<GameObject>();

    [Inject] private IUIService _uiService;    // ← 注入 UIService

    protected override void Awake()
    {
        base.Awake();
        // Awake 里不用再做额外事情
    }

    public void Start()
    {
        // 1. 加载所有技能 Prefab
        string pathToResources = "Prefabs/Weapons/Summons";
        GameObject[] prefabs = Resources.LoadAll<GameObject>(pathToResources);
        skillPrefabList.AddRange(prefabs);

        // 2. 随机挑 3 个
        List<GameObject> skillPrefabListRandom = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, skillPrefabList.Count);
            skillPrefabListRandom.Add(skillPrefabList[randomIndex]);
        }

        // 3. 给每个按钮绑定图标和回调
        int index = 0;
        foreach (Button button in GetComponentsInChildren<Button>())
        {
            var skillPrefab = skillPrefabListRandom[index++];
            var skillSprite = skillPrefab.GetComponent<SpriteRenderer>().sprite;
            button.image.sprite = skillSprite;

            button.onClick.AddListener(() =>
            {
                // 原有逻辑：给武器槽添加武器
                var weaponSlot = GameObject
                    .FindGameObjectWithTag("WeaponSlot")
                    .GetComponent<WeaponSlot>();
                weaponSlot.AddWeapon(skillPrefab);

                // 原有逻辑：恢复时间流逝
                Time.timeScale = 1;

                // 改造点：不要直接 Destroy，这里用 UIService 关闭自己
                _uiService.CloseUI<UISkillChooser_Ctrl>();
            });
        }

        // 面板打开时暂停游戏
        Time.timeScale = 0;
    }
}