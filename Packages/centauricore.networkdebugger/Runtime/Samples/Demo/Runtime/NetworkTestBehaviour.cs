
using System;
using Centauri.Utilities;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;
using Random = UnityEngine.Random;

namespace Centauri.NetDebug
{
    public class NetworkTestBehaviour : UdonSharpBehaviour
    {
        public UdonBehaviour TEMPTARGET;
        
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
                if (requestSerialize)
                {
                    RequestSerialization();
                }
            }
        }
    }
}
