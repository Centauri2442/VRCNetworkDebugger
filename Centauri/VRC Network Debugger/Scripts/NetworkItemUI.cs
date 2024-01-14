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
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace Centauri.NetDebug
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class NetworkItemUI : UdonSharpBehaviour
    {
        public GameObject RemoteOnly;
        public GameObject OwnerOnly;

        public GameObject bytesPerSecond;
        public GameObject totalBytes;
        
        public TextMeshProUGUI Name;
        public bool showOwner;
        public string objectName;
        public string ownerName;
        public TextMeshProUGUI BytesOut;
        public TextMeshProUGUI TotalBytes;
        public TextMeshProUGUI TimeSinceLastSerialization;
        public Image SerializationFailed;
        public Color FailedColor;
        public Color HiddenColor;
        
        public TextMeshProUGUI sendTimeText;
        public TextMeshProUGUI receiveTimeText;

        private float timeSinceSync;

        private void Start()
        {
            SendCustomEventDelayedFrames(nameof(UpdateTime), 1);
        }

        public void UpdateTime()
        {
            SendCustomEventDelayedFrames(nameof(UpdateTime), 1);
            
            timeSinceSync += Time.deltaTime;

            if (!gameObject.activeInHierarchy) return;

            Name.text = showOwner ? ownerName : objectName;
            
            SerializationFailed.color = Color.Lerp(SerializationFailed.color, HiddenColor, Time.deltaTime);
            
            TimeSinceLastSerialization.text = timeSinceSync.ToString();
        }

        public void ToggleShowOwner()
        {
            showOwner = !showOwner;
            
            Name.text = showOwner ? ownerName : objectName;
        }

        public void UpdateBytesOut(int bytes)
        {
            BytesOut.text = bytes.ToString();

            if (bytes > 0)
            {
                timeSinceSync = 0f;
            }
        }

        public void UpdateTotalBytes(int bytes)
        {
            TotalBytes.text = bytes.ToString();

            if (bytes > 0)
            {
                timeSinceSync = 0f;
            }
        }

        public void UpdateOwner(bool isOwner)
        {
            OwnerOnly.SetActive(isOwner);
            RemoteOnly.SetActive(!isOwner);
        }

        public void ShowTotal(bool value)
        {
            bytesPerSecond.SetActive(!value);
            totalBytes.SetActive(value);
        }

        public void UpdateRemote(float sendTime, float receiveTime)
        {
            timeSinceSync = 0f;
            sendTimeText.text = sendTime.ToString();
            receiveTimeText.text = receiveTime.ToString();
        }

        public void DataFailed()
        {
            SerializationFailed.color = FailedColor;
        }
    }
}
