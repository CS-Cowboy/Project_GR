using UnityEngine;


namespace com.braineeeeDevs.gr
{
    public class ObjectAttributes : ScriptableObject
    {
        /// <summary>
        /// We are using metric in this simulation.
        /// </summary>
        public float topSpeed, mass, drag, fireDelayTime, reloadDelayTime;
        
        public System.Guid poolID = System.Guid.Empty; //Important for ID persistance and pooling of objects.
        public uint poolCapacity = 5;

    }
}
