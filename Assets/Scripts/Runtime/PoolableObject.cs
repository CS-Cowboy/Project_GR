using System;
using UnityEngine;
namespace com.braineeeeDevs.objectPooling
{
    /// <summary>
    /// An abstract class to represent generic gameobjects which are poolable and require rigidbody physics, sound effects, and animations. Cannot be directly instantiated. You must derive from it to see it exist in game.
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshFilter))]
    public abstract class PoolableObject : MonoBehaviour
    {
        public Properties traits;
        protected Guid id;
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
        }

        public void OnDisable()
        {            
            PoolHandler.GiveObject(this);
        }
        /// <summary>
        /// Virtual method. Use to spawn this object at a particular place in the world. 
        /// </summary>
        /// <param name="point">The transform representing the orientation and position to spawn at in world space.</param>
        public virtual void SpawnAt(Transform point)
        {
            transform.position = point.position;
            transform.rotation = point.rotation;
        }

        public interface IAnimateSelf
        {
            void Animate(string animationToPlay, float speed);
        }
    }
}