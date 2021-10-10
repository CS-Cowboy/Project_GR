using UnityEngine;


namespace com.braineeeeDevs.gr
{
    public class ObjectAttributes : ScriptableObject
    {
        /// <summary>
        /// We are using metric in this simulation.
        /// </summary>
        public float topSpeed, mass, drag, fireDelayTime, reloadDelayTime;
        public uint poolCapacity = 5;

    }
}
