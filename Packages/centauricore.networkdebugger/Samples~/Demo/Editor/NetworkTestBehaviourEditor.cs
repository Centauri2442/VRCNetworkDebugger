using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using JetBrains.Annotations;
using UdonSharp;
using UdonSharpEditor;
using UnityEditor;
using UnityEngine;
using VRC.Udon.Editor.ProgramSources;
using VRC.Udon.Editor.ProgramSources.UdonGraphProgram;

namespace Centauri.NetDebug
{
    [CustomEditor(typeof(NetworkTestBehaviour))]
    public class NetworkTestBehaviourEditor : Editor
    {
        private NetworkTestBehaviour script;


        private void OnEnable()
        {
            script = target as NetworkTestBehaviour;
            if (script == null) return;
            
            Undo.undoRedoPerformed += PullFromScript;
            PullFromScript();
        }
        
        private void OnDisable()
        {
            Undo.undoRedoPerformed -= PullFromScript;
        }

        public override void OnInspectorGUI()
        {
            UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target);
            
            RenderGUI();
        }

        public void RenderGUI()
        {
            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Fetch Synced Behaviours"))
            {
                PopulateData();
            }
            
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();

            if (EditorGUI.EndChangeCheck())
            {
                PushToScript();
            }

            base.OnInspectorGUI();
        }

        public void PullFromScript()
        {
            
        }

        public void PopulateData()
        {
            if (script == null)
            {
                Debug.Log("Script is null!");
            }

            //var data = GetUASMStr((UdonSharpProgramAsset)UdonSharpEditorUtility.GetBackingUdonBehaviour(script).programSource);
            var rawData = script.TEMPTARGET.programSource;

            if (TryCast(rawData, out UdonGraphProgramAsset graphData)) // Use reflection to get assembly on udon graph behaviours
            {
                if(TryCast(rawData, out UdonAssemblyProgramAsset assemblyAsset))
                {
                    UdonAssemblyProgramAsset instance = assemblyAsset;

                    Type type = instance.GetType();

                    FieldInfo fieldInfo = type.GetField("udonAssembly", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (fieldInfo != null)
                    {
                        string fieldValue = (string)fieldInfo.GetValue(instance);

                        var values = ParseVariableData(fieldValue);
                        foreach (var value in values)
                        {
                            Debug.Log(value);
                        }
                        
                        
                    }
                }
            }
            
            if(TryCast(rawData, out UdonSharpProgramAsset sharpData)) // Just yoink the cached uasm values for U#
            {
                if (sharpData == null)
                {
                    Debug.Log("UdonSharp data is null!");
                }
                else
                { 
                    Debug.Log(GetUASMStr(sharpData));
                }
            }
        }
        
        public void PushToScript()
        {
            Undo.RecordObject(script, "Fetch synced behaviour data");
            PrefabUtility.RecordPrefabInstancePropertyModifications(script);
        }
        
        public bool TryCast<T>(object obj, out T result) where T : class
        {
            result = obj as T;
            return result != null;
        }
        
        private string[] ParseVariableData(string content)
        {
            var matches = Regex.Matches(content, @"\.sync (\w+),");
            List<string> VarData = new List<string>();
            List<string> VarNames = new List<string>();

            foreach (Match match in matches)
            {
                if (match.Groups.Count > 1)
                {
                    VarNames.Add(match.Groups[1].Value);
                    int dataStartIndex = content.IndexOf(match.Groups[1].Value + ":", match.Index) + match.Groups[1].Value.Length + 1;
                    int dataEndIndex = content.IndexOf(',', dataStartIndex);
                    string data = content.Substring(dataStartIndex, dataEndIndex - dataStartIndex).Trim();
                    VarData.Add(data);
                }
            }
            
            foreach (var value in VarNames)
            {
                Debug.Log(value);
            }

            return VarData.ToArray();
        }

        #region Copied from U# code

        const string UASM_DIR_PATH = "Library/UdonSharpCache/UASM/";
        /// <summary>
        /// Gets the uasm string for the last build of the given program asset
        /// </summary>
        /// <param name="programAsset"></param>
        /// <returns></returns>
        [PublicAPI]
        public string GetUASMStr(UdonSharpProgramAsset programAsset)
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
