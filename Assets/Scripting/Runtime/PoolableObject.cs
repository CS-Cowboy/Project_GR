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
		[SerializeField] protected Properties traits;
		[SerializeField] protected float healthPoints = 1f, lifeSpan = -1f;
		protected Coroutine deathWait, onDeathRoutine;
		[SerializeField] protected Guid id;
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
			PlaySound(traits.onSpawnSound);
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

		protected virtual void Die()
		{
			StartCoroutine(OnDeath());
		}
		protected virtual IEnumerator OnDeath()
		{
			PlaySound(traits.onDeathSound);
			animator.SetTrigger(traits.deathState); //Stage 1; Object is in the process of dying. 
			yield return new WaitForSeconds(traits.deathWaitLength);
			animator.SetFloat(traits.decomposeState, traits.decompositionAnimationSpeed); //Stage 2; Object is dead but still exists in the world.
			yield return new WaitForSeconds(traits.decompositionWaitLength);
			animator.SetTrigger(traits.deathState); //Stage 1; Object is in the process of dying. 
			gameObject.SetActive(false); //Stage 3; object is removed from the world and again pooled.
		}
		protected virtual void StopSound(AudioClip clip)
		{
			if(sounds.clip == clip)
				sounds.Stop();
		}
		protected virtual void PlaySound(AudioClip snd)
		{
			if (sounds.clip != snd)
			{
				sounds.clip = snd;
			}

			if (!sounds.isPlaying)
			{
				sounds.Play();
			}
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