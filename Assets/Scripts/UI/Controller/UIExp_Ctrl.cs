using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIExp_Ctrl : UIController
{
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Slider          _xpSlider;
    
    // 注入 XP 服务
    IXPService    _xpService;
    SignalBus     _signalBus;
    IUIService    _uiService;

    [Inject]
    public void Construct(
        IXPService xpService,
        SignalBus  signalBus,
        IUIService uiService)
    {
        _xpService = xpService;
        _signalBus = signalBus;
        _uiService = uiService;
    }

    void OnEnable()
    {
        // 订阅并立即刷新一次
        _xpService.OnExperienceChanged += UpdateXP;
        _xpService.OnLevelChanged      += UpdateLevel;

        // UpdateLevel(_xpService.Level);
        UpdateXP   (_xpService.Experience);
    }

    void OnDisable()
    {
        // 一定要解绑！
        _xpService.OnExperienceChanged -= UpdateXP;
        _xpService.OnLevelChanged      -= UpdateLevel;
    }

    void UpdateXP(int xp)
    {
        _xpSlider.value = xp / _xpService.NextLevelXP;
    }

    void UpdateLevel(int level)
    {
        _levelText.text = $"Level : {level}";
        _uiService.ShowUI<UISkillChooser_Ctrl>();
    }
}