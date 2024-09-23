using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorMenu : MonoBehaviour
{
    [MenuItem("Tools/Data Window")]
    public static void OpenPowerUpDataWindow()
    {
        PowerUpDataWindow.InitWindow();
    }
    [MenuItem("Tools/NavMesh Window")]
    public static void OpenNavMeshSetUpWindow()
    {
        NavMeshSetUpWindows.InitWindow();
    }
}
