using System;
using System.Collections;
using UnityEngine;
namespace com.braineeeeDevs.objectPooling
{
	/// <summary>
	/// An abstract class to represent generic gameobjects which are poolable and require rigidbody physics, sound effects, and animations. Cannot be directly instantiated. You must derive from it to see it exist in game.
	/// </summary>

	[RequireComponent(typeof(PoolableAnimator))]

	public abstract class PoolableObject : MonoBehaviour
	{
		public Properties traits;
		protected float healthPoints = 1f, lifeSpan = -1f;
		protected Coroutine deathWait;
		protected Guid id;
		protected PoolableAnimator selfAnimator;

		public Properties Traits
		{
			get
			{
				return traits;
			}
		}
		public Guid PoolID
		{
			get
			{
				return id;
			}
		}
		public virtual void Awake()
		{
			if (traits.poolID == Guid.Empty.ToString() || traits.poolID == String.Empty)
			{
				id = Guid.NewGuid();
				traits.poolID = id.ToString();
			}
			else
			{
				id = new Guid(traits.poolID);
			}
			selfAnimator = GetComponent<PoolableAnimator>();
		}
		public virtual void OnEnable()
		{
			InitializePoolable();
			if (lifeSpan > 0f && deathWait == null)	 	//Negative lifespan indicates object postpones death indefinitely
			{
				deathWait = StartCoroutine(WaitForDeath());
			}
		}
		
		///6D 00 65 00 6D 00 65 00 6E 00 74 00 6F 00 20 00 6D 00 6F 00 72 00 69 00
		protected IEnumerator WaitForDeath() 	//Stage 0; Object is alive and counting down its time
		{
			yield return new WaitForSeconds(lifeSpan);
			selfAnimator.Animate(traits.deathState, traits.deathAnimationSpeed); //Stage 1; Object is in the process of dying. 
			yield return new WaitForSeconds(traits.deathWaitLength);
			selfAnimator.Animate(traits.decomposeState, traits.decompositionAnimationSpeed); //Stage 2; Object is dead but still exists in the world.
			yield return new WaitForSeconds(traits.decompositionWaitLength);
			gameObject.SetActive(false); //Stage 3; object is removed from the world and again pooled.
		}
		

		protected void InitializePoolable()
		{
			healthPoints = traits.healthpoints;
			lifeSpan = traits.lifespan;
		}
		public virtual void OnDisable()
		{
			deathWait = null;
			PoolHandler.Give(this);
		}
		public virtual void TeleportTo(Transform point)
		{
			transform.position = point.position;
			transform.rotation = point.rotation;
		}
	}
}