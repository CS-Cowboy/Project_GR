
using UnityEngine;
using System.Collections.Generic;

namespace com.braineeeeDevs.objectPooling
{
	public class Calculus2D : Calculus
	{
		[SerializeField] IDifferentiate2D calculusTarget;
		public override void OnEnable()
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

		public override void OnDisable()
		{
			if (routine != null)
			{
				StopCoroutine(this.ComputeUsingPhysicsTime());
			}
		}
		/// <summary>
		/// Computes the derivative and sets the input for first and second differentiable Vector3's. 
		/// </summary>  
		protected override IEnumerator <WaitForFixedUpdate> ComputeUsingPhysicsTime()
		{            
			Vector2 pos, vel = Vector2.zero, accel = Vector2.zero;

			while (true)
			{
				pos = calculusTarget.GetInput();
				vel = calculusTarget.Differentiate(pos, ref vel);
				accel = calculusTarget.Differentiate(vel, ref accel);
                calculusTarget.ReturnComputation(pos, vel, accel);
				yield return new WaitForFixedUpdate();
			}
		}
		protected override IEnumerator <WaitForSeconds> ComputeUsingDeltaTime()
		{
            Vector2 pos, vel = Vector2.zero, accel = Vector2.zero;
			while (true)
			{
				pos = calculusTarget.GetInput();
				vel = calculusTarget.Differentiate(pos, ref vel);
				accel = calculusTarget.Differentiate(vel, ref accel);
                calculusTarget.ReturnComputation(pos, vel, accel);
				yield return new WaitForSeconds(Time.deltaTime);
			}
		}

		public interface IDifferentiate2D
		{
			Vector2 GetInput();
			Vector2 Differentiate(Vector2 a, ref Vector2 b);
			void ReturnComputation(Vector2 pos, Vector2 vel, Vector2 accel);
		}

	}
}