using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
	Player,
	Enemy,
}

public enum PlayerState
{
	Invalid = -1,
	Idle = 0,
	Walk = 1,
	Run = 2,
	Jump = 3,
	Attack1 = 4,
	Attack2 = 5,
	Attack3 = 6,

	Skill1 = 7,
	Skill2 = 8,
	Skill3 = 9,
	Dead = 10,
}
