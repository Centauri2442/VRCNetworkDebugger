﻿
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace Centauri.NetDebug
{
    public class SerializationBomber : UdonSharpBehaviour
    { 
        [UdonSynced] public string[] garbageData = { "rH4aTgEPuRn", "cFWpB0hTa", "5MIt2262PL3", "weIuzMhtZFwC5h", "2pS892uPrHUO9Vy", "bwjrsZulqai6lzMVA3TKTZo", "2pBDQkDBaf5o1K2R4zlhMgUB", "6GolXLsvFFGrBr", "6yJHGal2pi7sBTYnOZaxHeX7S5JImP", "ZlZoTwjr", "hIAEChEixSpY1z3JXYzwlTYQ", "QKJg1Zdcgu27UT9JUbl5Q86", "8GFaMXOZvZ9tEwia4HWD", "CS2yMCdFpxd42X", "kyDxs81WEveXBPxt1tyZbAPS7oo", "P2AmHdZSIiqMLeALti2sK249lP", "TD9cztzHiYSt0fhQgZrjg6SEv", "3FjjRNsZWproGTWJe4ef5FyW", "DB4dN", "gVU7KW3ADhSGd", "VjcwbhnuHX", "b7MNvPIXrT", "hhfYVKFW8D", "5HyN4KLf", "CagpN0JvIuUZl0BLQD3BPEqo0k", "9ER9yuvaHHlQTYXIRXakjsVpIuG7y", "gs6fc", "wrch8xojYHk6tt66hO5I7z", "09KVEKVwSzPkLRcBojmWlT5fnU7", "VZGbknxc8p8lv", "bJElr6ZK", "8hhiM5kBRP6YD5wuPQ", "PC7ywZHajEEXMxnmtu", "tUuKGynXNPWV", "fBURQVa25YlhxUg6EyvkH4cUtkSWo", "l4bbjBbE6fM4vuJ", "tUrcE1E7EcD", "b7Jg680qNcH6O9I", "DjdrydmraEjLn1", "0KdfbYlcT74", "PV7SyvdjnYPVPmgX", "pzPNDxIwVyTpTVkI9rsj3dC2bg", "pVGyntuBtlxIewBazMnuIZlBE6", "iRi1iyO", "Kuliqz56IjDhgHRCQe5FAi", "pUqNfkrvBVapVm", "2GxIvMVe6DtGpxdI", "uD4Gg5lEMcGZ2J4ZJiLEaydkyGS", "mtnm2DgJ44g8", "2murJG5bi04obHuARKac", "IxkIjAKbYG8dUDuT9d4wA6hE8vto", "wla8M1rgSVF13s6DjegA1X9qt66", "uyHuNRVpGCBjKTedEFU2UB", "kFuigT", "7Wd7B4AML", "JNlivmDpy7xnXZfDudZadAG", "KyLIMvO6rva1XPqqPSgYQ", "Sru6Cy1CFZTOTGFGw2TmY", "9YMXrWYfuTJEOI9WpL1ZRE8jxCurP", "HGXuktM1xs", "jOnvlX7zudhnq", "ou59Q", "Cr60j1q", "O6xxbajPqzVmvZwyzLc9KRPKC6fk", "K8nVSLxN89NJf3amWrpahow6H6bhLk", "mKhgFfLFzSRfPIWT9Qe6MYIED2", "uj4MV1NGqlmBgOKqL0", "xCaCpGsB2bpgEJ", "qt3Z32t", "cDac0QPTCnIq", "1yQbW4EK", "WYzQYeorXYhWL3s7689Ln", "O7LAaPXeHdvbaVvoceRxMi", "CxexQqc9OE8Fc7m", "VbrhHxzQwDgvcxJupQn1qmddyhm2Nm", "8GF01WTUhdsk9b", "TjB7wz6v7X6h64Q0AZmpbtIiFEK9y8", "ShoIhZnay9HZKkUBXopNDb", "oHDrMSA9af0fFaheFOhAhDsYepl2", "pZ3ZwWFe1tYvufuGV", "fxeDtr7hUWFMkDKPz9H", "CeN2za2cNIR5Mde5g", "R62eFunyEHS98MiSTx9Q4q6Ad", "3XGLHtvgxmoI", "AUlj8bDWwGDUKdanzRSwzKYZ", "5Gmc8RlABiKBpM", "uy02aej3XNu1", "ZfPuYPDZDyeTCuFUydU", "IFEGteY5efKVa", "jP3NNzW2y8zd3U", "GpQInOy1WGTfbkIr4F6Xga", "hl2BdI5z", "pyoAXv3F3HUqLutEZaMKeAlBq5hxU", "geuGq", "fN10PnaXj0zykteoRaANxl2", "gvL3B0rzAHfQ2", "kbciDJVRWD0WLZLiXrQZibqUpUBl", "hysdtfXt", "I3hOES1hoe8sT3kPW", "dcPLWPyb58Gup" };
        [UdonSynced] public string[] garbageData2 = { "rH4aTgEPuRn", "cFWpB0hTa", "5MIt2262PL3", "weIuzMhtZFwC5h", "2pS892uPrHUO9Vy", "bwjrsZulqai6lzMVA3TKTZo", "2pBDQkDBaf5o1K2R4zlhMgUB", "6GolXLsvFFGrBr", "6yJHGal2pi7sBTYnOZaxHeX7S5JImP", "ZlZoTwjr", "hIAEChEixSpY1z3JXYzwlTYQ", "QKJg1Zdcgu27UT9JUbl5Q86", "8GFaMXOZvZ9tEwia4HWD", "CS2yMCdFpxd42X", "kyDxs81WEveXBPxt1tyZbAPS7oo", "P2AmHdZSIiqMLeALti2sK249lP", "TD9cztzHiYSt0fhQgZrjg6SEv", "3FjjRNsZWproGTWJe4ef5FyW", "DB4dN", "gVU7KW3ADhSGd", "VjcwbhnuHX", "b7MNvPIXrT", "hhfYVKFW8D", "5HyN4KLf", "CagpN0JvIuUZl0BLQD3BPEqo0k", "9ER9yuvaHHlQTYXIRXakjsVpIuG7y", "gs6fc", "wrch8xojYHk6tt66hO5I7z", "09KVEKVwSzPkLRcBojmWlT5fnU7", "VZGbknxc8p8lv", "bJElr6ZK", "8hhiM5kBRP6YD5wuPQ", "PC7ywZHajEEXMxnmtu", "tUuKGynXNPWV", "fBURQVa25YlhxUg6EyvkH4cUtkSWo", "l4bbjBbE6fM4vuJ", "tUrcE1E7EcD", "b7Jg680qNcH6O9I", "DjdrydmraEjLn1", "0KdfbYlcT74", "PV7SyvdjnYPVPmgX", "pzPNDxIwVyTpTVkI9rsj3dC2bg", "pVGyntuBtlxIewBazMnuIZlBE6", "iRi1iyO", "Kuliqz56IjDhgHRCQe5FAi", "pUqNfkrvBVapVm", "2GxIvMVe6DtGpxdI", "uD4Gg5lEMcGZ2J4ZJiLEaydkyGS", "mtnm2DgJ44g8", "2murJG5bi04obHuARKac", "IxkIjAKbYG8dUDuT9d4wA6hE8vto", "wla8M1rgSVF13s6DjegA1X9qt66", "uyHuNRVpGCBjKTedEFU2UB", "kFuigT", "7Wd7B4AML", "JNlivmDpy7xnXZfDudZadAG", "KyLIMvO6rva1XPqqPSgYQ", "Sru6Cy1CFZTOTGFGw2TmY", "9YMXrWYfuTJEOI9WpL1ZRE8jxCurP", "HGXuktM1xs", "jOnvlX7zudhnq", "ou59Q", "Cr60j1q", "O6xxbajPqzVmvZwyzLc9KRPKC6fk", "K8nVSLxN89NJf3amWrpahow6H6bhLk", "mKhgFfLFzSRfPIWT9Qe6MYIED2", "uj4MV1NGqlmBgOKqL0", "xCaCpGsB2bpgEJ", "qt3Z32t", "cDac0QPTCnIq", "1yQbW4EK", "WYzQYeorXYhWL3s7689Ln", "O7LAaPXeHdvbaVvoceRxMi", "CxexQqc9OE8Fc7m", "VbrhHxzQwDgvcxJupQn1qmddyhm2Nm", "8GF01WTUhdsk9b", "TjB7wz6v7X6h64Q0AZmpbtIiFEK9y8", "ShoIhZnay9HZKkUBXopNDb", "oHDrMSA9af0fFaheFOhAhDsYepl2", "pZ3ZwWFe1tYvufuGV", "fxeDtr7hUWFMkDKPz9H", "CeN2za2cNIR5Mde5g", "R62eFunyEHS98MiSTx9Q4q6Ad", "3XGLHtvgxmoI", "AUlj8bDWwGDUKdanzRSwzKYZ", "5Gmc8RlABiKBpM", "uy02aej3XNu1", "ZfPuYPDZDyeTCuFUydU", "IFEGteY5efKVa", "jP3NNzW2y8zd3U", "GpQInOy1WGTfbkIr4F6Xga", "hl2BdI5z", "pyoAXv3F3HUqLutEZaMKeAlBq5hxU", "geuGq", "fN10PnaXj0zykteoRaANxl2", "gvL3B0rzAHfQ2", "kbciDJVRWD0WLZLiXrQZibqUpUBl", "hysdtfXt", "I3hOES1hoe8sT3kPW", "dcPLWPyb58Gup" };
        [UdonSynced] public string[] garbageData3 = { "rH4aTgEPuRn", "cFWpB0hTa", "5MIt2262PL3", "weIuzMhtZFwC5h", "2pS892uPrHUO9Vy", "bwjrsZulqai6lzMVA3TKTZo", "2pBDQkDBaf5o1K2R4zlhMgUB", "6GolXLsvFFGrBr", "6yJHGal2pi7sBTYnOZaxHeX7S5JImP", "ZlZoTwjr", "hIAEChEixSpY1z3JXYzwlTYQ", "QKJg1Zdcgu27UT9JUbl5Q86", "8GFaMXOZvZ9tEwia4HWD", "CS2yMCdFpxd42X", "kyDxs81WEveXBPxt1tyZbAPS7oo", "P2AmHdZSIiqMLeALti2sK249lP", "TD9cztzHiYSt0fhQgZrjg6SEv", "3FjjRNsZWproGTWJe4ef5FyW", "DB4dN", "gVU7KW3ADhSGd", "VjcwbhnuHX", "b7MNvPIXrT", "hhfYVKFW8D", "5HyN4KLf", "CagpN0JvIuUZl0BLQD3BPEqo0k", "9ER9yuvaHHlQTYXIRXakjsVpIuG7y", "gs6fc", "wrch8xojYHk6tt66hO5I7z", "09KVEKVwSzPkLRcBojmWlT5fnU7", "VZGbknxc8p8lv", "bJElr6ZK", "8hhiM5kBRP6YD5wuPQ", "PC7ywZHajEEXMxnmtu", "tUuKGynXNPWV", "fBURQVa25YlhxUg6EyvkH4cUtkSWo", "l4bbjBbE6fM4vuJ", "tUrcE1E7EcD", "b7Jg680qNcH6O9I", "DjdrydmraEjLn1", "0KdfbYlcT74", "PV7SyvdjnYPVPmgX", "pzPNDxIwVyTpTVkI9rsj3dC2bg", "pVGyntuBtlxIewBazMnuIZlBE6", "iRi1iyO", "Kuliqz56IjDhgHRCQe5FAi", "pUqNfkrvBVapVm", "2GxIvMVe6DtGpxdI", "uD4Gg5lEMcGZ2J4ZJiLEaydkyGS", "mtnm2DgJ44g8", "2murJG5bi04obHuARKac", "IxkIjAKbYG8dUDuT9d4wA6hE8vto", "wla8M1rgSVF13s6DjegA1X9qt66", "uyHuNRVpGCBjKTedEFU2UB", "kFuigT", "7Wd7B4AML", "JNlivmDpy7xnXZfDudZadAG", "KyLIMvO6rva1XPqqPSgYQ", "Sru6Cy1CFZTOTGFGw2TmY", "9YMXrWYfuTJEOI9WpL1ZRE8jxCurP", "HGXuktM1xs", "jOnvlX7zudhnq", "ou59Q", "Cr60j1q", "O6xxbajPqzVmvZwyzLc9KRPKC6fk", "K8nVSLxN89NJf3amWrpahow6H6bhLk", "mKhgFfLFzSRfPIWT9Qe6MYIED2", "uj4MV1NGqlmBgOKqL0", "xCaCpGsB2bpgEJ", "qt3Z32t", "cDac0QPTCnIq", "1yQbW4EK", "WYzQYeorXYhWL3s7689Ln", "O7LAaPXeHdvbaVvoceRxMi", "CxexQqc9OE8Fc7m", "VbrhHxzQwDgvcxJupQn1qmddyhm2Nm", "8GF01WTUhdsk9b", "TjB7wz6v7X6h64Q0AZmpbtIiFEK9y8", "ShoIhZnay9HZKkUBXopNDb", "oHDrMSA9af0fFaheFOhAhDsYepl2", "pZ3ZwWFe1tYvufuGV", "fxeDtr7hUWFMkDKPz9H", "CeN2za2cNIR5Mde5g", "R62eFunyEHS98MiSTx9Q4q6Ad", "3XGLHtvgxmoI", "AUlj8bDWwGDUKdanzRSwzKYZ", "5Gmc8RlABiKBpM", "uy02aej3XNu1", "ZfPuYPDZDyeTCuFUydU", "IFEGteY5efKVa", "jP3NNzW2y8zd3U", "GpQInOy1WGTfbkIr4F6Xga", "hl2BdI5z", "pyoAXv3F3HUqLutEZaMKeAlBq5hxU", "geuGq", "fN10PnaXj0zykteoRaANxl2", "gvL3B0rzAHfQ2", "kbciDJVRWD0WLZLiXrQZibqUpUBl", "hysdtfXt", "I3hOES1hoe8sT3kPW", "dcPLWPyb58Gup" };
        [UdonSynced] public string[] garbageData4 = { "rH4aTgEPuRn", "cFWpB0hTa", "5MIt2262PL3", "weIuzMhtZFwC5h", "2pS892uPrHUO9Vy", "bwjrsZulqai6lzMVA3TKTZo", "2pBDQkDBaf5o1K2R4zlhMgUB", "6GolXLsvFFGrBr", "6yJHGal2pi7sBTYnOZaxHeX7S5JImP", "ZlZoTwjr", "hIAEChEixSpY1z3JXYzwlTYQ", "QKJg1Zdcgu27UT9JUbl5Q86", "8GFaMXOZvZ9tEwia4HWD", "CS2yMCdFpxd42X", "kyDxs81WEveXBPxt1tyZbAPS7oo", "P2AmHdZSIiqMLeALti2sK249lP", "TD9cztzHiYSt0fhQgZrjg6SEv", "3FjjRNsZWproGTWJe4ef5FyW", "DB4dN", "gVU7KW3ADhSGd", "VjcwbhnuHX", "b7MNvPIXrT", "hhfYVKFW8D", "5HyN4KLf", "CagpN0JvIuUZl0BLQD3BPEqo0k", "9ER9yuvaHHlQTYXIRXakjsVpIuG7y", "gs6fc", "wrch8xojYHk6tt66hO5I7z", "09KVEKVwSzPkLRcBojmWlT5fnU7", "VZGbknxc8p8lv", "bJElr6ZK", "8hhiM5kBRP6YD5wuPQ", "PC7ywZHajEEXMxnmtu", "tUuKGynXNPWV", "fBURQVa25YlhxUg6EyvkH4cUtkSWo", "l4bbjBbE6fM4vuJ", "tUrcE1E7EcD", "b7Jg680qNcH6O9I", "DjdrydmraEjLn1", "0KdfbYlcT74", "PV7SyvdjnYPVPmgX", "pzPNDxIwVyTpTVkI9rsj3dC2bg", "pVGyntuBtlxIewBazMnuIZlBE6", "iRi1iyO", "Kuliqz56IjDhgHRCQe5FAi", "pUqNfkrvBVapVm", "2GxIvMVe6DtGpxdI", "uD4Gg5lEMcGZ2J4ZJiLEaydkyGS", "mtnm2DgJ44g8", "2murJG5bi04obHuARKac", "IxkIjAKbYG8dUDuT9d4wA6hE8vto", "wla8M1rgSVF13s6DjegA1X9qt66", "uyHuNRVpGCBjKTedEFU2UB", "kFuigT", "7Wd7B4AML", "JNlivmDpy7xnXZfDudZadAG", "KyLIMvO6rva1XPqqPSgYQ", "Sru6Cy1CFZTOTGFGw2TmY", "9YMXrWYfuTJEOI9WpL1ZRE8jxCurP", "HGXuktM1xs", "jOnvlX7zudhnq", "ou59Q", "Cr60j1q", "O6xxbajPqzVmvZwyzLc9KRPKC6fk", "K8nVSLxN89NJf3amWrpahow6H6bhLk", "mKhgFfLFzSRfPIWT9Qe6MYIED2", "uj4MV1NGqlmBgOKqL0", "xCaCpGsB2bpgEJ", "qt3Z32t", "cDac0QPTCnIq", "1yQbW4EK", "WYzQYeorXYhWL3s7689Ln", "O7LAaPXeHdvbaVvoceRxMi", "CxexQqc9OE8Fc7m", "VbrhHxzQwDgvcxJupQn1qmddyhm2Nm", "8GF01WTUhdsk9b", "TjB7wz6v7X6h64Q0AZmpbtIiFEK9y8", "ShoIhZnay9HZKkUBXopNDb", "oHDrMSA9af0fFaheFOhAhDsYepl2", "pZ3ZwWFe1tYvufuGV", "fxeDtr7hUWFMkDKPz9H", "CeN2za2cNIR5Mde5g", "R62eFunyEHS98MiSTx9Q4q6Ad", "3XGLHtvgxmoI", "AUlj8bDWwGDUKdanzRSwzKYZ", "5Gmc8RlABiKBpM", "uy02aej3XNu1", "ZfPuYPDZDyeTCuFUydU", "IFEGteY5efKVa", "jP3NNzW2y8zd3U", "GpQInOy1WGTfbkIr4F6Xga", "hl2BdI5z", "pyoAXv3F3HUqLutEZaMKeAlBq5hxU", "geuGq", "fN10PnaXj0zykteoRaANxl2", "gvL3B0rzAHfQ2", "kbciDJVRWD0WLZLiXrQZibqUpUBl", "hysdtfXt", "I3hOES1hoe8sT3kPW", "dcPLWPyb58Gup" };
        [UdonSynced] public string[] garbageData5 = { "rH4aTgEPuRn", "cFWpB0hTa", "5MIt2262PL3", "weIuzMhtZFwC5h", "2pS892uPrHUO9Vy", "bwjrsZulqai6lzMVA3TKTZo", "2pBDQkDBaf5o1K2R4zlhMgUB", "6GolXLsvFFGrBr", "6yJHGal2pi7sBTYnOZaxHeX7S5JImP", "ZlZoTwjr", "hIAEChEixSpY1z3JXYzwlTYQ", "QKJg1Zdcgu27UT9JUbl5Q86", "8GFaMXOZvZ9tEwia4HWD", "CS2yMCdFpxd42X", "kyDxs81WEveXBPxt1tyZbAPS7oo", "P2AmHdZSIiqMLeALti2sK249lP", "TD9cztzHiYSt0fhQgZrjg6SEv", "3FjjRNsZWproGTWJe4ef5FyW", "DB4dN", "gVU7KW3ADhSGd", "VjcwbhnuHX", "b7MNvPIXrT", "hhfYVKFW8D", "5HyN4KLf", "CagpN0JvIuUZl0BLQD3BPEqo0k", "9ER9yuvaHHlQTYXIRXakjsVpIuG7y", "gs6fc", "wrch8xojYHk6tt66hO5I7z", "09KVEKVwSzPkLRcBojmWlT5fnU7", "VZGbknxc8p8lv", "bJElr6ZK", "8hhiM5kBRP6YD5wuPQ", "PC7ywZHajEEXMxnmtu", "tUuKGynXNPWV", "fBURQVa25YlhxUg6EyvkH4cUtkSWo", "l4bbjBbE6fM4vuJ", "tUrcE1E7EcD", "b7Jg680qNcH6O9I", "DjdrydmraEjLn1", "0KdfbYlcT74", "PV7SyvdjnYPVPmgX", "pzPNDxIwVyTpTVkI9rsj3dC2bg", "pVGyntuBtlxIewBazMnuIZlBE6", "iRi1iyO", "Kuliqz56IjDhgHRCQe5FAi", "pUqNfkrvBVapVm", "2GxIvMVe6DtGpxdI", "uD4Gg5lEMcGZ2J4ZJiLEaydkyGS", "mtnm2DgJ44g8", "2murJG5bi04obHuARKac", "IxkIjAKbYG8dUDuT9d4wA6hE8vto", "wla8M1rgSVF13s6DjegA1X9qt66", "uyHuNRVpGCBjKTedEFU2UB", "kFuigT", "7Wd7B4AML", "JNlivmDpy7xnXZfDudZadAG", "KyLIMvO6rva1XPqqPSgYQ", "Sru6Cy1CFZTOTGFGw2TmY", "9YMXrWYfuTJEOI9WpL1ZRE8jxCurP", "HGXuktM1xs", "jOnvlX7zudhnq", "ou59Q", "Cr60j1q", "O6xxbajPqzVmvZwyzLc9KRPKC6fk", "K8nVSLxN89NJf3amWrpahow6H6bhLk", "mKhgFfLFzSRfPIWT9Qe6MYIED2", "uj4MV1NGqlmBgOKqL0", "xCaCpGsB2bpgEJ", "qt3Z32t", "cDac0QPTCnIq", "1yQbW4EK", "WYzQYeorXYhWL3s7689Ln", "O7LAaPXeHdvbaVvoceRxMi", "CxexQqc9OE8Fc7m", "VbrhHxzQwDgvcxJupQn1qmddyhm2Nm", "8GF01WTUhdsk9b", "TjB7wz6v7X6h64Q0AZmpbtIiFEK9y8", "ShoIhZnay9HZKkUBXopNDb", "oHDrMSA9af0fFaheFOhAhDsYepl2", "pZ3ZwWFe1tYvufuGV", "fxeDtr7hUWFMkDKPz9H", "CeN2za2cNIR5Mde5g", "R62eFunyEHS98MiSTx9Q4q6Ad", "3XGLHtvgxmoI", "AUlj8bDWwGDUKdanzRSwzKYZ", "5Gmc8RlABiKBpM", "uy02aej3XNu1", "ZfPuYPDZDyeTCuFUydU", "IFEGteY5efKVa", "jP3NNzW2y8zd3U", "GpQInOy1WGTfbkIr4F6Xga", "hl2BdI5z", "pyoAXv3F3HUqLutEZaMKeAlBq5hxU", "geuGq", "fN10PnaXj0zykteoRaANxl2", "gvL3B0rzAHfQ2", "kbciDJVRWD0WLZLiXrQZibqUpUBl", "hysdtfXt", "I3hOES1hoe8sT3kPW", "dcPLWPyb58Gup" };
        [UdonSynced] public string[] garbageData6 = { "rH4aTgEPuRn", "cFWpB0hTa", "5MIt2262PL3", "weIuzMhtZFwC5h", "2pS892uPrHUO9Vy", "bwjrsZulqai6lzMVA3TKTZo", "2pBDQkDBaf5o1K2R4zlhMgUB", "6GolXLsvFFGrBr", "6yJHGal2pi7sBTYnOZaxHeX7S5JImP", "ZlZoTwjr", "hIAEChEixSpY1z3JXYzwlTYQ", "QKJg1Zdcgu27UT9JUbl5Q86", "8GFaMXOZvZ9tEwia4HWD", "CS2yMCdFpxd42X", "kyDxs81WEveXBPxt1tyZbAPS7oo", "P2AmHdZSIiqMLeALti2sK249lP", "TD9cztzHiYSt0fhQgZrjg6SEv", "3FjjRNsZWproGTWJe4ef5FyW", "DB4dN", "gVU7KW3ADhSGd", "VjcwbhnuHX", "b7MNvPIXrT", "hhfYVKFW8D", "5HyN4KLf", "CagpN0JvIuUZl0BLQD3BPEqo0k", "9ER9yuvaHHlQTYXIRXakjsVpIuG7y", "gs6fc", "wrch8xojYHk6tt66hO5I7z", "09KVEKVwSzPkLRcBojmWlT5fnU7", "VZGbknxc8p8lv", "bJElr6ZK", "8hhiM5kBRP6YD5wuPQ", "PC7ywZHajEEXMxnmtu", "tUuKGynXNPWV", "fBURQVa25YlhxUg6EyvkH4cUtkSWo", "l4bbjBbE6fM4vuJ", "tUrcE1E7EcD", "b7Jg680qNcH6O9I", "DjdrydmraEjLn1", "0KdfbYlcT74", "PV7SyvdjnYPVPmgX", "pzPNDxIwVyTpTVkI9rsj3dC2bg", "pVGyntuBtlxIewBazMnuIZlBE6", "iRi1iyO", "Kuliqz56IjDhgHRCQe5FAi", "pUqNfkrvBVapVm", "2GxIvMVe6DtGpxdI", "uD4Gg5lEMcGZ2J4ZJiLEaydkyGS", "mtnm2DgJ44g8", "2murJG5bi04obHuARKac", "IxkIjAKbYG8dUDuT9d4wA6hE8vto", "wla8M1rgSVF13s6DjegA1X9qt66", "uyHuNRVpGCBjKTedEFU2UB", "kFuigT", "7Wd7B4AML", "JNlivmDpy7xnXZfDudZadAG", "KyLIMvO6rva1XPqqPSgYQ", "Sru6Cy1CFZTOTGFGw2TmY", "9YMXrWYfuTJEOI9WpL1ZRE8jxCurP", "HGXuktM1xs", "jOnvlX7zudhnq", "ou59Q", "Cr60j1q", "O6xxbajPqzVmvZwyzLc9KRPKC6fk", "K8nVSLxN89NJf3amWrpahow6H6bhLk", "mKhgFfLFzSRfPIWT9Qe6MYIED2", "uj4MV1NGqlmBgOKqL0", "xCaCpGsB2bpgEJ", "qt3Z32t", "cDac0QPTCnIq", "1yQbW4EK", "WYzQYeorXYhWL3s7689Ln", "O7LAaPXeHdvbaVvoceRxMi", "CxexQqc9OE8Fc7m", "VbrhHxzQwDgvcxJupQn1qmddyhm2Nm", "8GF01WTUhdsk9b", "TjB7wz6v7X6h64Q0AZmpbtIiFEK9y8", "ShoIhZnay9HZKkUBXopNDb", "oHDrMSA9af0fFaheFOhAhDsYepl2", "pZ3ZwWFe1tYvufuGV", "fxeDtr7hUWFMkDKPz9H", "CeN2za2cNIR5Mde5g", "R62eFunyEHS98MiSTx9Q4q6Ad", "3XGLHtvgxmoI", "AUlj8bDWwGDUKdanzRSwzKYZ", "5Gmc8RlABiKBpM", "uy02aej3XNu1", "ZfPuYPDZDyeTCuFUydU", "IFEGteY5efKVa", "jP3NNzW2y8zd3U", "GpQInOy1WGTfbkIr4F6Xga", "hl2BdI5z", "pyoAXv3F3HUqLutEZaMKeAlBq5hxU", "geuGq", "fN10PnaXj0zykteoRaANxl2", "gvL3B0rzAHfQ2", "kbciDJVRWD0WLZLiXrQZibqUpUBl", "hysdtfXt", "I3hOES1hoe8sT3kPW", "dcPLWPyb58Gup" };
        [UdonSynced] public string[] garbageData7 = { "rH4aTgEPuRn", "cFWpB0hTa", "5MIt2262PL3", "weIuzMhtZFwC5h", "2pS892uPrHUO9Vy", "bwjrsZulqai6lzMVA3TKTZo", "2pBDQkDBaf5o1K2R4zlhMgUB", "6GolXLsvFFGrBr", "6yJHGal2pi7sBTYnOZaxHeX7S5JImP", "ZlZoTwjr", "hIAEChEixSpY1z3JXYzwlTYQ", "QKJg1Zdcgu27UT9JUbl5Q86", "8GFaMXOZvZ9tEwia4HWD", "CS2yMCdFpxd42X", "kyDxs81WEveXBPxt1tyZbAPS7oo", "P2AmHdZSIiqMLeALti2sK249lP", "TD9cztzHiYSt0fhQgZrjg6SEv", "3FjjRNsZWproGTWJe4ef5FyW", "DB4dN", "gVU7KW3ADhSGd", "VjcwbhnuHX", "b7MNvPIXrT", "hhfYVKFW8D", "5HyN4KLf", "CagpN0JvIuUZl0BLQD3BPEqo0k", "9ER9yuvaHHlQTYXIRXakjsVpIuG7y", "gs6fc", "wrch8xojYHk6tt66hO5I7z", "09KVEKVwSzPkLRcBojmWlT5fnU7", "VZGbknxc8p8lv", "bJElr6ZK", "8hhiM5kBRP6YD5wuPQ", "PC7ywZHajEEXMxnmtu", "tUuKGynXNPWV", "fBURQVa25YlhxUg6EyvkH4cUtkSWo", "l4bbjBbE6fM4vuJ", "tUrcE1E7EcD", "b7Jg680qNcH6O9I", "DjdrydmraEjLn1", "0KdfbYlcT74", "PV7SyvdjnYPVPmgX", "pzPNDxIwVyTpTVkI9rsj3dC2bg", "pVGyntuBtlxIewBazMnuIZlBE6", "iRi1iyO", "Kuliqz56IjDhgHRCQe5FAi", "pUqNfkrvBVapVm", "2GxIvMVe6DtGpxdI", "uD4Gg5lEMcGZ2J4ZJiLEaydkyGS", "mtnm2DgJ44g8", "2murJG5bi04obHuARKac", "IxkIjAKbYG8dUDuT9d4wA6hE8vto", "wla8M1rgSVF13s6DjegA1X9qt66", "uyHuNRVpGCBjKTedEFU2UB", "kFuigT", "7Wd7B4AML", "JNlivmDpy7xnXZfDudZadAG", "KyLIMvO6rva1XPqqPSgYQ", "Sru6Cy1CFZTOTGFGw2TmY", "9YMXrWYfuTJEOI9WpL1ZRE8jxCurP", "HGXuktM1xs", "jOnvlX7zudhnq", "ou59Q", "Cr60j1q", "O6xxbajPqzVmvZwyzLc9KRPKC6fk", "K8nVSLxN89NJf3amWrpahow6H6bhLk", "mKhgFfLFzSRfPIWT9Qe6MYIED2", "uj4MV1NGqlmBgOKqL0", "xCaCpGsB2bpgEJ", "qt3Z32t", "cDac0QPTCnIq", "1yQbW4EK", "WYzQYeorXYhWL3s7689Ln", "O7LAaPXeHdvbaVvoceRxMi", "CxexQqc9OE8Fc7m", "VbrhHxzQwDgvcxJupQn1qmddyhm2Nm", "8GF01WTUhdsk9b", "TjB7wz6v7X6h64Q0AZmpbtIiFEK9y8", "ShoIhZnay9HZKkUBXopNDb", "oHDrMSA9af0fFaheFOhAhDsYepl2", "pZ3ZwWFe1tYvufuGV", "fxeDtr7hUWFMkDKPz9H", "CeN2za2cNIR5Mde5g", "R62eFunyEHS98MiSTx9Q4q6Ad", "3XGLHtvgxmoI", "AUlj8bDWwGDUKdanzRSwzKYZ", "5Gmc8RlABiKBpM", "uy02aej3XNu1", "ZfPuYPDZDyeTCuFUydU", "IFEGteY5efKVa", "jP3NNzW2y8zd3U", "GpQInOy1WGTfbkIr4F6Xga", "hl2BdI5z", "pyoAXv3F3HUqLutEZaMKeAlBq5hxU", "geuGq", "fN10PnaXj0zykteoRaANxl2", "gvL3B0rzAHfQ2", "kbciDJVRWD0WLZLiXrQZibqUpUBl", "hysdtfXt", "I3hOES1hoe8sT3kPW", "dcPLWPyb58Gup" };
        [UdonSynced] public string[] garbageData8 = { "rH4aTgEPuRn", "cFWpB0hTa", "5MIt2262PL3", "weIuzMhtZFwC5h", "2pS892uPrHUO9Vy", "bwjrsZulqai6lzMVA3TKTZo", "2pBDQkDBaf5o1K2R4zlhMgUB", "6GolXLsvFFGrBr", "6yJHGal2pi7sBTYnOZaxHeX7S5JImP", "ZlZoTwjr", "hIAEChEixSpY1z3JXYzwlTYQ", "QKJg1Zdcgu27UT9JUbl5Q86", "8GFaMXOZvZ9tEwia4HWD", "CS2yMCdFpxd42X", "kyDxs81WEveXBPxt1tyZbAPS7oo", "P2AmHdZSIiqMLeALti2sK249lP", "TD9cztzHiYSt0fhQgZrjg6SEv", "3FjjRNsZWproGTWJe4ef5FyW", "DB4dN", "gVU7KW3ADhSGd", "VjcwbhnuHX", "b7MNvPIXrT", "hhfYVKFW8D", "5HyN4KLf", "CagpN0JvIuUZl0BLQD3BPEqo0k", "9ER9yuvaHHlQTYXIRXakjsVpIuG7y", "gs6fc", "wrch8xojYHk6tt66hO5I7z", "09KVEKVwSzPkLRcBojmWlT5fnU7", "VZGbknxc8p8lv", "bJElr6ZK", "8hhiM5kBRP6YD5wuPQ", "PC7ywZHajEEXMxnmtu", "tUuKGynXNPWV", "fBURQVa25YlhxUg6EyvkH4cUtkSWo", "l4bbjBbE6fM4vuJ", "tUrcE1E7EcD", "b7Jg680qNcH6O9I", "DjdrydmraEjLn1", "0KdfbYlcT74", "PV7SyvdjnYPVPmgX", "pzPNDxIwVyTpTVkI9rsj3dC2bg", "pVGyntuBtlxIewBazMnuIZlBE6", "iRi1iyO", "Kuliqz56IjDhgHRCQe5FAi", "pUqNfkrvBVapVm", "2GxIvMVe6DtGpxdI", "uD4Gg5lEMcGZ2J4ZJiLEaydkyGS", "mtnm2DgJ44g8", "2murJG5bi04obHuARKac", "IxkIjAKbYG8dUDuT9d4wA6hE8vto", "wla8M1rgSVF13s6DjegA1X9qt66", "uyHuNRVpGCBjKTedEFU2UB", "kFuigT", "7Wd7B4AML", "JNlivmDpy7xnXZfDudZadAG", "KyLIMvO6rva1XPqqPSgYQ", "Sru6Cy1CFZTOTGFGw2TmY", "9YMXrWYfuTJEOI9WpL1ZRE8jxCurP", "HGXuktM1xs", "jOnvlX7zudhnq", "ou59Q", "Cr60j1q", "O6xxbajPqzVmvZwyzLc9KRPKC6fk", "K8nVSLxN89NJf3amWrpahow6H6bhLk", "mKhgFfLFzSRfPIWT9Qe6MYIED2", "uj4MV1NGqlmBgOKqL0", "xCaCpGsB2bpgEJ", "qt3Z32t", "cDac0QPTCnIq", "1yQbW4EK", "WYzQYeorXYhWL3s7689Ln", "O7LAaPXeHdvbaVvoceRxMi", "CxexQqc9OE8Fc7m", "VbrhHxzQwDgvcxJupQn1qmddyhm2Nm", "8GF01WTUhdsk9b", "TjB7wz6v7X6h64Q0AZmpbtIiFEK9y8", "ShoIhZnay9HZKkUBXopNDb", "oHDrMSA9af0fFaheFOhAhDsYepl2", "pZ3ZwWFe1tYvufuGV", "fxeDtr7hUWFMkDKPz9H", "CeN2za2cNIR5Mde5g", "R62eFunyEHS98MiSTx9Q4q6Ad", "3XGLHtvgxmoI", "AUlj8bDWwGDUKdanzRSwzKYZ", "5Gmc8RlABiKBpM", "uy02aej3XNu1", "ZfPuYPDZDyeTCuFUydU", "IFEGteY5efKVa", "jP3NNzW2y8zd3U", "GpQInOy1WGTfbkIr4F6Xga", "hl2BdI5z", "pyoAXv3F3HUqLutEZaMKeAlBq5hxU", "geuGq", "fN10PnaXj0zykteoRaANxl2", "gvL3B0rzAHfQ2", "kbciDJVRWD0WLZLiXrQZibqUpUBl", "hysdtfXt", "I3hOES1hoe8sT3kPW", "dcPLWPyb58Gup" };
        [UdonSynced] public string[] garbageData9 = { "rH4aTgEPuRn", "cFWpB0hTa", "5MIt2262PL3", "weIuzMhtZFwC5h", "2pS892uPrHUO9Vy", "bwjrsZulqai6lzMVA3TKTZo", "2pBDQkDBaf5o1K2R4zlhMgUB", "6GolXLsvFFGrBr", "6yJHGal2pi7sBTYnOZaxHeX7S5JImP", "ZlZoTwjr", "hIAEChEixSpY1z3JXYzwlTYQ", "QKJg1Zdcgu27UT9JUbl5Q86", "8GFaMXOZvZ9tEwia4HWD", "CS2yMCdFpxd42X", "kyDxs81WEveXBPxt1tyZbAPS7oo", "P2AmHdZSIiqMLeALti2sK249lP", "TD9cztzHiYSt0fhQgZrjg6SEv", "3FjjRNsZWproGTWJe4ef5FyW", "DB4dN", "gVU7KW3ADhSGd", "VjcwbhnuHX", "b7MNvPIXrT", "hhfYVKFW8D", "5HyN4KLf", "CagpN0JvIuUZl0BLQD3BPEqo0k", "9ER9yuvaHHlQTYXIRXakjsVpIuG7y", "gs6fc", "wrch8xojYHk6tt66hO5I7z", "09KVEKVwSzPkLRcBojmWlT5fnU7", "VZGbknxc8p8lv", "bJElr6ZK", "8hhiM5kBRP6YD5wuPQ", "PC7ywZHajEEXMxnmtu", "tUuKGynXNPWV", "fBURQVa25YlhxUg6EyvkH4cUtkSWo", "l4bbjBbE6fM4vuJ", "tUrcE1E7EcD", "b7Jg680qNcH6O9I", "DjdrydmraEjLn1", "0KdfbYlcT74", "PV7SyvdjnYPVPmgX", "pzPNDxIwVyTpTVkI9rsj3dC2bg", "pVGyntuBtlxIewBazMnuIZlBE6", "iRi1iyO", "Kuliqz56IjDhgHRCQe5FAi", "pUqNfkrvBVapVm", "2GxIvMVe6DtGpxdI", "uD4Gg5lEMcGZ2J4ZJiLEaydkyGS", "mtnm2DgJ44g8", "2murJG5bi04obHuARKac", "IxkIjAKbYG8dUDuT9d4wA6hE8vto", "wla8M1rgSVF13s6DjegA1X9qt66", "uyHuNRVpGCBjKTedEFU2UB", "kFuigT", "7Wd7B4AML", "JNlivmDpy7xnXZfDudZadAG", "KyLIMvO6rva1XPqqPSgYQ", "Sru6Cy1CFZTOTGFGw2TmY", "9YMXrWYfuTJEOI9WpL1ZRE8jxCurP", "HGXuktM1xs", "jOnvlX7zudhnq", "ou59Q", "Cr60j1q", "O6xxbajPqzVmvZwyzLc9KRPKC6fk", "K8nVSLxN89NJf3amWrpahow6H6bhLk", "mKhgFfLFzSRfPIWT9Qe6MYIED2", "uj4MV1NGqlmBgOKqL0", "xCaCpGsB2bpgEJ", "qt3Z32t", "cDac0QPTCnIq", "1yQbW4EK", "WYzQYeorXYhWL3s7689Ln", "O7LAaPXeHdvbaVvoceRxMi", "CxexQqc9OE8Fc7m", "VbrhHxzQwDgvcxJupQn1qmddyhm2Nm", "8GF01WTUhdsk9b", "TjB7wz6v7X6h64Q0AZmpbtIiFEK9y8", "ShoIhZnay9HZKkUBXopNDb", "oHDrMSA9af0fFaheFOhAhDsYepl2", "pZ3ZwWFe1tYvufuGV", "fxeDtr7hUWFMkDKPz9H", "CeN2za2cNIR5Mde5g", "R62eFunyEHS98MiSTx9Q4q6Ad", "3XGLHtvgxmoI", "AUlj8bDWwGDUKdanzRSwzKYZ", "5Gmc8RlABiKBpM", "uy02aej3XNu1", "ZfPuYPDZDyeTCuFUydU", "IFEGteY5efKVa", "jP3NNzW2y8zd3U", "GpQInOy1WGTfbkIr4F6Xga", "hl2BdI5z", "pyoAXv3F3HUqLutEZaMKeAlBq5hxU", "geuGq", "fN10PnaXj0zykteoRaANxl2", "gvL3B0rzAHfQ2", "kbciDJVRWD0WLZLiXrQZibqUpUBl", "hysdtfXt", "I3hOES1hoe8sT3kPW", "dcPLWPyb58Gup" };
        [UdonSynced] public string[] garbageData10 = { "rH4aTgEPuRn", "cFWpB0hTa", "5MIt2262PL3", "weIuzMhtZFwC5h", "2pS892uPrHUO9Vy", "bwjrsZulqai6lzMVA3TKTZo", "2pBDQkDBaf5o1K2R4zlhMgUB", "6GolXLsvFFGrBr", "6yJHGal2pi7sBTYnOZaxHeX7S5JImP", "ZlZoTwjr", "hIAEChEixSpY1z3JXYzwlTYQ", "QKJg1Zdcgu27UT9JUbl5Q86", "8GFaMXOZvZ9tEwia4HWD", "CS2yMCdFpxd42X", "kyDxs81WEveXBPxt1tyZbAPS7oo", "P2AmHdZSIiqMLeALti2sK249lP", "TD9cztzHiYSt0fhQgZrjg6SEv", "3FjjRNsZWproGTWJe4ef5FyW", "DB4dN", "gVU7KW3ADhSGd", "VjcwbhnuHX", "b7MNvPIXrT", "hhfYVKFW8D", "5HyN4KLf", "CagpN0JvIuUZl0BLQD3BPEqo0k", "9ER9yuvaHHlQTYXIRXakjsVpIuG7y", "gs6fc", "wrch8xojYHk6tt66hO5I7z", "09KVEKVwSzPkLRcBojmWlT5fnU7", "VZGbknxc8p8lv", "bJElr6ZK", "8hhiM5kBRP6YD5wuPQ", "PC7ywZHajEEXMxnmtu", "tUuKGynXNPWV", "fBURQVa25YlhxUg6EyvkH4cUtkSWo", "l4bbjBbE6fM4vuJ", "tUrcE1E7EcD", "b7Jg680qNcH6O9I", "DjdrydmraEjLn1", "0KdfbYlcT74", "PV7SyvdjnYPVPmgX", "pzPNDxIwVyTpTVkI9rsj3dC2bg", "pVGyntuBtlxIewBazMnuIZlBE6", "iRi1iyO", "Kuliqz56IjDhgHRCQe5FAi", "pUqNfkrvBVapVm", "2GxIvMVe6DtGpxdI", "uD4Gg5lEMcGZ2J4ZJiLEaydkyGS", "mtnm2DgJ44g8", "2murJG5bi04obHuARKac", "IxkIjAKbYG8dUDuT9d4wA6hE8vto", "wla8M1rgSVF13s6DjegA1X9qt66", "uyHuNRVpGCBjKTedEFU2UB", "kFuigT", "7Wd7B4AML", "JNlivmDpy7xnXZfDudZadAG", "KyLIMvO6rva1XPqqPSgYQ", "Sru6Cy1CFZTOTGFGw2TmY", "9YMXrWYfuTJEOI9WpL1ZRE8jxCurP", "HGXuktM1xs", "jOnvlX7zudhnq", "ou59Q", "Cr60j1q", "O6xxbajPqzVmvZwyzLc9KRPKC6fk", "K8nVSLxN89NJf3amWrpahow6H6bhLk", "mKhgFfLFzSRfPIWT9Qe6MYIED2", "uj4MV1NGqlmBgOKqL0", "xCaCpGsB2bpgEJ", "qt3Z32t", "cDac0QPTCnIq", "1yQbW4EK", "WYzQYeorXYhWL3s7689Ln", "O7LAaPXeHdvbaVvoceRxMi", "CxexQqc9OE8Fc7m", "VbrhHxzQwDgvcxJupQn1qmddyhm2Nm", "8GF01WTUhdsk9b", "TjB7wz6v7X6h64Q0AZmpbtIiFEK9y8", "ShoIhZnay9HZKkUBXopNDb", "oHDrMSA9af0fFaheFOhAhDsYepl2", "pZ3ZwWFe1tYvufuGV", "fxeDtr7hUWFMkDKPz9H", "CeN2za2cNIR5Mde5g", "R62eFunyEHS98MiSTx9Q4q6Ad", "3XGLHtvgxmoI", "AUlj8bDWwGDUKdanzRSwzKYZ", "5Gmc8RlABiKBpM", "uy02aej3XNu1", "ZfPuYPDZDyeTCuFUydU", "IFEGteY5efKVa", "jP3NNzW2y8zd3U", "GpQInOy1WGTfbkIr4F6Xga", "hl2BdI5z", "pyoAXv3F3HUqLutEZaMKeAlBq5hxU", "geuGq", "fN10PnaXj0zykteoRaANxl2", "gvL3B0rzAHfQ2", "kbciDJVRWD0WLZLiXrQZibqUpUBl", "hysdtfXt", "I3hOES1hoe8sT3kPW", "dcPLWPyb58Gup" };
        [UdonSynced] public string[] garbageData11 = { "rH4aTgEPuRn", "cFWpB0hTa", "5MIt2262PL3", "weIuzMhtZFwC5h", "2pS892uPrHUO9Vy", "bwjrsZulqai6lzMVA3TKTZo", "2pBDQkDBaf5o1K2R4zlhMgUB", "6GolXLsvFFGrBr", "6yJHGal2pi7sBTYnOZaxHeX7S5JImP", "ZlZoTwjr", "hIAEChEixSpY1z3JXYzwlTYQ", "QKJg1Zdcgu27UT9JUbl5Q86", "8GFaMXOZvZ9tEwia4HWD", "CS2yMCdFpxd42X", "kyDxs81WEveXBPxt1tyZbAPS7oo", "P2AmHdZSIiqMLeALti2sK249lP", "TD9cztzHiYSt0fhQgZrjg6SEv", "3FjjRNsZWproGTWJe4ef5FyW", "DB4dN", "gVU7KW3ADhSGd", "VjcwbhnuHX", "b7MNvPIXrT", "hhfYVKFW8D", "5HyN4KLf", "CagpN0JvIuUZl0BLQD3BPEqo0k", "9ER9yuvaHHlQTYXIRXakjsVpIuG7y", "gs6fc", "wrch8xojYHk6tt66hO5I7z", "09KVEKVwSzPkLRcBojmWlT5fnU7", "VZGbknxc8p8lv", "bJElr6ZK", "8hhiM5kBRP6YD5wuPQ", "PC7ywZHajEEXMxnmtu", "tUuKGynXNPWV", "fBURQVa25YlhxUg6EyvkH4cUtkSWo", "l4bbjBbE6fM4vuJ", "tUrcE1E7EcD", "b7Jg680qNcH6O9I", "DjdrydmraEjLn1", "0KdfbYlcT74", "PV7SyvdjnYPVPmgX", "pzPNDxIwVyTpTVkI9rsj3dC2bg", "pVGyntuBtlxIewBazMnuIZlBE6", "iRi1iyO", "Kuliqz56IjDhgHRCQe5FAi", "pUqNfkrvBVapVm", "2GxIvMVe6DtGpxdI", "uD4Gg5lEMcGZ2J4ZJiLEaydkyGS", "mtnm2DgJ44g8", "2murJG5bi04obHuARKac", "IxkIjAKbYG8dUDuT9d4wA6hE8vto", "wla8M1rgSVF13s6DjegA1X9qt66", "uyHuNRVpGCBjKTedEFU2UB", "kFuigT", "7Wd7B4AML", "JNlivmDpy7xnXZfDudZadAG", "KyLIMvO6rva1XPqqPSgYQ", "Sru6Cy1CFZTOTGFGw2TmY", "9YMXrWYfuTJEOI9WpL1ZRE8jxCurP", "HGXuktM1xs", "jOnvlX7zudhnq", "ou59Q", "Cr60j1q", "O6xxbajPqzVmvZwyzLc9KRPKC6fk", "K8nVSLxN89NJf3amWrpahow6H6bhLk", "mKhgFfLFzSRfPIWT9Qe6MYIED2", "uj4MV1NGqlmBgOKqL0", "xCaCpGsB2bpgEJ", "qt3Z32t", "cDac0QPTCnIq", "1yQbW4EK", "WYzQYeorXYhWL3s7689Ln", "O7LAaPXeHdvbaVvoceRxMi", "CxexQqc9OE8Fc7m", "VbrhHxzQwDgvcxJupQn1qmddyhm2Nm", "8GF01WTUhdsk9b", "TjB7wz6v7X6h64Q0AZmpbtIiFEK9y8", "ShoIhZnay9HZKkUBXopNDb", "oHDrMSA9af0fFaheFOhAhDsYepl2", "pZ3ZwWFe1tYvufuGV", "fxeDtr7hUWFMkDKPz9H", "CeN2za2cNIR5Mde5g", "R62eFunyEHS98MiSTx9Q4q6Ad", "3XGLHtvgxmoI", "AUlj8bDWwGDUKdanzRSwzKYZ", "5Gmc8RlABiKBpM", "uy02aej3XNu1", "ZfPuYPDZDyeTCuFUydU", "IFEGteY5efKVa", "jP3NNzW2y8zd3U", "GpQInOy1WGTfbkIr4F6Xga", "hl2BdI5z", "pyoAXv3F3HUqLutEZaMKeAlBq5hxU", "geuGq", "fN10PnaXj0zykteoRaANxl2", "gvL3B0rzAHfQ2", "kbciDJVRWD0WLZLiXrQZibqUpUBl", "hysdtfXt", "I3hOES1hoe8sT3kPW", "dcPLWPyb58Gup" };
        [UdonSynced] public string[] garbageData12 = { "rH4aTgEPuRn", "cFWpB0hTa", "5MIt2262PL3", "weIuzMhtZFwC5h", "2pS892uPrHUO9Vy", "bwjrsZulqai6lzMVA3TKTZo", "2pBDQkDBaf5o1K2R4zlhMgUB", "6GolXLsvFFGrBr", "6yJHGal2pi7sBTYnOZaxHeX7S5JImP", "ZlZoTwjr", "hIAEChEixSpY1z3JXYzwlTYQ", "QKJg1Zdcgu27UT9JUbl5Q86", "8GFaMXOZvZ9tEwia4HWD", "CS2yMCdFpxd42X", "kyDxs81WEveXBPxt1tyZbAPS7oo", "P2AmHdZSIiqMLeALti2sK249lP", "TD9cztzHiYSt0fhQgZrjg6SEv", "3FjjRNsZWproGTWJe4ef5FyW", "DB4dN", "gVU7KW3ADhSGd", "VjcwbhnuHX", "b7MNvPIXrT", "hhfYVKFW8D", "5HyN4KLf", "CagpN0JvIuUZl0BLQD3BPEqo0k", "9ER9yuvaHHlQTYXIRXakjsVpIuG7y", "gs6fc", "wrch8xojYHk6tt66hO5I7z", "09KVEKVwSzPkLRcBojmWlT5fnU7", "VZGbknxc8p8lv", "bJElr6ZK", "8hhiM5kBRP6YD5wuPQ", "PC7ywZHajEEXMxnmtu", "tUuKGynXNPWV", "fBURQVa25YlhxUg6EyvkH4cUtkSWo", "l4bbjBbE6fM4vuJ", "tUrcE1E7EcD", "b7Jg680qNcH6O9I", "DjdrydmraEjLn1", "0KdfbYlcT74", "PV7SyvdjnYPVPmgX", "pzPNDxIwVyTpTVkI9rsj3dC2bg", "pVGyntuBtlxIewBazMnuIZlBE6", "iRi1iyO", "Kuliqz56IjDhgHRCQe5FAi", "pUqNfkrvBVapVm", "2GxIvMVe6DtGpxdI", "uD4Gg5lEMcGZ2J4ZJiLEaydkyGS", "mtnm2DgJ44g8", "2murJG5bi04obHuARKac", "IxkIjAKbYG8dUDuT9d4wA6hE8vto", "wla8M1rgSVF13s6DjegA1X9qt66", "uyHuNRVpGCBjKTedEFU2UB", "kFuigT", "7Wd7B4AML", "JNlivmDpy7xnXZfDudZadAG", "KyLIMvO6rva1XPqqPSgYQ", "Sru6Cy1CFZTOTGFGw2TmY", "9YMXrWYfuTJEOI9WpL1ZRE8jxCurP", "HGXuktM1xs", "jOnvlX7zudhnq", "ou59Q", "Cr60j1q", "O6xxbajPqzVmvZwyzLc9KRPKC6fk", "K8nVSLxN89NJf3amWrpahow6H6bhLk", "mKhgFfLFzSRfPIWT9Qe6MYIED2", "uj4MV1NGqlmBgOKqL0", "xCaCpGsB2bpgEJ", "qt3Z32t", "cDac0QPTCnIq", "1yQbW4EK", "WYzQYeorXYhWL3s7689Ln", "O7LAaPXeHdvbaVvoceRxMi", "CxexQqc9OE8Fc7m", "VbrhHxzQwDgvcxJupQn1qmddyhm2Nm", "8GF01WTUhdsk9b", "TjB7wz6v7X6h64Q0AZmpbtIiFEK9y8", "ShoIhZnay9HZKkUBXopNDb", "oHDrMSA9af0fFaheFOhAhDsYepl2", "pZ3ZwWFe1tYvufuGV", "fxeDtr7hUWFMkDKPz9H", "CeN2za2cNIR5Mde5g", "R62eFunyEHS98MiSTx9Q4q6Ad", "3XGLHtvgxmoI", "AUlj8bDWwGDUKdanzRSwzKYZ", "5Gmc8RlABiKBpM", "uy02aej3XNu1", "ZfPuYPDZDyeTCuFUydU", "IFEGteY5efKVa", "jP3NNzW2y8zd3U", "GpQInOy1WGTfbkIr4F6Xga", "hl2BdI5z", "pyoAXv3F3HUqLutEZaMKeAlBq5hxU", "geuGq", "fN10PnaXj0zykteoRaANxl2", "gvL3B0rzAHfQ2", "kbciDJVRWD0WLZLiXrQZibqUpUBl", "hysdtfXt", "I3hOES1hoe8sT3kPW", "dcPLWPyb58Gup" };

        public float SerializationTimer = 5f;

        public void Start()
        {
            SendCustomEventDelayedSeconds(nameof(SerializationLoop), SerializationTimer);
        }

        public void SerializationLoop()
        {
            if (Networking.IsOwner(gameObject))
            {
                RequestSerialization();
            }
            
            SendCustomEventDelayedSeconds(nameof(SerializationLoop), SerializationTimer);
        }
    }
}