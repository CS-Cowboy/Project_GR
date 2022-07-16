
using UnityEngine;
using System.Collections.Generic;

namespace com.braineeeeDevs.objectPooling
{
	public class Calculus3D : Calculus
	{
		[SerializeField]  IDifferentiate3D calculusTarget;


		
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
				StopCoroutine(routine);
			}
		}
		/// <summary>
		/// Computes the derivative and sets the input for first and second differentiable Vector3's. 
		/// </summary>  
		protected override IEnumerator <WaitForFixedUpdate> ComputeUsingPhysicsTime()
		{            
			Vector3 pos, vel = Vector3.zero, accel = Vector3.zero;

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
			Vector3 pos, vel = Vector3.zero, accel = Vector3.zero;

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
	public interface IDifferentiate3D
	{
		Vector3 GetInput();
		Vector3 Differentiate(Vector3 a, ref Vector3 b);
		void ReturnComputation(Vector3 pos, Vector3 vel, Vector3 accel);
	}


}