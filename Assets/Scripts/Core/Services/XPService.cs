using System;
using UnityEngine;
using Zenject;

public interface IXPService
{
    int Experience { get; set; }
    int Level { get; }
    float NextLevelXP { get; }
    event Action<int> OnExperienceChanged;
    event Action<int> OnLevelChanged;
}

public class XPService : IXPService
{
    readonly AnimationCurve _curve;
    readonly int _baseXP, _xpOffset;
    int _exp = 0, _level = 0;
    float _nextLevelXP;

    public event Action<int> OnExperienceChanged;
    public event Action<int> OnLevelChanged;

    [Inject]
    public XPService([Inject(Id="XP_CURVE")] AnimationCurve curve,
        [Inject(Id="BASE_XP")] int baseXP, [Inject(Id="XP_OFFSET")] int xpOffset)
    {
        _curve = curve;
        _baseXP = baseXP;
        _xpOffset = xpOffset;
        _nextLevelXP = _baseXP * _curve.Evaluate(_level) + _xpOffset;
    }

    public int Experience
    {
        get => _exp;
        set
        {
            _exp = Mathf.Max(0, value);
            OnExperienceChanged?.Invoke(_exp);
            if (_exp >= _nextLevelXP)
            {
                _level++;
                OnLevelChanged?.Invoke(_level);
                _exp = 0;
                _nextLevelXP += _baseXP * _curve.Evaluate(_level) + _xpOffset;
            }
        }
    }

    public int Level => _level;
    public float NextLevelXP => _nextLevelXP;
}