using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIExp_Ctrl : UIController
{
    [SerializeField] private TMPro.TextMeshProUGUI _levelText;
    [SerializeField] private Slider _xpSilder;

	public override void Awake()
	{
		base.Awake();
		_levelText = transform.GetComponentInChildren<TMPro.TextMeshProUGUI>();
		_xpSilder = transform.GetComponent<Slider>();
	}

	public void Start()
	{
		GameMgr.Instance.OnExperienceChanged += OnExperienceChanged;
		GameMgr.Instance.OnLevelChanged += OnLevelChanged;
	}

	private void OnDestroy()
	{
		if(GameMgr.Instance != null)
			GameMgr.Instance.OnExperienceChanged -= OnExperienceChanged;
	}
	private void OnExperienceChanged(int newXP)
	{
		_xpSilder.value = (float)newXP / GameMgr.Instance.NextLevelXP;
	}

	private void OnLevelChanged(int newLevel)
	{
		_levelText.text = "Level : " + newLevel.ToString();
	}
}
