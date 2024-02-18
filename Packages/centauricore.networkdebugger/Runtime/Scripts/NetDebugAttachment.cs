/*
Copyright 2024 CentauriCore

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common;

namespace Centauri.NetDebug
{
    public class NetDebugAttachment : UdonSharpBehaviour
    {
        public NetworkDebugger Debugger;
        public int debugIndex;
        public int MaxHeaders;
        public bool hasArrsOrStrings;
        public string[] allDataTypes;

        private bool justFiredPostSerialization;

        public void InitializeScript()
        {
            Debugger.UpdateUIOwner(debugIndex, Networking.GetOwner(gameObject).displayName);
        }

        
        public override void OnPostSerialization(SerializationResult result)
        {
            if (justFiredPostSerialization)
            {
                Debug.LogError("<color=red>Serialization fired twice on debug attachment!</color>");
                return;
            }
            justFiredPostSerialization = true;
            
            SendCustomEventDelayedFrames(nameof(ResetSerialization), 2);
            
            Debugger.AddBytes(debugIndex, result.byteCount);
            
            Debugger.ItemUIs[debugIndex].timeSinceSync = 0f;

            if (!result.success)
            {
                Debugger.SerializationFailed(debugIndex);
            }
        }

        public void ResetSerialization()
        {
            justFiredPostSerialization = false;
        }

        public override void OnOwnershipTransferred(VRCPlayerApi player)
        {
            Debugger.UpdateUIOwner(debugIndex, player.displayName);
        }

        public override void OnDeserialization(DeserializationResult result)
        {
            if (!Debugger.ShowRemote) return;
            
            Debugger.AddResultData(debugIndex, result.sendTime, result.receiveTime);
            
        }
    }
}
