using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : UnitySingleton<GameMgr>
{
    public GameState state { get; private set; } = GameState.Init;


    [SerializeField]private readonly int _maxExpNeeded = 100;
    private int _experience = 0;
	private int _playerLevel = 0;
    public AnimationCurve _xpLevelCapIncreaseCurve;

	public override void Awake()
	{
        base.Awake();
        if(!this.gameObject.GetComponent<MapMgr>())
		this.gameObject.AddComponent<MapMgr>();
	}

	public void Start()
	{
        OnLevelChanged += OnLvChanged;
		_nextLevelXP = _maxExpNeeded * _xpLevelCapIncreaseCurve.Evaluate(_playerLevel);
	}

	public int Experience
    {
        get { return _experience; }
        set
        {
            _experience = Mathf.Max(value, 0);
            if(Experience >= _nextLevelXP)
            {
                PlayerLevel = _playerLevel + 1;
                _lastLevelUpXP = _nextLevelXP;
                _nextLevelXP += _xpLevelCapIncreaseCurve.Evaluate(_playerLevel) * _maxExpNeeded;
                _experience = 0;
            }
            OnExperienceChanged?.Invoke(_experience);
        }
    }

    private int _score = 0;
    public int Score
    {
		get { return _score; }
		set
        {
			_score = Mathf.Max(value, 0);
			OnScoreChanged?.Invoke(_score);
		}
	}

    public int PlayerLevel
    {
        get { return _playerLevel; }
        set
        {
            _playerLevel = Mathf.Max(value, 0);
            OnLevelChanged?.Invoke(_playerLevel);
        }
    }

    public float NextLevelXP
    {
		get { return _nextLevelXP; }
	}

    private float _lastLevelUpXP = 0;
    private float _nextLevelXP = 0;
	

    public event Action<int> OnExperienceChanged;
    public event Action<int> OnLevelChanged;
    public event Action<int> OnScoreChanged;

    [Serializable]
	public enum GameState
	{
		Init,
		Start,
		Pause,
		Resume,
		End
	}
	private void OnLvChanged(int Level)
    {
        UIController skillChooser = UIMgr.Instance.ShowUI("UISkillChooser");
    }
}
