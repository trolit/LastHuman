using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

// custom input manager from:
// https://forum.unity.com/threads/can-i-modify-the-input-manager-via-script.458800/

public static class GameInputManager
{ 
    static Dictionary<string, KeyCode> keyMapping;
    static string[] keyMaps = new string[4]
    {
        "Attack",
        "Right",
        "Left",
        "Jump",
    };
    static KeyCode[] defaults = new KeyCode[4]
    {
        KeyCode.F,
        KeyCode.X,
        KeyCode.Z,
        KeyCode.Space,
    };

    static GameInputManager()
    {
        InitializeDictionary();
    }

    private static void InitializeDictionary()
    {
        keyMapping = new Dictionary<string, KeyCode>();
        for (int i = 0; i < keyMaps.Length; ++i)
        {
            keyMapping.Add(keyMaps[i], defaults[i]);
        }
    }

    public static void SetKeyMap(string keyMap, KeyCode key)
    {
        if (!keyMapping.ContainsKey(keyMap))
            throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);
        keyMapping[keyMap] = key;
    }

    public static bool GetKeyDown(string keyMap)
    {
        return Input.GetKeyDown(keyMapping[keyMap]);
    }
}
