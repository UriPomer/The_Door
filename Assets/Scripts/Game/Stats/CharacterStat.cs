using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class CharacterStat : Stat
{
	public override float GetStat(Stats stat)
	{
		float value = base.GetStat(stat);
		foreach(var st in GetComponents<Stat>())
		{
			if(st == this)
			{
				continue;
			}
			value += st.GetStat(stat);
		}
		return value;
	}

}
