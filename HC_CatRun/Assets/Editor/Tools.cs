using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tools : Editor
{
    [MenuItem("tool/ClearAll")]
    public static void Delete()
    {
        PlayerPrefs.DeleteAll();
    }
}
