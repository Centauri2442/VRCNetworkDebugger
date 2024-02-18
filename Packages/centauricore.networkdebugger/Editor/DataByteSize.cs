using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Centauri.NetDebug
{
    public static class DataByteSize
    {
        public static DataBytes FetchData(string key)
        {
            if (DataBytes.byteDictionary.TryGetValue(key, out DataBytes value))
            {
                return value;
            }
            else
            {
                return new DataBytes(0, 0, "None");
            }
        }
    }
    
    
    public struct DataBytes
    {
        /// <summary>
        /// Cost of a synced variable, data will be one of the 2 values, too many edge cases to figure out certain values automatically | base 16 per serialization + data cost
        /// </summary>
        public static Dictionary<string, DataBytes> byteDictionary = new Dictionary<string, DataBytes>()
        {
            { "%SystemBoolean,", new DataBytes(2, 2, "Bool") },
            { "%SystemChar,", new DataBytes(2, 2, "Char") },
            { "%SystemByte,", new DataBytes(2, 2, "Byte") },
            { "%SystemSByte,", new DataBytes(2, 2, "UByte") },
            { "%SystemInt16,", new DataBytes(4, 2, "Short") }, // Swaps every other
            { "%SystemUInt16,", new DataBytes(4, 2, "UShort") }, // ^
            { "%SystemInt32,", new DataBytes(8, 4, "Int") },
            { "%SystemUInt32,", new DataBytes(8, 4, "UInt") },
            { "%SystemInt64,", new DataBytes(12, 8, "Long") },
            { "%SystemUInt64,", new DataBytes(12, 8, "ULong") },
            { "%SystemSingle,", new DataBytes(8, 4, "Float") },
            { "%SystemDouble,", new DataBytes(12, 8, "Double") },
            { "%UnityEngineVector2,", new DataBytes(12, 8, "Vector2") },
            { "%UnityEngineVector3,", new DataBytes(16, 12, "Vector3") },
            { "%UnityEngineVector4,", new DataBytes(20, 16, "Vector4") },
            { "%UnityEngineQuaternion,", new DataBytes(20, 16, "Quaternion") },
            { "%SystemString,", new DataBytes(68, 68, "String") }, // Null: No serialization | 68 + 1 byte per character, byte size allocated per for (84, 88, 92, etc)
            { "%VRCSDKBaseVRCUrl,", new DataBytes(68, 68, "VRCUrl") }, // ^
            { "%UnityEngineColor,", new DataBytes(20, 16, "Color") },
            { "%UnityEngineColor32,", new DataBytes(5, 5, "Color32") },

            { "%SystemBooleanArray,", new DataBytes(64, 64, "BooleanArray") }, // 64 + array size in increments of 4 as buffer (80, 84, 88, etc | Will increase every 4 values (5, 9, 13, etc)
            { "%SystemCharArray,", new DataBytes(64, 64, "CharArray") }, // ^
            { "%SystemByteArray,", new DataBytes(64, 64, "ByteArray") }, // ^
            { "%SystemSByteArray,", new DataBytes(64, 64, "UByteArray") }, // ^
            { "%SystemInt16Array,", new DataBytes(64, 64, "ShortArray") }, // 64 + array size in increments of 4 as buffer (80, 84, 88, etc | Will increase every 2 values (3, 5, 7, etc)
            { "%SystemUInt16Array,", new DataBytes(64, 64, "UShortArray") }, // ^
            { "%SystemInt32Array,", new DataBytes(64, 64, "IntArray") }, // 64 + array size * 4
            { "%SystemUInt32Array,", new DataBytes(64, 64, "UIntArray") }, // ^
            { "%SystemInt64Array,", new DataBytes(64, 64, "LongArray") }, // 64 + array size * 8
            { "%SystemUInt64Array,", new DataBytes(64, 64, "ULongArray") }, // ^
            { "%SystemSingleArray,", new DataBytes(64, 64, "FloatArray") }, // 64 + array size * 4
            { "%SystemDoubleArray,", new DataBytes(64, 64, "DoubleArray") }, // 64 + array size * 8
            { "%UnityEngineVector2Array,", new DataBytes(64, 64, "Vector2Array") }, // 64 + array size * 8
            { "%UnityEngineVector3Array,", new DataBytes(64, 64, "Vector3Array") }, // 64 + array size * 12
            { "%UnityEngineVector4Array,", new DataBytes(64, 64, "Vector4Array") }, // 64 + array size * 16
            { "%UnityEngineQuaternionArray,", new DataBytes(64, 64, "QuaternionArray") }, // 64 + array size * 16
            // STRING ARRAY INFO
            // Size of 0: 68 bytes
            // Size greater than 0 but with any null values: 0 bytes
            // Size greater than 0 with all valid strings: 68 + (2, 4, or 8 bytes per character) 
            { "%SystemStringArray,", new DataBytes(68, 68, "StringArray") }, // ^<
            { "%VRCSDKBaseVRCUrlArray,", new DataBytes(68, 68, "VRCUrlArray") }, // ^
            { "%UnityEngineColorArray,", new DataBytes(64, 64, "ColorArray") }, // 64 + array size * 16
            { "%UnityEngineColor32Array,", new DataBytes(64, 64, "Color32Array") }, // 64 + array size * 4
        };

        public int maxByte;
        public int minByte;
        public string variableType;

        public DataBytes(int maxByte, int minByte, string variableType)
        {
            this.maxByte = maxByte;
            this.minByte = minByte;
            this.variableType = variableType;
        }
    }
}
