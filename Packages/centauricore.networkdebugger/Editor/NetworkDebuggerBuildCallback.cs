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
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using JetBrains.Annotations;
using UdonSharp;
using UnityEditor;
using VRC.SDK3.Data;
using VRC.Udon.Editor.ProgramSources;
using VRC.Udon.Editor.ProgramSources.UdonGraphProgram;

namespace Centauri.NetDebug
{
    public class NetworkDebuggerBuildCallback : MonoBehaviour
    {
        [PostProcessScene(-99)]
        public static void OnPostProcessScene()
        {
            #region Creating listeners

            NetworkDebugger debugger = FindObjectOfType<NetworkDebugger>();
            if (!debugger) return;

            debugger.NetworkedObjects = new NetDebugAttachment[0];

            var allBehaviours = FindObjectsOfType<UdonBehaviour>();
            HashSet<GameObject> uniqueGameObjects = new HashSet<GameObject>();
            List<UdonBehaviour> uniqueScripts = new List<UdonBehaviour>();

            foreach (var script in allBehaviours)
            {
                if (script == UdonSharpEditorUtility.GetBackingUdonBehaviour(debugger)) continue;
                
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

                    var behaviour = UdonSharpEditorUtility.GetBackingUdonBehaviour(newBehaviour);
                    behaviour.SyncMethod = behaviours[i].SyncMethod;

                    newBehaviour.Debugger = debugger;
                    debug = newBehaviour;
                }
                else // Else just register existing debug script
                {
                    debug.debugIndex = counter;
                    counter++;

                    debugger.NetworkedObjects = debugger.NetworkedObjects.Add(debug);
                    
                    var behaviour = UdonSharpEditorUtility.GetBackingUdonBehaviour(debug);
                    behaviour.SyncMethod = behaviours[i].SyncMethod;

                    debug.Debugger = debugger;
                }

                var newItemUI = Instantiate(debugger.NetworkItemUIPrefab, debugger.NetworkItemDisplayRoot).GetComponent<NetworkItemUI>();
                newItemUI.NetworkDebugger = debugger;
                debugger.ItemUIs = debugger.ItemUIs.Add(newItemUI);
                newItemUI.objectName = behaviours[i].gameObject.name;
                
                newItemUI.debugIndex = debug.debugIndex;
                
                #region Fetching all synced variables
                
                var rawData = behaviours[i].programSource;
                var dataSnippet = String.Empty;

                if (TryCast(rawData, out UdonGraphProgramAsset graphData)) // Use reflection to get assembly on udon graph behaviours
                {
                    if(TryCast(rawData, out UdonAssemblyProgramAsset assemblyAsset))
                    {
                        UdonAssemblyProgramAsset instance = assemblyAsset;

                        Type type = instance.GetType();

                        FieldInfo fieldInfo = type.GetField("udonAssembly", BindingFlags.NonPublic | BindingFlags.Instance);

                        if (fieldInfo != null)
                        {
                            dataSnippet = GetSubstringBetween((string)fieldInfo.GetValue(instance), ".data_start", ".data_end");
                    
                            //Debug.Log(dataSnippet);
                        }
                    }
                }
                else if(TryCast(rawData, out UdonSharpProgramAsset sharpData)) // Just yoink the cached uasm values for U#
                {
                    if (sharpData == null)
                    {
                        Debug.Log("UdonSharp data is null!");
                    }
                    else
                    { 
                        dataSnippet = GetSubstringBetween(GetUASMStr(sharpData), ".data_start", ".data_end");
                    
                        //Debug.Log(dataSnippet);
                    }
                }

                if (dataSnippet == String.Empty)
                {
                    Debug.Log($"Synced data not found on {behaviours[i].name}!");
                    continue;
                }

                DataDictionary rootDictionary = debugger.DataFetcher.OnBuildLoadJSON();
                
                int minBytes = 0;
                int maxBytes = 0;
                debug.allDataTypes = new string[0];
                bool containsArrOrString = false;
                
                foreach (var syncString in ExtractSyncSubstrings(dataSnippet))
                {
                    string DataValue = ExtractFirstSubstringAfterKeyword(dataSnippet, $"{syncString}: ");
                    //DataBytes byteData = DataByteSize.FetchData($"{DataValue},");
                    DataDictionary dataDictionary = rootDictionary[$"{DataValue},"].DataDictionary;

                    var minByte = (int)dataDictionary["minByte"].Double;
                    var maxByte = (int)dataDictionary["maxByte"].Double;
                    var variableType = dataDictionary["variableType"].String;

                    if (variableType != "None")
                    {
                        minBytes += minByte;
                        maxBytes += maxByte;

                        debug.allDataTypes = debug.allDataTypes.Add($"{DataValue},");
                    }

                    if (variableType == "String" || variableType == "VRCUrl" || variableType.Contains("Array"))
                    {
                        containsArrOrString = true;
                    }
                }

                newItemUI.minBytes = minBytes;
                newItemUI.maxBytes = maxBytes;
                newItemUI.hasArrsOrStr = containsArrOrString;
                
                #endregion

                debug.MaxHeaders = maxBytes - minBytes;
                debug.hasArrsOrStrings = containsArrOrString;
            }

