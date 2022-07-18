using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.braineeeeDevs.objectPooling
{
	[RequireComponent(typeof(Animator))]
	public class PoolableAnimator : MonoBehaviour
	{
		protected Animator animator;
		protected void Awake()
		{
			animator = GetComponent<Animator>();
		}
		public void Stop()
		{
			animator.StopPlayback();
		}
		public void Animate(string stateToPlay, float speed)
		{
			animator.SetFloat(stateToPlay , speed);
		}
		public void TriggerAnimation(string stateToPlay)
		{
			animator.SetBool(stateToPlay, true);
		}

	}
}