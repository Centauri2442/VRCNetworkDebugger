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
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class NetworkDebugger : UdonSharpBehaviour
    {
        [Tooltip("Amount of data the network will process per frame! This can be turned up at the cost of CPU performance!")]
        public int iterationsPerFrame = 1;
        public float OcclusionDistance = 10f;
        [Space] 
        public DataFetcher DataFetcher;
        [HideInInspector] public NetDebugAttachment[] NetworkedObjects;
        [HideInInspector] public int[] perBehaviourByteCount;
        [HideInInspector] public int[] perBehaviourByteCountNoHeaders;
        [HideInInspector] public int[] perBehaviourByteCountHeadersOnly;
        [HideInInspector] public int[] perBehaviourByteCountMinHeaders;
        [HideInInspector] public int[] perBehaviourByteCountMaxHeaders;
        [HideInInspector] public int[] perBehaviourByteCountTotal;
        private int bytesLastSecond;
        private int bytesLastSecondNoHeaders;
        private int bytesLastSecondHeadersOnly;
        private int bytesLastSecondMinHeaders;
        private int bytesLastSecondMaxHeaders;

        public GameObject LeftWingVisuals;
        public GameObject RightWingVisuals;

        public TextMeshProUGUI MasterText;

        public TextMeshProUGUI KBytesOut;
        public TextMeshProUGUI KBytesOutNoHeaders;
        public TextMeshProUGUI KBytesOutHeadersOnly;
        public TextMeshProUGUI KBytesOutMin;
        public TextMeshProUGUI KBytesOutMax;
        public TextMeshProUGUI NetworkClogged;
        public TextMeshProUGUI SerialsPerSecText;
        private int serialsPerSec;
        
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

        public BoxCollider CanvasCollider;
        public RectTransform UICanvas;


        #region Panel Refreshing

        private void Start()
        {
            SendCustomEventDelayedFrames(nameof(InitializeDictionaries), 25);
            
            leftWing.localPosition = Vector3.zero;
            rightWing.localPosition = Vector3.zero;

            for (int i = 0; i < NetworkedObjects.Length; i++)
            {
                NetworkedObjects[i].InitializeScript(); // We'll manually initialize the scripts, cause for some reason they don't fire builtin events like Start!
            }

            _localPlayer = Networking.LocalPlayer;

            if (NetworkedObjects.Length > 400)
            {
                iterationsPerFrame = 1;
            }
            else if (NetworkedObjects.Length > 200)
            {
                iterationsPerFrame = 2;
            }
            else if (NetworkedObjects.Length > 100)
            {
                iterationsPerFrame = 4;
            }
            else if (NetworkedObjects.Length > 50)
            {
                iterationsPerFrame = 8;
            }
            else
            {
                iterationsPerFrame = 16;
            }
        }
        
        public void Refresh()
        {
            SendCustomEventDelayedSeconds(nameof(Refresh), 1f, EventTiming.LateUpdate);
            
            bytesLastSecond = 0;
            
            for (var i = 0; i < perBehaviourByteCount.Length; i++)
            {
                bytesLastSecond += perBehaviourByteCount[i];
            }
            bytesLastSecondHeadersOnly = 0;
            bytesLastSecondNoHeaders = 0;
            bytesLastSecondMinHeaders = 0;
            bytesLastSecondMaxHeaders = 0;

            if (leftWingOpen || rightWingOpen)
            {
                SerialsPerSecText.text = serialsPerSec.ToString();
                
                for (var i = 0; i < perBehaviourByteCountNoHeaders.Length; i++)
                {
                    bytesLastSecondNoHeaders += perBehaviourByteCountNoHeaders[i];
                    bytesLastSecondHeadersOnly += perBehaviourByteCountHeadersOnly[i];

                    bytesLastSecondMinHeaders += perBehaviourByteCountMinHeaders[i];
                    bytesLastSecondMaxHeaders += perBehaviourByteCountMaxHeaders[i];

                    ItemUIs[i].headersLastSecond = perBehaviourByteCountHeadersOnly[i];
                }
            }
            
            serialsPerSec = 0;
            
            UpdateDictionaries();
            ResetArrays();

            if (Vector3.Distance(_localPlayer.GetPosition(), transform.position) > OcclusionDistance)
            {
                NetworkItemDisplayRoot.gameObject.SetActive(false);
                LeftWingVisuals.SetActive(false);
                RightWingVisuals.SetActive(false);
                return;
            }
            else
            {
                NetworkItemDisplayRoot.gameObject.SetActive(true);
                LeftWingVisuals.SetActive(leftWingOpen);
                RightWingVisuals.SetActive(rightWingOpen);
            }

            if (!Networking.IsOwner(gameObject))
            {
                if (_localPlayer.isMaster)
                {
                    Networking.SetOwner(_localPlayer, gameObject);
                }
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

            for (var i = 0; i < perBehaviourByteCountNoHeaders.Length; i++)
            {
                perBehaviourByteCountNoHeaders[i] = 0;
                perBehaviourByteCountHeadersOnly[i] = 0;
                perBehaviourByteCountMaxHeaders[i] = 0;
                perBehaviourByteCountMinHeaders[i] = 0;
            }
        }
        public void UpdateUI()
        {
            KBytesOut.text = BytesToKilobytes(bytesLastSecond).ToString();

            if (leftWingOpen)
            {
                KBytesOutNoHeaders.text = BytesToKilobytes(bytesLastSecondNoHeaders).ToString();
                KBytesOutHeadersOnly.text = BytesToKilobytes(bytesLastSecondHeadersOnly).ToString();
                KBytesOutMin.text = BytesToKilobytes(bytesLastSecondMinHeaders).ToString();
                KBytesOutMax.text = BytesToKilobytes(bytesLastSecondMaxHeaders).ToString();
            }
            
            NetworkClogged.text = Networking.IsClogged.ToString();

            for (var i = 0; i < sortedDictionaries.Length; i++)
            {
                if (sortedDictionaries[i].TryGetValue("ui", out DataToken ui))
                {
                    tempItemUI = ((NetworkItemUI)ui.Reference);
                    tempItemUI.transform.SetSiblingIndex(i);
                    tempItemUI.IsVisibleInScrollView(ScrollViewRect);
                    
                    if (tempItemUI.isVisible)
                    {
                        if (!ShowTotal)
                        {
                            if (sortedDictionaries[i].TryGetValue("byte", out DataToken value))
                            {
                                tempItemUI.UpdateBytesOut(value.Int);
                                tempItemUI.UpdateOwner(true);
                            }
                        }
                        else
                        {
                            if (sortedDictionaries[i].TryGetValue("totalByte", out DataToken value))
                            {
                                tempItemUI.UpdateTotalBytes(value.Int);
                                tempItemUI.UpdateOwner(true);
                            }
                        }
                    }
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

        private DataDictionary tempDictionary;
        private NetDebugAttachment tempDebugAttach;
        private NetworkItemUI tempItemUI;
        public void UpdateNetworkItem(int i)
        {
            if (DataDictionaries.TryGetValue(i, out DataToken token))
            {
                tempDictionary = token.DataDictionary;
                
                if (tempDictionary.TryGetValue("ntwrkItm", out DataToken item))
                {
                    tempDebugAttach = ((NetDebugAttachment)item.Reference);
                    tempItemUI = (NetworkItemUI)tempDictionary["ui"].Reference;
                    
                    if (Networking.IsOwner(tempDebugAttach.gameObject))
                    {
                        if (!ShowAllItems)
                        {
                            if (tempDictionary.TryGetValue("byte", out DataToken bytes))
                            {
                                tempItemUI.gameObject.SetActive((int)bytes > 0 && tempItemUI.searchVisible);
                            }
                        }
                        else
                        {
                            tempItemUI.gameObject.SetActive(tempItemUI.searchVisible);
                        }
                    }
                    else
                    {
                        if (tempItemUI.isVisible)
                        {
                            if (!ShowRemote)
                            {
                                tempItemUI.gameObject.SetActive(false);
                            }
                            else
                            {
                                tempItemUI.gameObject.SetActive(tempItemUI.searchVisible);
                                tempItemUI.UpdateOwner(false);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Data Loading

        
        public void LoadUpdatedData(DataDictionary newData)
        {
            int tempMin = 0;
            int tempMax = 0;
            
            for (var i = 0; i < NetworkedObjects.Length; i++)
            {
                tempMin = 0;
                tempMax = 0;
                
                if (NetworkedObjects[i].allDataTypes.Length > 0)
                {
                    for (int j = 0; j < NetworkedObjects[i].allDataTypes.Length; j++)
                    {
                        DataDictionary dataDictionary = newData[NetworkedObjects[i].allDataTypes[j]].DataDictionary;
                        tempMin += (int)dataDictionary["minByte"].Double;
                        tempMax += (int)dataDictionary["maxByte"].Double;
                    }
                    
                    ItemUIs[i].minBytes = tempMin;
                    ItemUIs[i].maxBytes = tempMax;
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
                debugItem["noVRC"] = perBehaviourByteCountNoHeaders[i];
                debugItem["VRCOnly"] = perBehaviourByteCountHeadersOnly[i];

                DataDictionaries[NetworkedObjects[i].debugIndex] = debugItem;

                sortedDictionaries = sortedDictionaries.Add(debugItem);
            }
            
            SendCustomEventDelayedSeconds(nameof(Refresh), 1f, EventTiming.LateUpdate); // We make sure the dictionaries are created before beginning the panel refresh!
            SendCustomEventDelayedSeconds(nameof(DetailPanelRefresh), 1f, EventTiming.LateUpdate);
        }

        private DataDictionary[] inactiveDictionaries = new DataDictionary[0];
        public void UpdateDictionaries()
        {
            sortedDictionaries = new DataDictionary[0];
            inactiveDictionaries = new DataDictionary[0];
            
            for (int i = 0; i < NetworkedObjects.Length; i++)
            {
                if(DataDictionaries.TryGetValue(i, out DataToken dictionary))
                {
                    dictionary.DataDictionary["byte"] = perBehaviourByteCount[i];
                    dictionary.DataDictionary["totalByte"] = perBehaviourByteCountTotal[i];
                    dictionary.DataDictionary["noVRC"] = perBehaviourByteCountNoHeaders[i];
                    dictionary.DataDictionary["VRCOnly"] = perBehaviourByteCountHeadersOnly[i];

                    if (perBehaviourByteCount[i] > 0)
                    {
                        sortedDictionaries = sortedDictionaries.Add(dictionary.DataDictionary);
                    }
                    else
                    {
                        inactiveDictionaries = inactiveDictionaries.Add(dictionary.DataDictionary);
                    }
                }
            }
        }
        
        public void SortDictionaries()
        {
            dictLength = sortedDictionaries.Length;
            tempDict = new DataDictionary();
            
            for (int j = 0; j < dictLength; j++)
            {
                for (int i = 0; i < dictLength - j - 1; i++)
                {
                    if (sortedDictionaries[i]["byte"].Int < sortedDictionaries[i + 1]["byte"].Int)
                    {
                        tempDict = sortedDictionaries[i + 1];
                        sortedDictionaries[i + 1] = sortedDictionaries[i];
                        sortedDictionaries[i] = tempDict;
                    }
                }
            }

            DataDictionary[] tempArr = new DataDictionary[sortedDictionaries.Length + inactiveDictionaries.Length];
            
            for (int i = 0; i < tempArr.Length; i++)
            {
                if (i < sortedDictionaries.Length)
                {
                    tempArr[i] = sortedDictionaries[i];
                }
                else
                {
                    tempArr[i] = inactiveDictionaries[i - sortedDictionaries.Length];
                }
            }

            sortedDictionaries = tempArr;
            
            for (var i = 0; i < sortedDictionaries.Length; i++)
            {
                if (sortedDictionaries[i].TryGetValue("ui", out DataToken ui))
                {
                    ((NetworkItemUI)ui.Reference).transform.SetSiblingIndex(i);
                }
            }
            
            SendCustomEventDelayedFrames(nameof(UpdateUI), 1, EventTiming.LateUpdate);
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

        public void BeginScroll()
        {
            isScrolling = true;
            
            Scrolling();
        }

        private bool isScrolling;

        public void Scrolling()
        {
            if (isScrolling)
            {
                SendCustomEventDelayedFrames(nameof(Scrolling), 10);

                for (int i = 0; i < ItemUIs.Length; i++)
                {
                    ItemUIs[i].IsVisibleInScrollView(ScrollViewRect);
                }
            }
        }

        public void EndScroll()
        {
            isScrolling = false;
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

        private int headers;
        public void AddBytes(int index, int bytes)
        {
            headers = 16;
            
            perBehaviourByteCount[index] += bytes;
            perBehaviourByteCountTotal[index] += bytes;
            
            bytes -= 16;

            perBehaviourByteCountNoHeaders[index] += ItemUIs[index].minBytes;

            perBehaviourByteCountMaxHeaders[index] += headers + ItemUIs[index].maxBytes;
            perBehaviourByteCountMinHeaders[index] += headers + ItemUIs[index].minBytes;

            perBehaviourByteCountHeadersOnly[index] += headers + (bytes - ItemUIs[index].minBytes);

            /*
            if (targetNetworkItem)
            {
                if (index == targetNetworkItem.debugIndex)
                {
                    Debug.Log($"Adding {bytes + headers} bytes to target behaviour. Total header is 16 + {(bytes - ItemUIs[index].minBytes)}, totaling {headers + ItemUIs[index].minBytes}");
                }
            }*/
                
            serialsPerSec++;
            
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

        //private int serializationCount;
        //private string serializationList;
        
        public void AddSerializationEntry(string entry)
        {
            if (lastSerializations.Length >= 20)
            {
                lastSerializations = lastSerializations.RemoveAt(0); // Remove the oldest entry
            }

            //serializationList = string.Concat(serializationCount >= 20 ? serializationList.Substring(serializationList.IndexOf('\n') + 1) : serializationList, "\n", entry);

            lastSerializations = lastSerializations.Add(entry); // Add the new entry
            
            UpdateSerializationDisplay(); // Update the display
        }

        // Updates the TextMeshProUGUI with the log entries
        private void UpdateSerializationDisplay()
        {
            //lastSerializationsText.text = serializationList;
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

        #region Wing Menus

        public void WingCanvasScaling()
        {
            if (leftWingOpen || rightWingOpen)
            {
                CanvasCollider.size = new Vector3(2050f, CanvasCollider.size.y, CanvasCollider.size.z);
                
                UICanvas.sizeDelta = new Vector2(2050f, UICanvas.sizeDelta.y);
            }
            else
            {
                CanvasCollider.size = new Vector3(1050f, CanvasCollider.size.y, CanvasCollider.size.z);

                UICanvas.sizeDelta = new Vector2(1050f, UICanvas.sizeDelta.y);
            }
        }

        private bool isMovingLeftWing;
        private bool leftWingOpen;
        [SerializeField] private RectTransform leftWing;
        private float leftWingMaxVal = -500;
        
        public void ToggleLeftWing()
        {
            if (isMovingLeftWing) return;
            isMovingLeftWing = true;
            
            leftWingOpen = !leftWingOpen;
            
            LeftWingVisuals.SetActive(leftWingOpen);

            WingCanvasScaling();
            
            LeftWingTween();
        }

        public void LeftWingTween()
        {
            if (leftWingOpen)
            {
                if (Math.Abs(leftWing.localPosition.x - leftWingMaxVal) > 0.1f)
                {
                    leftWing.localPosition = new Vector3(Mathf.Lerp(leftWing.localPosition.x, leftWingMaxVal, Time.deltaTime * 12f), 0f, 0f);
                    
                    SendCustomEventDelayedFrames(nameof(LeftWingTween), 1);
                    return;
                }

                isMovingLeftWing = false;
            }
            else
            {
                if (Math.Abs(leftWing.localPosition.x) > 0.1f)
                {
                    leftWing.localPosition = new Vector3(Mathf.Lerp(leftWing.localPosition.x, 0, Time.deltaTime * 12f), 0f, 0f);
                    
                    SendCustomEventDelayedFrames(nameof(LeftWingTween), 1);
                    return;
                }

                isMovingLeftWing = false;
            }
        }

        private bool isMovingRightWing;
        private bool rightWingOpen;
        [SerializeField] private RectTransform rightWing;
        private float rightWingMaxVal = 500;
        
        public void ToggleRightWing()
        {
            if (isMovingRightWing) return;
            isMovingRightWing = true;
            
            rightWingOpen = !rightWingOpen;
            
            RightWingVisuals.SetActive(rightWingOpen);
            
            WingCanvasScaling();
            
            RightWingTween();
        }

        public void RightWingTween()
        {
            if (rightWingOpen)
            {
                if (Math.Abs(rightWing.localPosition.x - rightWingMaxVal) > 0.1f)
                {
                    rightWing.localPosition = new Vector3(Mathf.Lerp(rightWing.localPosition.x, rightWingMaxVal, Time.deltaTime * 12f), 0f, 0f);
                    
                    SendCustomEventDelayedFrames(nameof(RightWingTween), 1);
                    return;
                }

                isMovingRightWing = false;
            }
            else
            {
                if (Math.Abs(rightWing.localPosition.x) > 0.1f)
                {
                    rightWing.localPosition = new Vector3(Mathf.Lerp(rightWing.localPosition.x, 0, Time.deltaTime * 12f), 0f, 0f);
                    
                    SendCustomEventDelayedFrames(nameof(RightWingTween), 1);
                    return;
                }

                isMovingRightWing = false;
            }
        }

        #endregion

        #region Panel Details

        [Space(15)] 
        
        public TextMeshProUGUI NetworkItemName;
        public TextMeshProUGUI NetworkItemTotalBytes;
        public TextMeshProUGUI NetworkItemBPS;
        public TextMeshProUGUI NetworkItemMinBytes;
        public TextMeshProUGUI NetworkItemMaxBytes;
        public TextMeshProUGUI NetworkItemContainsArrsOrStrings;
        public TextMeshProUGUI NetworkItemTimeSinceLast;
        public TextMeshProUGUI NetworkItemTimeUdonHeaders;

        private NetworkItemUI targetNetworkItem;
        public void SetDetailPanel(NetworkItemUI relevantUI)
        {
            if (!rightWingOpen)
            {
                ToggleRightWing();
            }

            targetNetworkItem = relevantUI;
        }

        public void DetailPanelRefresh()
        {
            if (!targetNetworkItem)
            {
                SendCustomEventDelayedSeconds(nameof(DetailPanelRefresh), 1f);
                return;
            }

            if (rightWingOpen)
            {
                NetworkItemName.text = targetNetworkItem.objectName;
            
                NetworkItemTotalBytes.text = perBehaviourByteCountTotal[targetNetworkItem.debugIndex].ToString();
            
                NetworkItemBPS.text = targetNetworkItem.BytesOut.text;
                NetworkItemMinBytes.text = targetNetworkItem.minBytes.ToString();
                NetworkItemMaxBytes.text = targetNetworkItem.maxBytes.ToString();
                NetworkItemContainsArrsOrStrings.text = targetNetworkItem.hasArrsOrStr.ToString();
                NetworkItemTimeSinceLast.text = targetNetworkItem.timeSinceSync.ToString();

                if (targetNetworkItem.hasArrsOrStr)
                {
                    NetworkItemTimeUdonHeaders.text = "Unknown";
                }
                else
                {
                    NetworkItemTimeUdonHeaders.text = ItemUIs[targetNetworkItem.debugIndex].headersLastSecond.ToString();
                }
            }
            
            SendCustomEventDelayedFrames(nameof(DetailPanelRefresh), 2);
        }

        #endregion
    }
}
