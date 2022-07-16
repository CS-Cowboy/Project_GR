
using UnityEngine;
using System.Collections.Generic;

namespace com.braineeeeDevs.objectPooling
{
	public class Calculus : MonoBehaviour
	{
		[SerializeField] IDifferentiate calculusTarget;
		protected Coroutine routine;
		public bool usePhysicsTime = false;

		public virtual void OnEnable()
		{
			if (routine == null)
			{
				if(usePhysicsTime)
				{
					routine = StartCoroutine(this.ComputeUsingPhysicsTime());
				} else 
				{
					routine = StartCoroutine(this.ComputeUsingDeltaTime());
				}
			}
		}
        public virtual void OnDisable()
		{
			if (routine != null)
			{
				StopCoroutine(this.ComputeUsingPhysicsTime());
			}
		}
		/// <summary>
		/// Computes the derivative and sets the input for first and second differentiable Vector3's. 
		/// </summary>  
		protected virtual IEnumerator <WaitForFixedUpdate> ComputeUsingPhysicsTime()
		{
            float pos, vel = 0f, accel = 0f;
			while (true)
			{
				pos = calculusTarget.GetInput();
				vel = calculusTarget.Differentiate(pos, ref vel);
				accel = calculusTarget.Differentiate(vel, ref accel);
                calculusTarget.ReturnComputation(pos, vel, accel);
				yield return new WaitForFixedUpdate();
			}
		}
		protected virtual IEnumerator <WaitForSeconds> ComputeUsingDeltaTime()
		{
            float pos, vel = 0f, accel = 0f;
			while (true)
			{
				pos = calculusTarget.GetInput();
				vel = calculusTarget.Differentiate(pos, ref vel);
				accel = calculusTarget.Differentiate(vel, ref accel);
                calculusTarget.ReturnComputation(pos, vel, accel);
				yield return new WaitForSeconds(Time.deltaTime);
			}
		}
	}
	public interface IDifferentiate
	{

		/*
				/// <summary>
				/// Example differentiation function. 
				/// </summary>
				/// <param name="b">The current value.</param>
				/// <param name="a">The old value.</param>
				/// <returns>The differentiated value.</returns>        
				protected float Differentiate(float b, ref float a)
				{
					var diff = b - a;
					a = b;
					return diff;
				}
		*/		float GetInput();
		float Differentiate(float a, ref float b);
		void ReturnComputation(float pos, float vel, float accel);
	}

}
