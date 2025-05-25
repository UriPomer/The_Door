using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour,IGetStat
{
	public Action OnHealthEqualsZero;

	public enum Stats
	{
		Health,
		Range,
		Damage,
		Defense,
		AttackSpeed,
		FireInterval,
		HitCount,
		DamageRate,
	}

	[System.Serializable]
	protected struct StatToValue
	{
		public Stats stat;
		public float value;
	}

	
	[SerializeField] private List<StatToValue> statsList = new()
	{
		new StatToValue {stat = Stats.Health, value = 100f},
		new StatToValue {stat = Stats.Range, value = 10f},
		new StatToValue {stat = Stats.Damage, value = 10f},
		new StatToValue {stat = Stats.Defense, value = 0f},
		new StatToValue {stat = Stats.AttackSpeed, value = 1f},
		new StatToValue {stat = Stats.FireInterval, value = 4f},
		new StatToValue {stat = Stats.HitCount, value = 1f},
	};

	protected Dictionary<Stats,float> stats = new();

	protected virtual void Awake()
	{
		foreach (var stat in statsList)
		{
			stats.Add(stat.stat, stat.value);
		}
	}

	public virtual float GetStat(Stats stat)
	{
		if (stats.ContainsKey(stat))
		{
			return stats[stat];
		}
		else
		{
			throw new System.Exception("Attribute not found :" + name);
		}
	}

	public void AddStat(Stats stat, float value)
	{
		if(stats.ContainsKey(stat))
		{
			stats[stat] += value;
		}
		else
		{
			stats.Add(stat, value);
		}
	}

	public void TakeDamage(float damage)
	{
		stats[Stats.Health] -= damage;
		if (stats[Stats.Health] <= 0)
		{
			stats[Stats.Health] = 0;
			OnHealthEqualsZero?.Invoke();
		}
	}
}
