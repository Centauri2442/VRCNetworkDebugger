
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

namespace Centauri.NetDebug
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class DataFetcher : UdonSharpBehaviour
    {
        public NetworkDebugger Debugger;
        public VRCUrl NetworkDataURL;
        public TextAsset LocalBackup;
        
        private string rawJson;


        private void Start()
        {
            SendCustomEventDelayedSeconds(nameof(BeginDownload), 2f);
        }

        public void BeginDownload()
        {
            VRCStringDownloader.LoadUrl(NetworkDataURL, (IUdonEventReceiver)this);
        }

        public override void OnStringLoadError(IVRCStringDownload result)
        {
            rawJson = LocalBackup.text;
            LoadData(LocalBackup.text);
        }

        public override void OnStringLoadSuccess(IVRCStringDownload result)
        {
            rawJson = result.Result;
            LoadData(result.Result);
        }

        public void LoadData(string json)
        {
            if (VRCJson.TryDeserializeFromJson(json, out DataToken jsonFile))
            {
                Debug.Log("<color=green>[SUCCESS]</color> Network Debugger loaded json file! Loading latest network data!");
                Debugger.LoadUpdatedData(jsonFile.DataDictionary);
            }
            else if(rawJson != LocalBackup.text)
            {
                Debug.LogError("<color=orange>[ERROR]</color> Network Debugger failed to load json file! Loading backup!");
                rawJson = LocalBackup.text;
                SendCustomEventDelayedFrames(nameof(ReloadData), 1);
                return;
            }
            else
            {
                Debug.LogError("<color=red>[CRITCAL ERROR]</color> Network Debugger failed to load json backup! Shutting down!");
                Debugger.gameObject.SetActive(false);
                return;
            }
        }

        public void ReloadData()
        {
            if (rawJson == null) return;
            LoadData(rawJson);
        }

        public DataDictionary OnBuildLoadJSON()
        {
            if (VRCJson.TryDeserializeFromJson(LocalBackup.text, out DataToken jsonFile))
            {
                return jsonFile.DataDictionary;
            }
            else
            {
                return null;
            }
        }
    }
}
