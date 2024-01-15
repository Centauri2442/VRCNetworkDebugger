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

using System.Collections;
using System.Collections.Generic;
using Centauri.Utilities;
using UdonSharpEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Centauri.NetDebug
{
    public class NetworkDebuggerBuildCallback : MonoBehaviour
    {
        [PostProcessScene(-99)]
        public static void OnPostProcessScene()
        {
            NetworkDebugger debugger = FindObjectOfType<NetworkDebugger>();
            if (!debugger) return;

            debugger.NetworkedObjects = new NetDebugAttachment[0];

            var allBehaviours = FindObjectsOfType<UdonBehaviour>();
            HashSet<GameObject> uniqueGameObjects = new HashSet<GameObject>();
            List<UdonBehaviour> uniqueScripts = new List<UdonBehaviour>();

            foreach (var script in allBehaviours)
            {
                if (uniqueGameObjects.Add(script.gameObject))
                {
                    // This GameObject was not already in the HashSet, so add the script to the list
                    uniqueScripts.Add(script);
                }
            }

            var behaviours = uniqueScripts.ToArray();
            
            int counter = 0;
            while (debugger.NetworkItemDisplayRoot.childCount > 0 && counter < 100) // Destroy existing children
            {
                DestroyImmediate(debugger.NetworkItemDisplayRoot.GetChild(0));
                counter++;
            }

            debugger.ItemUIs = new NetworkItemUI[0];

            counter = 0;
            for (var i = 0; i < behaviours.Length; i++)
            {
                if (behaviours[i].SyncMethod == Networking.SyncType.None) continue;
                
                if (!behaviours[i].gameObject.TryGetComponent(out NetDebugAttachment debug)) // Add debug attachment if one doesn't already exist
                {
                    NetDebugAttachment newBehaviour = UdonSharpUndo.AddComponent<NetDebugAttachment>(behaviours[i].gameObject);

                    newBehaviour.debugIndex = counter;
                    counter++;

                    debugger.NetworkedObjects = debugger.NetworkedObjects.Add(newBehaviour);

                    UdonSharpEditorUtility.GetBackingUdonBehaviour(newBehaviour).SyncMethod = behaviours[i].SyncMethod;

                    newBehaviour.Debugger = debugger;
                }
                else // Else just register existing debug script
                {
                    debug.debugIndex = counter;
                    counter++;

                    debugger.NetworkedObjects = debugger.NetworkedObjects.Add(debug);
                    
                    UdonSharpEditorUtility.GetBackingUdonBehaviour(debug).SyncMethod = behaviours[i].SyncMethod;

                    debug.Debugger = debugger;
                }

                var newItemUI = Instantiate(debugger.NetworkItemUIPrefab, debugger.NetworkItemDisplayRoot).GetComponent<NetworkItemUI>();
                debugger.ItemUIs = debugger.ItemUIs.Add(newItemUI);
                newItemUI.objectName = behaviours[i].gameObject.name;
            }

            debugger.perBehaviourByteCount = new int[debugger.NetworkedObjects.Length];
            debugger.perBehaviourByteCountTotal = new int[debugger.NetworkedObjects.Length];

            /*
            var sizeDelta = debugger.NetworkItemDisplayRoot.GetComponent<RectTransform>().sizeDelta;
            sizeDelta.y = 55 * debugger.NetworkedObjects.Length;
            debugger.NetworkItemDisplayRoot.GetComponent<RectTransform>().sizeDelta = sizeDelta;*/
        }
    }
}
