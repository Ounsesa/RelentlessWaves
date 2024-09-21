using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorMenu : MonoBehaviour
{
    [MenuItem("Tools/PowerUp Data Window")]
    public static void OpenPowerUpDataWindow()
    {
        PowerUpDataWindow.InitWindow();
    }
}
