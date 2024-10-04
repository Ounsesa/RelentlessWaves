using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class EditorMenu
{
    [MenuItem("Tools/Data Window")]
    public static void OpenPowerUpDataWindow()
    {
        PowerUpDataWindow.InitWindow();
    }
}
#endif
