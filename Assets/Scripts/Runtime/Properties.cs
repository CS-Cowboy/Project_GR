using UnityEngine;


namespace com.braineeeeDevs.objectPooling
{
    [RequireComponent(typeof(AnimationCurve))]
    public class Properties : ScriptableObject
    {
        /// <summary>
        /// We are using metric in this simulation.
        /// </summary>
        public float topSpeed, mass, drag, healthpoints, lifespan, battleValue;
        public string poolID = System.Guid.Empty.ToString();
        public uint poolCapacity = 5;

    }
}