            debugger.perBehaviourByteCount = new int[debugger.NetworkedObjects.Length];
            debugger.perBehaviourByteCountTotal = new int[debugger.NetworkedObjects.Length];
            debugger.perBehaviourByteCountNoHeaders = new int[debugger.NetworkedObjects.Length];
            debugger.perBehaviourByteCountHeadersOnly = new int[debugger.NetworkedObjects.Length];
            debugger.perBehaviourByteCountMinHeaders = new int[debugger.NetworkedObjects.Length];
            debugger.perBehaviourByteCountMaxHeaders = new int[debugger.NetworkedObjects.Length];
            

            #endregion
        }

        #region Helpers
        
        public static List<string> ExtractSyncSubstrings(string input)
        {
            List<string> results = new List<string>();
            string pattern = @"\.sync\s+(\w+),";

            MatchCollection matches = Regex.Matches(input, pattern);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    // Adding the captured group which is the substring after ".sync "
                    results.Add(match.Groups[1].Value);
                }
            }

            return results;
        }
        
        public static string ExtractFirstSubstringAfterKeyword(string text, string keyword)
        {
            int keywordLength = keyword.Length;
            int start = text.IndexOf(keyword);

            if (start != -1)
            {
                // Move the start position to the end of the keyword
                start += keywordLength;

                // Find the next comma after the keyword
                int end = text.IndexOf(',', start);
                if (end == -1) end = text.Length; // If no comma is found, go to the end of the text

                // Extract the substring from the end of the keyword to the next comma
                return text.Substring(start, end - start).Trim();
            }

            return null; // Return null if the keyword is not found
        }

        public static bool TryCast<T>(object obj, out T result) where T : class
        {
            result = obj as T;
            return result != null;
        }
        
        public static string GetSubstringBetween(string source, string startString, string endString)
        {
            if (source.Contains(startString) && source.Contains(endString))
            {
                int startIndex = source.IndexOf(startString) + startString.Length;
                int endIndex = source.IndexOf(endString, startIndex);

                if (startIndex < endIndex)
                {
                    return source.Substring(startIndex, endIndex - startIndex);
                }
            }
            return "";
        }
        
        private string[] ParseVariableData(string content)
        {
            var matches = Regex.Matches(content, @"\.sync (\w+),");
            List<string> VarData = new List<string>();

            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    //SyncNames.Add(match.Groups[1].Value);
                    int dataStartIndex = content.IndexOf(match.Groups[1].Value + ":", match.Index) + match.Groups[1].Value.Length + 1;
                    int dataEndIndex = content.IndexOf(',', dataStartIndex);
                    string data = content.Substring(dataStartIndex, dataEndIndex - dataStartIndex).Trim();
                    VarData.Add(data);
                }
            }

            return VarData.ToArray();
        }

        #endregion

        #region Copied from U# code

        const string UASM_DIR_PATH = "Library/UdonSharpCache/UASM/";
        /// <summary>
        /// Gets the uasm string for the last build of the given program asset
        /// </summary>
        /// <param name="programAsset"></param>
        /// <returns></returns>
        [PublicAPI]
        public static string GetUASMStr(UdonSharpProgramAsset programAsset)
        {
            if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(programAsset, out string guid, out long _))
                return "";

            string uasm;

            string filePath = $"{UASM_DIR_PATH}{guid}.uasm";
            if (!File.Exists(filePath))
                return "";
                
            uasm = ReadFileTextSync(filePath);

            return uasm;
        }
        
        public static string ReadFileTextSync(string filePath, float timeoutSeconds = 2f)
        {
            bool sourceLoaded = false;

            string fileText = "";

            DateTime startTime = DateTime.Now;

            while (true)
            {
                IOException exception = null;

                try
                {
                    fileText = File.ReadAllText(filePath);
                    sourceLoaded = true;
                }
                catch (IOException e)
                {
                    exception = e;

                    if (e is FileNotFoundException ||
                        e is DirectoryNotFoundException)
                        throw;
                }

                if (sourceLoaded)
                    break;
                
                Thread.Sleep(20);

                TimeSpan timeFromStart = DateTime.Now - startTime;

                if (timeFromStart.TotalSeconds > timeoutSeconds)
                {
                    Debug.LogError($"Timeout when attempting to read file {filePath}");
                    if (exception != null)
                        throw exception;
                }
            }

            return fileText;
        }

        #endregion
    }
}
