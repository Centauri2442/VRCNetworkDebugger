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
using Centauri.Utilities;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Enums;

namespace Centauri.NetDebug
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class NetworkDebugger : UdonSharpBehaviour
    {
        [Tooltip("Amount of data the network will process per frame! This can be turned up at the cost of CPU performance!")]
        public int iterationsPerFrame = 1;
        public float OcclusionDistance = 10f;
        [Space]
        [HideInInspector] public NetDebugAttachment[] NetworkedObjects;
        [HideInInspector] public int[] perBehaviourByteCount;
        [HideInInspector] public int[] perBehaviourByteCountTotal;
        private int bytesLastSecond;

        public TextMeshProUGUI MasterText;

        public TextMeshProUGUI KBytesOut;
        public TextMeshProUGUI NetworkClogged;
        
        public TextMeshProUGUI lastSerializationsText;
        private string[] lastSerializations = new string[20];

        public Toggle ShowAllItemsToggle;
        public Toggle ShowRemoteToggle;
        public Toggle ShowTotalToggle;
        [HideInInspector] public NetworkItemUI[] ItemUIs;
        public GameObject NetworkItemUIPrefab;
        public Transform NetworkItemDisplayRoot;
        public RectTransform ScrollViewRect;

        [HideInInspector] public bool ShowAllItems = false;
        [HideInInspector] public bool ShowRemote = true;
        [HideInInspector] public bool ShowTotal = false;
        private DataDictionary DataDictionaries = new DataDictionary();
        private DataDictionary[] sortedDictionaries = new DataDictionary[0];

        public InputField SearchBar;
        [HideInInspector] public string searchVal;

        private VRCPlayerApi _localPlayer;
        private bool isUpdatingUI;
        
        private int dictLength = 0;
        private DataDictionary tempDict;
        private int currentIndex = 0;


        #region Panel Refreshing

        private void Start()
        {
            SendCustomEventDelayedFrames(nameof(InitializeDictionaries), 25);

            for (int i = 0; i < NetworkedObjects.Length; i++)
            {
                NetworkedObjects[i].InitializeScript(); // We'll manually initialize the scripts, cause for some reason they don't fire builtin events like Start!
            }

            _localPlayer = Networking.GetOwner(gameObject);

            if (NetworkedObjects.Length > 100)
            {
                iterationsPerFrame = 1;
            }
            else if (NetworkedObjects.Length > 50)
            {
                iterationsPerFrame = 2;
            }
            else
            {
                iterationsPerFrame = 4;
            }
        }

        public void Refresh()
        {
            SendCustomEventDelayedSeconds(nameof(Refresh), 1f, EventTiming.LateUpdate);
            
            bytesLastSecond = 0;
            
            for (var i = 0; i < perBehaviourByteCount.Length; i++)
            {
                bytesLastSecond += perBehaviourByteCount[i];
                perBehaviourByteCountTotal[i] += perBehaviourByteCount[i];
            }

            if (!Networking.IsOwner(gameObject)) // Ensures object is always owned by the master!
            {
                if (_localPlayer.isMaster)
                {
                    Networking.SetOwner(_localPlayer, gameObject);
                }
            }
            
            UpdateDictionaries();
            ResetArrays();

            if (Vector3.Distance(_localPlayer.GetPosition(), transform.position) > OcclusionDistance)
            {
                NetworkItemDisplayRoot.gameObject.SetActive(false);
                return;
            }
            else
            {
                NetworkItemDisplayRoot.gameObject.SetActive(true);
            }
            
            MasterText.text = Networking.GetOwner(gameObject).displayName;

            if (isUpdatingUI) return;
            isUpdatingUI = true;
            
            SendCustomEventDelayedFrames(nameof(SortDictionaries), 1, EventTiming.LateUpdate);
        }
        
        public void ResetArrays()
        {
            for (var i = 0; i < perBehaviourByteCount.Length; i++)
            {
                perBehaviourByteCount[i] = 0;
            }
        }
        public void UpdateUI()
        {
            KBytesOut.text = BytesToKilobytes(bytesLastSecond).ToString();
            NetworkClogged.text = Networking.IsClogged.ToString();

            for (var i = 0; i < sortedDictionaries.Length; i++)
            {
                if (sortedDictionaries[i].TryGetValue("ui", out DataToken ui))
                {
                    ((NetworkItemUI)ui.Reference).transform.SetSiblingIndex(i);
                }
            }

            currentIndex = 0;
            dictLength = NetworkedObjects.Length;
            BufferedUpdateUI();
        }

        public void BufferedUpdateUI()
        {
            for (int iter = 0; iter < iterationsPerFrame; iter++)
            {
                if (currentIndex <= dictLength)
                {
                    for (int i = 0; i < dictLength - currentIndex; i++)
                    {
                        UpdateNetworkItem(i);
                    }

                    currentIndex++;
                }
                else
                {
                    isUpdatingUI = false;
                    return;
                }
            }
            
            SendCustomEventDelayedFrames(nameof(BufferedUpdateUI), 1, EventTiming.LateUpdate);
        }

        public void UpdateNetworkItem(int i)
        {
            if (DataDictionaries.TryGetValue(i, out DataToken token))
            {
                var dictionary = token.DataDictionary;
                    
                if (dictionary.TryGetValue("ntwrkItm", out DataToken item))
                {
                    if (Networking.IsOwner(((NetDebugAttachment)item.Reference).gameObject))
                    {
                        if (!ShowAllItems)
                        {
                            if (dictionary.TryGetValue("ui", out DataToken ui) && dictionary.TryGetValue("byte", out DataToken bytes))
                            {
                                ((NetworkItemUI)ui.Reference).gameObject.SetActive((int)bytes > 0 && ((NetworkItemUI)ui.Reference).searchVisible);
                                ((NetworkItemUI)ui.Reference).IsVisibleInScrollView(ScrollViewRect);
                            }
                        }
                        else
                        {
                            if (dictionary.TryGetValue("ui", out DataToken ui))
                            {
                                ((NetworkItemUI)ui.Reference).gameObject.SetActive(((NetworkItemUI)ui.Reference).searchVisible);
                                ((NetworkItemUI)ui.Reference).IsVisibleInScrollView(ScrollViewRect);
                            }
                        }

                        if (dictionary.TryGetValue("ui", out DataToken val))
                        {
                            if (((NetworkItemUI)val.Reference).isVisible)
                            {
                                if (!ShowTotal)
                                {
                                    if (dictionary.TryGetValue("byte", out DataToken value))
                                    {
                                        ItemUIs[i].UpdateBytesOut(value.Int);
                                        ItemUIs[i].UpdateOwner(true);
                                    }
                                }
                                else
                                {
                                    if (dictionary.TryGetValue("totalByte", out DataToken value))
                                    {
                                        ItemUIs[i].UpdateTotalBytes(value.Int);
                                        ItemUIs[i].UpdateOwner(true);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (dictionary.TryGetValue("ui", out DataToken ui))
                        {
                            ((NetworkItemUI)ui.Reference).IsVisibleInScrollView(ScrollViewRect);

                            if (((NetworkItemUI)ui.Reference).isVisible)
                            {
                                if (!ShowRemote)
                                {
                                    ((NetworkItemUI)ui.Reference).gameObject.SetActive(false);
                                }
                                else
                                {
                                    ((NetworkItemUI)ui.Reference).gameObject.SetActive(((NetworkItemUI)ui.Reference).searchVisible);
                                    ((NetworkItemUI)ui.Reference).UpdateOwner(false);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Dictionary Handling
        
        public void InitializeDictionaries()
        {
            for (var i = 0; i < NetworkedObjects.Length; i++)
            {
                DataDictionary debugItem = new DataDictionary();
                debugItem["ntwrkItm"] = NetworkedObjects[i];
                debugItem["ui"] = ItemUIs[i];
                debugItem["byte"] = perBehaviourByteCount[i];
                debugItem["totalByte"] = perBehaviourByteCountTotal[i];

                DataDictionaries[NetworkedObjects[i].debugIndex] = debugItem;

                sortedDictionaries = sortedDictionaries.Add(debugItem);
            }
            
            SendCustomEventDelayedSeconds(nameof(Refresh), 1f, EventTiming.LateUpdate); // We make sure the dictionaries are created before beginning the panel refresh!
        }

        public void UpdateDictionaries()
        {
            for (int i = 0; i < NetworkedObjects.Length; i++)
            {
                if(DataDictionaries.TryGetValue(i, out DataToken dictionary))
                {
                    dictionary.DataDictionary["byte"] = perBehaviourByteCount[i];
                    dictionary.DataDictionary["totalByte"] = perBehaviourByteCountTotal[i];
                }
            }
        }
        
        public void SortDictionaries()
        {
            dictLength = sortedDictionaries.Length;
            tempDict = new DataDictionary();

            for (int iter = 0; iter < iterationsPerFrame; iter++)
            {
                if (currentIndex < dictLength - 1)
                {
                    for (int i = 0; i < dictLength - currentIndex - 1; i++)
                    {
                        if (sortedDictionaries[i]["byte"].Int < sortedDictionaries[i + 1]["byte"].Int)
                        {
                            // Swap the elements
                            tempDict = sortedDictionaries[i + 1];
                            sortedDictionaries[i + 1] = sortedDictionaries[i];
                            sortedDictionaries[i] = tempDict;
                        }

                        currentIndex++;
                    }
                }
                else
                {
                    currentIndex = 0;
                    SendCustomEventDelayedFrames(nameof(UpdateUI), 1, EventTiming.LateUpdate);
                    return;
                }
            }
            
            SendCustomEventDelayedFrames(nameof(SortDictionaries), 1, EventTiming.LateUpdate);
        }

        #endregion

        #region Externs

        public void SearchUpdate()
        {
            searchVal = SearchBar.text;
            bool isSearching = searchVal.Length > 0;

            for (int i = 0; i < NetworkedObjects.Length; i++)
            {
                if (DataDictionaries.TryGetValue(i, out DataToken token))
                {
                    var dictionary = token.DataDictionary;
                    
                    if (dictionary.TryGetValue("ntwrkItm", out DataToken item))
                    {
                        if (isSearching)
                        {
                            if (dictionary.TryGetValue("ui", out DataToken ui))
                            {
                                ((NetworkItemUI)ui.Reference).searchVisible = ((NetDebugAttachment)item.Reference).gameObject.name.Contains(searchVal); // Set UI active state based off of target behaviour gameObject name
                            }
                        }
                        else
                        {
                            if (dictionary.TryGetValue("ui", out DataToken ui))
                            {
                                ((NetworkItemUI)ui.Reference).searchVisible = true;
                            }
                        }
                    }
                }
            }
        }

        public void ToggleShowAllItems()
        {
            ShowAllItems = ShowAllItemsToggle.isOn;
            
            for (int i = 0; i < ItemUIs.Length; i++)
            {
                if (!ShowAllItems)
                {
                    ItemUIs[i].gameObject.SetActive(perBehaviourByteCount[i] > 0 || !Networking.IsOwner(NetworkedObjects[i].gameObject));
                }
                else
                {
                    ItemUIs[i].gameObject.SetActive(true);
                }
            }
        }

        public void ToggleShowTotalBytes()
        {
            ShowTotal = ShowTotalToggle.isOn;
            
            for (int i = 0; i < ItemUIs.Length; i++)
            {
                ItemUIs[i].ShowTotal(ShowTotal);
            }
        }

        public void ToggleShowRemote()
        {
            ShowRemote = ShowRemoteToggle.isOn;
            
            ToggleShowAllItems();
        }
        
        public void AddBytes(int index, int bytes)
        {
            perBehaviourByteCount[index] += bytes;
            
            AddSerializationEntry($"{NetworkedObjects[index].gameObject.name}: {bytes} bytes");
        }

        public void SerializationFailed(int index)
        {
            if (DataDictionaries[index].DataDictionary.TryGetValue("ui", out DataToken ui))
            {
                ((NetworkItemUI)ui.Reference).DataFailed();
            }
        }

        public void AddResultData(int index, float sendTime, float receiveTime)
        {
            ItemUIs[index].UpdateRemote(sendTime, receiveTime);
        }

        public void UpdateUIOwner(int index, string newOwner)
        {
            ItemUIs[index].ownerName = newOwner;

            if (ItemUIs[index].showOwner) // Reset when owner gets changed
            {
                ItemUIs[index].ToggleShowOwner();
            }
        }

        #endregion

        #region Last Serializations

        public void AddSerializationEntry(string entry)
        {
            if (lastSerializations.Length >= 20)
            {
                lastSerializations = lastSerializations.RemoveAt(0); // Remove the oldest entry
            }

            lastSerializations = lastSerializations.Add(entry); // Add the new entry
            UpdateSerializationDisplay(); // Update the display
        }

        // Updates the TextMeshProUGUI with the log entries
        private void UpdateSerializationDisplay()
        {
            lastSerializationsText.text = String.Join("\n", lastSerializations); // Join all entries with a newline
        }

        #endregion

        #region Helpers

        private float BytesToKilobytes(int bytes)
        {
            float kilobytes = bytes / 1024f;
            return Mathf.Round(kilobytes * 1000f) / 1000f;
        }

        #endregion
    }
}
