using UnityEngine;


namespace com.braineeeeDevs.objectPooling
{
	[CreateAssetMenu()]
    public class Properties : ScriptableObject
    {
        /// <summary>
        /// We are using metric in this simulation.
        /// </summary>
        public float topSpeed, mass, drag, healthpoints = 1f, lifespan = -1f, deathWaitLength = 3f, decompositionAnimationSpeed, decompositionWaitLength, battleValue, deathAnimationSpeed = 0.25f;
        public string poolID = System.Guid.Empty.ToString(), deathState, decomposeState;
        public uint poolCapacity = 5;

    }
}