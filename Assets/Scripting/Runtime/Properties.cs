using UnityEngine;


namespace com.braineeeeDevs.objectPooling
{
	[CreateAssetMenu()]
    public class Properties : ScriptableObject
    {
        public float healthpoints = 1f, lifespan = -1f, deathWaitLength = 3f, decompositionAnimationSpeed, decompositionWaitLength, battleValue;
        public string poolID = System.Guid.Empty.ToString(), deathState, decomposeState;
        public uint poolCapacity = 5;
        public AudioClip onSpawnSound, onDeathSound;

    }
}