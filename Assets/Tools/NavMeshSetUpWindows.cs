using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NavMeshSetUpWindows : EditorWindow
{
    static NavMeshSetUpWindows window;

    GameObject[] selectedGOs = new GameObject[0];
    public static void InitWindow()
    {
        window = EditorWindow.GetWindow<NavMeshSetUpWindows>("NavmeshSetUpWindow");
        window.Show();

    }

    private void OnGUI()
    {
        selectedGOs = Selection.gameObjects;

        if(GUILayout.Button("SetNavMeshComponents"))
        {
            foreach (GameObject go in selectedGOs)
            {
                Rigidbody2D[] rbs = go.GetComponents<Rigidbody2D>();
                foreach (Rigidbody2D rbd in rbs)
                {
                    rbd.bodyType = RigidbodyType2D.Static;
                }

                go.AddComponent<NavMeshModifier>();
                go.GetComponent<NavMeshModifier>().overrideArea = true;
                go.GetComponent<NavMeshModifier>().area = 1;
            }
        }
    }

}
