using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86;

enum CSVWindow
{
    PlayerStats,
    EnemyStats,
    PowerUpStats,
}

public class PowerUpDataWindow : EditorWindow
{
    static PowerUpDataWindow window;
    private static string DataFileName = "PowerUpData";
    static List<PowerUpValues> PowerUpValues = new List<PowerUpValues>();
    private CSVWindow CSVWindow = CSVWindow.PlayerStats;
    
    public static void InitWindow()
    {
        window = EditorWindow.GetWindow<PowerUpDataWindow>("PowerUpData");
        window.Show();

        CSVParser.ParseStringListToPowerUpValuesList(DataFileName, out PowerUpValues);
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Player Stats"))
        {
            CSVWindow = CSVWindow.PlayerStats;
        }
        else if(GUILayout.Button("Enemy Stats"))
        {
            CSVWindow = CSVWindow.EnemyStats;
        }
        else if(GUILayout.Button("Power Up Stats"))
        {
            CSVWindow = CSVWindow.PowerUpStats;
        }
        EditorGUILayout.EndHorizontal();

        switch (CSVWindow)
        {
            case CSVWindow.PlayerStats:
                ShowPlayerStats(); break;
            case CSVWindow.EnemyStats:
                ShowEnemyStats(); break;
            case CSVWindow.PowerUpStats:
                ShowPowerUpStats(); break;
        }

        if (GUILayout.Button("Save Data"))
        {
            CSVParser.ParsePowerUpListToCSV(DataFileName, PowerUpValues);
        }
    }

    private void ShowPlayerStats()
    {

    }

    private void ShowEnemyStats()
    {

    }

    private void ShowPowerUpStats()
    {
        EditorGUILayout.BeginVertical();
        for (int i = 0; i < PowerUpValues.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField(PowerUpValues[i].powerUpValue.ToString());
            string newAmount = EditorGUILayout.TextField(PowerUpValues[i].powerUpAmount.ToString());
            string newDuration = EditorGUILayout.TextField(PowerUpValues[i].powerUpDuration.ToString());

            float newAmountFloat;
            if (!(float.TryParse(newAmount, out newAmountFloat) || newAmount == "-"))
            {
                EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Invalid Amount", "Okey");
            }
            else
            {
                PowerUpValues[i].powerUpAmount = newAmountFloat;
            }
            float newDurationFloat;
            if (!(float.TryParse(newDuration, out newDurationFloat) || newDuration == "-"))
            {
                EditorUtility.DisplayDialog("Error: Invalid Number", "Error: Invalid Duration", "Okey");
            }
            else
            {
                PowerUpValues[i].powerUpDuration = newDurationFloat;
            }

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
    }
}
