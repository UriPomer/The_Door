using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    private Animation anim = null;
	private int state = (int)PlayerState.Invalid;

	public void Init()
	{
		this.anim = this.gameObject.GetComponent<Animation>();
		this.SetState((int)PlayerState.Idle);
	}

	public void SetState(int state)
	{
		if(this.state == state)
		{
			return;
		}

		this.state = state;
		//switch(this.state)
		//{
		//	case (int)PlayerState.Idle:
		//		this.anim.CrossFade("idle");
		//		break;
		//	case (int)PlayerState.Walk:
		//		this.anim.CrossFade("walk");
		//		break;
		//	case (int)PlayerState.Run:
		//		this.anim.CrossFade("run");
		//		break;
		//	case (int)PlayerState.Jump:
		//		this.anim.CrossFade("jump");
		//		break;
		//	case (int)PlayerState.Attack1:
		//		this.anim.CrossFade("attack1");
		//		break;
		//	case (int)PlayerState.Attack2:
		//		this.anim.CrossFade("attack2");
		//		break;
		//	case (int)PlayerState.Attack3:
		//		this.anim.CrossFade("attack3");
		//		break;
		//	case (int)PlayerState.Skill1:
		//		this.anim.CrossFade("skill1");
		//		break;
		//	case (int)PlayerState.Skill2:
		//		this.anim.CrossFade("skill2");
		//		break;
		//	case (int)PlayerState.Skill3:
		//		this.anim.CrossFade("skill3");
		//		break;
		//	case (int)PlayerState.Dead:
		//		this.anim.CrossFade("dead");
		//		break;
		//	default:
		//		break;
		//}
		//end
	}
}
