using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSpear : FollowColliderSummon
{
	protected override void Awake()
	{
		base.Awake();
		followTarget = GameObject.FindWithTag("Player");
	}
}
