
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Centauri.NetDebug
{
    public class NetworkTestBehaviour : UdonSharpBehaviour
    {
        [UdonSynced] public int syncedValue;

        public bool requestSerialize;

        public float syncTime = 1f;

        private void Start()
        {
            SendCustomEventDelayedSeconds(nameof(SerializationLoop), syncTime);
        }

        public void SerializationLoop()
        {
            SendCustomEventDelayedSeconds(nameof(SerializationLoop), syncTime);

            if (Networking.IsOwner(gameObject))
            {
                syncedValue = Random.Range(0, 100);

                if (requestSerialize)
                {
                    RequestSerialization();
                }
            }
        }
    }
}
