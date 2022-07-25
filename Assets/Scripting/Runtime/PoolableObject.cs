using System;
using System.Collections;
using UnityEngine;
namespace com.braineeeeDevs.objectPooling
{
	/// <summary>
	/// An abstract class to represent generic gameobjects which are poolable and require rigidbody physics, sound effects, and animations. Cannot be directly instantiated. You must derive from it to see it exist in game.
	/// </summary>

	[RequireComponent(typeof(Animator))]
	[RequireComponent(typeof(AudioSource))]

	public abstract class PoolableObject : MonoBehaviour
	{
		public Properties traits;
		protected float healthPoints = 1f, lifeSpan = -1f;
		protected Coroutine deathWait, onDeathRoutine;
		protected Guid id;
		protected Animator animator;
		protected AudioSource sounds;
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
			CheckForNull();
			animator = GetComponent<Animator>();
			sounds = GetComponent<AudioSource>();
		}
		protected virtual void CheckForNull()
		{
			if (traits == null)
			{
				Debug.LogWarningFormat("PoolableObject: {0} had null traits value and will be made inactive.", name);
				gameObject.SetActive(false);
				return;
			}
			else
			{
				if (traits.onSpawnSound == null || traits.onDeathSound == null)
				{
					Debug.LogWarningFormat("PoolableObject: {0} had null traits sound clips value and will be made inactive.", name);
					gameObject.SetActive(false);
					return;
				}
			}
		}
		public virtual void OnEnable()
		{
			InitializePoolable();
			sounds.playOnAwake = false;
			if (lifeSpan > 0f && deathWait == null)     //Negative lifespan indicates object postpones death indefinitely
			{
				deathWait = StartCoroutine(WaitForDeath());
			}
		}

		///6D 00 65 00 6D 00 65 00 6E 00 74 00 6F 00 20 00 6D 00 6F 00 72 00 69 00
		protected IEnumerator WaitForDeath()    //Stage 0; Object is alive and counting down its time
		{
			sounds.clip = traits.onSpawnSound;
			sounds.Play();
			yield return new WaitForSeconds(lifeSpan);
			Die();
		}

		public virtual void Damage(float value)
		{
			healthPoints = Mathf.Clamp(healthPoints - Mathf.Abs(value), 0f, traits.healthpoints);
			if (healthPoints == 0f)
			{
				Die();
			}
		}

		public virtual void Die()
		{
			StartCoroutine(OnDeath());
		}
		protected virtual IEnumerator OnDeath()
		{
			sounds.clip = traits.onDeathSound;
			sounds.Play();
			animator.SetBool(traits.deathState, true); //Stage 1; Object is in the process of dying. 
			yield return new WaitForSeconds(traits.deathWaitLength);
			animator.SetFloat(traits.decomposeState, traits.decompositionAnimationSpeed); //Stage 2; Object is dead but still exists in the world.
			yield return new WaitForSeconds(traits.decompositionWaitLength);
			animator.SetBool(traits.deathState, false); //Stage 1; Object is in the process of dying. 
			gameObject.SetActive(false); //Stage 3; object is removed from the world and again pooled.
		}

		protected virtual void PlaySound(AudioClip snd)
		{
			if (sounds.isPlaying)
			{
				sounds.Stop();
			}
			sounds.clip = snd;
			sounds.Play();
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